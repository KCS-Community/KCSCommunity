using KCSCommunity.Access.Security.ApiSignature;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace KCSCommunity.Access.Middleware;

public class ApiSignatureVerificationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiSignatureSettings _settings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ApiSignatureVerificationMiddleware> _logger;
    private static readonly HashSet<string> _publicPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/auth/login",
        "/api/users/activate"
    };

    public ApiSignatureVerificationMiddleware(RequestDelegate next, IOptions<ApiSignatureSettings> settings, IMemoryCache cache, ILogger<ApiSignatureVerificationMiddleware> logger)
    {
        _next = next; _settings = settings.Value; _cache = cache; _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_settings.Enabled || context.Request.Path.StartsWithSegments("/swagger") || IsPublicPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-ApiKey", out var apiKey) ||
            !context.Request.Headers.TryGetValue("X-Timestamp", out var timestampStr) ||
            !context.Request.Headers.TryGetValue("X-Nonce", out var nonce) ||
            !context.Request.Headers.TryGetValue("X-Signature", out var clientSignature))
        {
            _logger.LogWarning("API Signature verification failed: Missing required headers.");
            await WriteUnauthorizedResponse(context, "Missing signature headers.");
            return;
        }

        var keyPair = _settings.ApiKeys.FirstOrDefault(k => k.Key == apiKey.ToString());
        if (keyPair == null)
        {
            await WriteUnauthorizedResponse(context, "Invalid ApiKey.");
            return;
        }

        if (!long.TryParse(timestampStr, out var timestamp) || Math.Abs((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timestamp)) > _settings.TimeWindowSeconds)
        {
            await WriteUnauthorizedResponse(context, "Request timestamp is outside the valid time window.");
            return;
        }

        if (_cache.TryGetValue(nonce.ToString(), out _))
        {
            await WriteUnauthorizedResponse(context, "Replay attack detected.");
            return;
        }

        var serverSignature = await GenerateServerSignature(context.Request, timestampStr.ToString(), nonce.ToString(), keyPair.Secret);
        if (!string.Equals(serverSignature, clientSignature.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            await WriteUnauthorizedResponse(context, "Invalid signature.");
            return;
        }

        _cache.Set(nonce.ToString(), true, TimeSpan.FromSeconds(_settings.TimeWindowSeconds));
        await _next(context);
    }

    private static bool IsPublicPath(PathString path) => _publicPaths.Contains(path.Value ?? string.Empty);
    
    private static async Task<string> GenerateServerSignature(HttpRequest request, string timestamp, string nonce, string apiSecret)
    {
        request.EnableBuffering();
        string requestBody = string.Empty;
        if (request.ContentLength > 0 && request.ContentType?.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase) != true)
        {
            using (var reader = new StreamReader(request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }
        }
        
        var sortedQuery = QueryHelpers.ParseQuery(request.QueryString.ToString()).OrderBy(q => q.Key)
            .SelectMany(q => q.Value, (k, v) => new { k.Key, Value = v })
            .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value ?? "")}");
        var canonicalQueryString = string.Join("&", sortedQuery);
        var pathAndQuery = request.Path + (string.IsNullOrEmpty(canonicalQueryString) ? "" : "?" + canonicalQueryString);
        
        var rawData = $"{request.Method.ToUpper()}{pathAndQuery}{timestamp}{nonce}{requestBody}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(hash);
    }
    
    private static Task WriteUnauthorizedResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return context.Response.WriteAsJsonAsync(new ProblemDetails { Title = "Unauthorized", Detail = message, Status = 401 });
    }
}