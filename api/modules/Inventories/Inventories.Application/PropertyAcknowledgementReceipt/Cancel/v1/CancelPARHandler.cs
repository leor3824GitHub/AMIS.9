using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Cancel.v1;

public sealed class CancelPARHandler(
    ILogger<CancelPARHandler> logger,
    [FromKeyedServices("inventories:par")] IRepository<Domain.PropertyAcknowledgementReceipt> parRepository,
    [FromKeyedServices("inventories:physical-assets")] IRepository<PhysicalAsset> assetRepository,
    IAuthorizationService authorizationService)
    : IRequestHandler<CancelPARCommand, CancelPARResponse>
{
    public async Task<CancelPARResponse> Handle(CancelPARCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check - only accounting can cancel
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Update}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized cancel attempt for PAR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "Only Accounting personnel can cancel Property Accountability Receipts.");
        }

        try
        {
            var par = await parRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (par is null)
            {
                throw new InvalidOperationException($"PAR with Id {request.Id} was not found.");
            }

            par.Cancel();

            // Unassign custodian from each asset in the PAR (reverse the assignment)
            foreach (var lineItem in par.LineItems)
            {
                var spec = new AssetByPropertyCodeSpec(lineItem.PropertyCode);
                var asset = await assetRepository
                    .FirstOrDefaultAsync(spec, cancellationToken)
                    .ConfigureAwait(false);

                if (asset is not null)
                {
                    asset.ClearCustodian();
                    await assetRepository.UpdateAsync(asset, cancellationToken).ConfigureAwait(false);

                    logger.LogInformation(
                        "Asset {PropertyCode} custodian unassigned due to PAR {PARNumber} cancellation",
                        lineItem.PropertyCode,
                        par.PARNumber);
                }
            }

            await parRepository.UpdateAsync(par, cancellationToken);
            await parRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PAR {PARNumber} cancelled successfully", par.PARNumber);

            return new CancelPARResponse(
                par.Id,
                par.PARNumber,
                par.Status.ToString(),
                par.LastModified.DateTime);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cancelling PAR {Id}", request.Id);
            throw;
        }
    }
}
