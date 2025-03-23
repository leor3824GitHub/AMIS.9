using System.Reflection;
using Asp.Versioning.Conventions;
using FluentValidation;
using AMIS.Framework.Core;
using AMIS.Framework.Core.Origin;
using AMIS.Framework.Infrastructure.Auth;
using AMIS.Framework.Infrastructure.Auth.Jwt;
using AMIS.Framework.Infrastructure.Behaviours;
using AMIS.Framework.Infrastructure.Caching;
using AMIS.Framework.Infrastructure.Cors;
using AMIS.Framework.Infrastructure.Exceptions;
using AMIS.Framework.Infrastructure.Identity;
using AMIS.Framework.Infrastructure.Jobs;
using AMIS.Framework.Infrastructure.Logging.Serilog;
using AMIS.Framework.Infrastructure.Mail;
using AMIS.Framework.Infrastructure.OpenApi;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.Framework.Infrastructure.RateLimit;
using AMIS.Framework.Infrastructure.SecurityHeaders;
using AMIS.Framework.Infrastructure.Storage.Files;
using AMIS.Framework.Infrastructure.Tenant;
using AMIS.Framework.Infrastructure.Tenant.Endpoints;
using AMIS.Aspire.ServiceDefaults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace AMIS.Framework.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder ConfigureFshFramework(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.AddServiceDefaults();
        builder.ConfigureSerilog();
        builder.ConfigureDatabase();
        builder.Services.ConfigureMultitenancy();
        builder.Services.ConfigureIdentity();
        builder.Services.AddCorsPolicy(builder.Configuration);
        builder.Services.ConfigureFileStorage();
        builder.Services.ConfigureJwtAuth();
        builder.Services.ConfigureOpenApi();
        builder.Services.ConfigureJobs(builder.Configuration);
        builder.Services.ConfigureMailing();
        builder.Services.ConfigureCaching(builder.Configuration);
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddHealthChecks();
        builder.Services.AddOptions<OriginOptions>().BindConfiguration(nameof(OriginOptions));

        // Define module assemblies
        var assemblies = new Assembly[]
        {
            typeof(FshCore).Assembly,
            typeof(FshInfrastructure).Assembly
        };

        // Register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        // Register MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        builder.Services.ConfigureRateLimit(builder.Configuration);
        builder.Services.ConfigureSecurityHeaders(builder.Configuration);

        return builder;
    }

    public static WebApplication UseFshFramework(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.UseRateLimit();
        app.UseSecurityHeaders();
        app.UseMultitenancy();
        app.UseExceptionHandler();
        app.UseCorsPolicy();
        app.UseOpenApi();
        app.UseJobDashboard(app.Configuration);
        app.UseRouting();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "assets")),
            RequestPath = new PathString("/assets")
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapTenantEndpoints();
        app.MapIdentityEndpoints();

        // Current user middleware
        app.UseMiddleware<CurrentUserMiddleware>();

        // Register API versions
        var versions = app.NewApiVersionSet()
                    .HasApiVersion(1)
                    .HasApiVersion(2)
                    .ReportApiVersions()
                    .Build();

        // Map versioned endpoint
        app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        return app;
    }
}
