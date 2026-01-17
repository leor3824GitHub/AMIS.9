using AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Create.v1;
using AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Get.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Modules.Catalog.Infrastructure.Endpoints.MaterialsIssuance;

public class SuppliesAndMaterialsIssuanceReportEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/materials-issuance")
            .WithTags("Materials Issuance")
            .WithName("MaterialsIssuance");

        group.MapPost("/", CreateSuppliesAndMaterialsIssuanceReport)
            .WithName(nameof(CreateSuppliesAndMaterialsIssuanceReport))
            .WithOpenApi()
            .Produces<CreateSuppliesAndMaterialsIssuanceReportResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .MapToApiVersion(new ApiVersion(1, 0));

        group.MapGet("/{id}", GetSuppliesAndMaterialsIssuanceReportById)
            .WithName(nameof(GetSuppliesAndMaterialsIssuanceReportById))
            .WithOpenApi()
            .Produces<GetSuppliesAndMaterialsIssuanceReportByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    private static async Task<IResult> CreateSuppliesAndMaterialsIssuanceReport(
        CreateSuppliesAndMaterialsIssuanceReportCommand request,
        ISender mediator,
        HttpContext httpContext)
    {
        var response = await mediator.Send(request);
        return Results.CreatedAtRoute(
            nameof(GetSuppliesAndMaterialsIssuanceReportById),
            new { id = response.Id },
            response);
    }

    private static async Task<IResult> GetSuppliesAndMaterialsIssuanceReportById(
        Guid id,
        ISender mediator)
    {
        var response = await mediator.Send(new GetSuppliesAndMaterialsIssuanceReportByIdQuery(id));
        return Results.Ok(response);
    }
}
