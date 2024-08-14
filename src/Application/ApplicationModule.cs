using CleanArch.Application.Common.Behaviours;
using CleanArch.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArch.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var sharedAssembly = typeof(SharedModule).Assembly;

        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddValidatorsFromAssembly(sharedAssembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(AppInfo.AppAssemblies);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}