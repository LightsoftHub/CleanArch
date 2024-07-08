using CleanArch.eCode.Application.Common.Interfaces;
using CleanArch.eCode.Infrastructure.Identity;
using CleanArch.eCode.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.eCode.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTime, DateTimeService>();

        services.AddInfrastructureIdentity(configuration);

        return services;
    }
}