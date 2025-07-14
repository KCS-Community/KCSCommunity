using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Application.Shared.Behaviors;
using KCSCommunity.Application.Shared.Mappings;
using KCSCommunity.Application.Shared.Validators;

namespace KCSCommunity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        
        ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) => 
            member != null ? member.Name : null;
            
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.LanguageManager.Enabled = true;
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IUserStatusValidator, UserStatusValidator>();
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}