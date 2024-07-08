using CleanArch.eCode.Application;
using CleanArch.eCode.Infrastructure;
using CleanArch.eCode.Infrastructure.Auth;
using Light.ActiveDirectory;
using Light.AspNetCore.Hosting;
using Light.AspNetCore.Hosting.CORS;
using Light.AspNetCore.Hosting.JwtAuth;
using Light.AspNetCore.Hosting.Middlewares;
using Light.AspNetCore.Hosting.Swagger;
using Light.Identity.EntityFrameworkCore;
using Light.Identity.EntityFrameworkCore.Options;

namespace CleanArch.eCode.WebApi;

public static class ConfigureExtensions
{
    private const bool swaggerVersionDefinition = true;

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersion(1);
        services.AddSwagger(configuration, swaggerVersionDefinition);

        services.AddAuth(configuration);

        services.AddCorsPolicies(configuration);

        services.AddGlobalExceptionHandler();

        services
            .AddInfrastructure(configuration)
            .AddApplication();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddCurrentUser();

        services.AddPermissions();

        var sectionName = "JWT";

        // Overide by BindConfiguration
        services.AddOptions<JwtOptions>().BindConfiguration(sectionName);

        services.AddTokenServices();
        services.AddActiveDirectory();

        // add JWT Auth
        var jwtSettings = configuration.GetSection(sectionName).Get<JwtOptions>();
        ArgumentNullException.ThrowIfNull(jwtSettings, nameof(JwtOptions));
        services.AddJwtAuth(jwtSettings.Issuer, jwtSettings.SecretKey); // inject this for use jwt auth

        services.AddActiveDirectory(opt => opt.Name = "aswgroup.net");

        return services;
    }

    public static IApplicationBuilder ConfigurePipelines(this IApplicationBuilder builder, IConfiguration configuration) =>
    builder
        .UseRequestLoggingMiddleware(configuration)
        .UseExceptionHandler()
        .UseRouting()
        .UseCorsPolicies(configuration) // must add before Auth
        .UseAuthentication()
        .UseAuthorization()
        .UseSwagger(configuration, swaggerVersionDefinition);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder, bool allowAnonymous = false)
    {
        if (allowAnonymous)
        {
            builder.MapControllers().AllowAnonymous();
        }
        else
        {
            builder.MapControllers().RequireAuthorization();
        }

        return builder;
    }
}