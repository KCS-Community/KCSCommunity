// src/3-Infrastructure/KCSCommunity.Infrastructure/Persistence/DbInitializer.cs

using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KCSCommunity.Infrastructure.Persistence;

public static class DbInitializer
{
public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(DbInitializer));
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        try
        {
            //初始化器的唯一职责是应用迁移
            
            var autoMigrate = configuration.GetValue<bool>("Database:AutoMigrateOnStartup", true);
            if (!autoMigrate)
            {
                logger.LogInformation("Automatic migration is disabled. Skipping database initialization.");
                return;
            }

            logger.LogInformation("Starting database migration check...");

            //如果数据库已经有了，它就会应用挂起的迁移。要是数据库还没建立就会失败，抛出异常
            await context.Database.MigrateAsync();

            logger.LogInformation("Database is up-to-date.");

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await SeedRolesAsync(roleManager, logger);
            await SeedOwnerUserAsync(userManager, configuration, logger);

            logger.LogInformation("Database initialization and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "A critical error occurred while migrating or seeding the database. Please ensure the database server is running and the database has been created.");
            throw;
        }
    }
    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger)
    {
        foreach (var roleName in RoleConstants.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                logger.LogInformation("Role '{RoleName}' created.", roleName);
            }
        }
    }

private static async Task SeedOwnerUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger logger)
    {
        var ownerUserName = configuration["DefaultOwner:UserName"];
        if (string.IsNullOrEmpty(ownerUserName))
        {
            logger.LogTrace("DefaultOwner:UserName not configured. Skipping owner user creation.");
            return;
        }

        if (await userManager.FindByNameAsync(ownerUserName) != null)
        {
            logger.LogDebug("Default owner user '{UserName}' already exists.", ownerUserName);
            return;
        }

        var ownerEmail = configuration["DefaultOwner:Email"];
        var ownerPassword = configuration["DefaultOwner:Password"];

        if (string.IsNullOrEmpty(ownerEmail) || string.IsNullOrEmpty(ownerPassword))
        {
            logger.LogWarning("Default Owner user is not fully configured (missing Email or Password). Skipping creation.");
            return;
        }

        var owner = ApplicationUser.CreateNewUser(
            ownerUserName, /*ownerEmail,*/ "System Owner", "Owner", /*Domain.Enums.Gender.Other,*/
            /*new DateTime(1970, 1, 1),*/ Domain.Enums.UserRoleType.Staff, null, null, "Owner"
        );
        
        owner.ActivateAccount();

        var createResult = await userManager.CreateAsync(owner, ownerPassword);
        if (!createResult.Succeeded)
        {
            //创建失败
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            logger.LogCritical("Failed to create default Owner user. Errors: {Errors}", errors);
            throw new Exception($"Fatal error: Could not create the default owner user. Please check logs. Errors: {errors}");
        }

        var addToRoleResult = await userManager.AddToRoleAsync(owner, RoleConstants.Owner);
        if (!addToRoleResult.Succeeded)
        {
            var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
            logger.LogCritical("Failed to add default owner to Owner role. Errors: {Errors}", errors);
            throw new Exception($"Fatal error: Could not assign Owner role to the default user. Errors: {errors}");
        }

        logger.LogInformation("Default Owner user '{UserName}' created and assigned to Owner role successfully.", ownerUserName);
    }}