using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Application.Shared.Behaviors;
using KCSCommunity.Application.Shared.Mappings;
using KCSCommunity.Application.Shared.Validators;

namespace KCSCommunity.Application.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationAuthServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IUserStatusValidator, UserStatusValidator>();
        services.AddScoped<IPasskeyOptionsValidator, PasskeyOptionsValidator>();
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}