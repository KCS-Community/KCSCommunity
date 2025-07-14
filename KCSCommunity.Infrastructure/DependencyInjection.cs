using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Models.Configuration;
using KCSCommunity.Domain.Entities;
using KCSCommunity.Infrastructure.Persistence;
using KCSCommunity.Infrastructure.Security.Hashing;
using KCSCommunity.Infrastructure.Security.Jwt;
using KCSCommunity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KCSCommunity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //JWT Settings
        //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        //services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
        
        //数据库
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ISessionStore, HttpSessionStore>();

        var passwordPolicy = new PasswordPolicySettings();
        configuration.GetSection(PasswordPolicySettings.SectionName).Bind(passwordPolicy);
        
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = passwordPolicy.RequiredLength;
                options.Password.RequireDigit = passwordPolicy.RequireDigit;
                options.Password.RequireLowercase = passwordPolicy.RequireLowercase;
                options.Password.RequireUppercase = passwordPolicy.RequireUppercase;
                options.Password.RequireNonAlphanumeric = passwordPolicy.RequireNonAlphanumeric;
            
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();//password reset/2FA tokens

        //服务
        services.AddScoped<IPasswordHasher<ApplicationUser>, Argon2PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasskeyService, Fido2PasskeyService>();
        
        // 锁，后期考虑Redis-based
        services.AddSingleton<IResourceLockService, InMemoryLockService>();

        return services;
    }
    
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        await DbInitializer.InitializeAsync(serviceProvider);
    }
}