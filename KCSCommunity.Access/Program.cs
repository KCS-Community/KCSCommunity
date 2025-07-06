using KCSCommunity.Access.Filters;
using KCSCommunity.Access.Middleware;
using KCSCommunity.Access.Security.ApiSignature;
using KCSCommunity.Application;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using KCSCommunity.Abstractions.Models.Configuration;
using KCSCommunity.Infrastructure.Security.Jwt;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<PasscodeSettings>()
    .Bind(builder.Configuration.GetSection(PasscodeSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Add services from other layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Access layer specific services
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.Configure<ApiSignatureSettings>(builder.Configuration.GetSection(ApiSignatureSettings.SectionName));

builder.Services.AddControllers(options => 
    options.Filters.Add<ApiExceptionFilterAttribute>());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "KCSCommunity API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;
var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<PasscodeSettings>>().Value);

#region 密码配置服务
builder.Services.AddOptions<PasswordPolicySettings>()
    .Bind(builder.Configuration.GetSection(PasswordPolicySettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<PasswordPolicySettings>>().Value);
#endregion

#region i18n
builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "zh-CN", "zh-TW" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});
#endregion

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero // No tolerance for token expiration
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyConstants.OwnerOnly, policy => policy.RequireRole(RoleConstants.Owner));
    options.AddPolicy(PolicyConstants.AdminOrOwner, policy => policy.RequireRole(RoleConstants.Administrator, RoleConstants.Owner));
});


var app = builder.Build();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KCSCommunity API v1"));
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        await scope.ServiceProvider.InitializeDatabaseAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogCritical(ex, "An error occurred during database initialization. The application will not start.");
        return; 
    }
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiSignatureVerificationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();