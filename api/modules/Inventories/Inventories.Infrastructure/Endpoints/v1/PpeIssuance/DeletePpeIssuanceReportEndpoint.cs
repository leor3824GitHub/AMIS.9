using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class DeletePpeIssuanceReportEndpoint
{
    public static RouteHandlerBuilder MapDeletePpeIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var command = new Application.PpeIssuance.Delete.v1.DeletePpeIssuanceReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePpeIssuanceReportEndpoint))
            .WithSummary("Delete a PPE Issuance Report")
            .WithDescription("Deletes a draft PPEIR. Only draft reports can be deleted.")
            .Produces<Application.PpeIssuance.Delete.v1.DeletePpeIssuanceReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.PpeIssuance.Delete")
            .MapToApiVersion(1);
    }
}
