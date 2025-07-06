using FluentValidation;
using KCSCommunity.Application.Common.Behaviors;
using KCSCommunity.Application.Common.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using KCSCommunity.Application.Resources;

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
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}