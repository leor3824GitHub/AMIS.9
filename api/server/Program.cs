using AMIS.Framework.Infrastructure;
using AMIS.Framework.Infrastructure.Logging.Serilog;
using AMIS.WebApi.Host;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("server booting up..");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureFshFramework();
    builder.RegisterModules();

    var app = builder.Build();

    app.UseFshFramework();
    app.UseModules();
    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex.Message, "unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("server shutting down..");
    await Log.CloseAndFlushAsync();
}
