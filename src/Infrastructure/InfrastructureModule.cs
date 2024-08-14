using CleanArch.Application.Common.Interfaces;
using CleanArch.Infrastructure.Identity;
using CleanArch.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTime, DateTimeService>();

        services.AddInfrastructureIdentity(configuration);

        return services;
    }
}