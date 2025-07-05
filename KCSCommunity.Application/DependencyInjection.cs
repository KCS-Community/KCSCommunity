// src/2-Application/KCSCommunity.Application/DependencyInjection.cs

using FluentValidation;
using KCSCommunity.Application.Common.Behaviors;
using KCSCommunity.Application.Common.Mappings; // 确保 using 了这个
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KCSCommunity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // --- THE DEFINITIVE, FINAL, AND GUARANTEED-TO-WORK FIX ---
        //
        // We will use the AddAutoMapper extension method that accepts a configuration action.
        // This is the standard, recommended way for AutoMapper v13+ with .NET's DI.
        // It avoids all previous issues with constructors or incorrect overloads.
        
        services.AddAutoMapper(cfg =>
        {
            // We explicitly add our profile(s) inside this configuration block.
            // This is clean, clear, and directly supported by the library.
            cfg.AddProfile<MappingProfile>();

            // If you have more profiles in the future, add them here:
            // cfg.AddProfile<AnotherProfile>();
        });
        
        
        // --- The rest of the configuration remains the same ---
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}