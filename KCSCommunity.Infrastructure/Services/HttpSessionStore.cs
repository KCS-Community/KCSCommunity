using KCSCommunity.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace KCSCommunity.Infrastructure.Services;

public class HttpSessionStore : ISessionStore
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    
    public HttpSessionStore(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext?.Session ?? throw new InvalidOperationException("Session is not available in the current context.");

    public void Set<T>(string key, T value)
    {
        Session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions));
    }

    public T? Get<T>(string key)
    {
        var data = Session.Get(key);
        return data == null ? default : JsonSerializer.Deserialize<T>(data, JsonOptions);
    }

    public void Remove(string key)
    {
        Session.Remove(key);
    }
}