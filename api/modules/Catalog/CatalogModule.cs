using Carter;
using Catalog.Application.Services.v1;
using FSH.WebApi.Modules.Catalog.Features.Products.ProductCreation.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Services;
using AMIS.WebApi.Catalog.Infrastructure.Persistence.Repositories;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints;

namespace FSH.WebApi.Modules.Catalog;

public static class CatalogModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("catalog") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("products").WithTags("products");
            productGroup.MapProductCreationEndpoint();

            // PPE Type Mappings endpoints
            app.MapPPETypeMappingEndpoints();

            var testGroup = app.MapGroup("test").WithTags("test");
            testGroup.MapGet("/test", () => "hi");
        }
    }
    public static WebApplicationBuilder RegisterCatalogServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        // Register audit service
        builder.Services.AddScoped<IInspectionAuditService, InspectionAuditService>();
        
        // Register PPE account code mapping services
        builder.Services.AddScoped<DefaultPPEAccountCodeMapper>();
        builder.Services.AddScoped<IPPEAccountCodeMapper, DatabasePPEAccountCodeMapper>();
        builder.Services.AddScoped<IPPETypeAccountMappingRepository, PPETypeAccountMappingRepository>();
        
        return builder;
    }
    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        return app;
    }
}
