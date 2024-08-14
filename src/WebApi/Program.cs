using CleanArch.WebApi;
using CleanArch.WebApi.SignalR;
using Light.Extensions.DependencyInjection;
using Light.Serilog;
using Serilog;
using Spectre.Console;

SerilogExtensions.EnsureInitialized();

AnsiConsole.Write(new FigletText("eCode API").Color(Color.DodgerBlue1));

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ConfigureSerilog();

    // Add services to the container.

    builder.Services.ConfigureServices(builder.Configuration);

    // SignalR
    builder.Services.AddNotifications();

    builder.Services
        .AddLowercaseControllers()
        .AddDefaultJsonOptions()
        .AddInvalidModelStateHandler();

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.ConfigurePipelines(builder.Configuration);

    app.MapEndpoints(builder.Configuration.GetValue<bool>("AllowAnonymous"));

    // SignalR
    app.UseWebSockets();
    app.MapNotificationHub();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    SerilogExtensions.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}

