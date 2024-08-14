using CleanArch.Application;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Auth;
using Light.ActiveDirectory;
using Light.AspNetCore.Builder;
using Light.AspNetCore.CORS;
using Light.AspNetCore.Middlewares;
using Light.AspNetCore.Swagger;
using Light.Extensions.DependencyInjection;
using Light.Identity.EntityFrameworkCore;
using Light.Identity.EntityFrameworkCore.Options;

namespace CleanArch.WebApi;

public static class ConfigureExtensions
{
    private const bool swaggerVersionDefinition = true;

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersion(1);
        services.AddSwagger(configuration, swaggerVersionDefinition);

        services.AddAuth(configuration);

        services.AddCorsPolicies(configuration);

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
        .UseMiddleware<TraceIdMiddleware>()
        .UseRequestLoggingMiddleware(configuration)
        .UseExceptionHandlerMiddleware()
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