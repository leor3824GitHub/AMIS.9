using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Issue.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

/// <summary>
/// Endpoint to issue a physical asset to an employee
/// </summary>
public static class IssuePhysicalAssetEndpoint
{
    internal static RouteHandlerBuilder MapPhysicalAssetIssueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/issue", async (Guid id, [FromBody] IssuePhysicalAssetCommand command, ISender mediator) =>
            {
                var issueCommand = new IssuePhysicalAssetCommand
                {
                    Id = id,
                    EmployeeId = command.EmployeeId,
                    EmployeeName = command.EmployeeName,
                    DocumentNumber = command.DocumentNumber,
                    QuantityIssued = command.QuantityIssued,
                    Location = command.Location
                };
                var response = await mediator.Send(issueCommand);
                return Results.Ok(response);
            })
            .WithName(nameof(IssuePhysicalAssetEndpoint))
            .WithSummary("Issues a physical asset to an employee")
            .WithDescription("Issues a physical asset via ICS (Semi-Expendable) or PAR (PPE) document")
            .Produces<IssuePhysicalAssetResponse>()
            .RequirePermission("Permissions.PhysicalAssets.Issue")
            .MapToApiVersion(1);
    }
}

