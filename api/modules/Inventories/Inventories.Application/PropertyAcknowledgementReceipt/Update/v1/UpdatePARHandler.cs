using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Update.v1;

public sealed class UpdatePARHandler(
    ILogger<UpdatePARHandler> logger,
    [FromKeyedServices("inventories:par")] IRepository<Domain.PropertyAcknowledgementReceipt> repository,
    IAuthorizationService authorizationService)
    : IRequestHandler<UpdatePARCommand, UpdatePARResponse>
{
    public async Task<UpdatePARResponse> Handle(UpdatePARCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Update}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized update attempt for PAR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to update Property Accountability Receipts.");
        }

        var par = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (par is null)
        {
            logger.LogWarning("PAR {Id} not found for update", request.Id);
            throw new InvalidOperationException($"PAR {request.Id} not found.");
        }

        // Status validation - only allow updates in Draft status
        if (par.Status == Domain.PARStatus.Posted)
        {
            throw new InvalidOperationException(
                "Posted PARs cannot be updated. Only Accounting personnel can modify posted records. Please contact Accounting department for changes.");
        }

        if (par.Status == Domain.PARStatus.Returned || par.Status == Domain.PARStatus.Cancelled)
        {
            throw new InvalidOperationException(
                $"Cannot update PAR in {par.Status} status. Only Draft PARs can be updated.");
        }

        par.UpdateHeader(
            request.EmployeeId,
            request.EmployeeName,
            request.Department,
            request.IssuanceDate,
            request.Position,
            request.IssuancePurpose,
            request.IssuanceLocation,
            request.Notes);

        par.ClearLineItems();
        
        var lineItems = request.LineItems.Select(x => new Domain.PARLineItem(
            x.PropertyCode,
            x.Description,
            x.DateAcquired,
            x.AcquisitionCost,
            x.Condition,
            x.Remarks)).ToList();

        par.AddLineItems(lineItems);

        await repository.UpdateAsync(par, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("PAR {PARNumber} updated successfully", par.PARNumber);
        return new UpdatePARResponse(par.Id, par.PARNumber, par.LastModified);
    }
}
