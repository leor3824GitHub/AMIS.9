using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Return.v1;

public sealed class ReturnPARHandler(
    ILogger<ReturnPARHandler> logger,
    [FromKeyedServices("inventories:par")] IRepository<Domain.PropertyAcknowledgementReceipt> parRepository,
    [FromKeyedServices("inventories:physical-assets")] IRepository<PhysicalAsset> assetRepository,
    IAuthorizationService authorizationService)
    : IRequestHandler<ReturnPARCommand, ReturnPARResponse>
{
    public async Task<ReturnPARResponse> Handle(ReturnPARCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Update}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized return attempt for PAR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to process PAR returns.");
        }

        try
        {
            var par = await parRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (par is null)
            {
                throw new InvalidOperationException($"PAR with Id {request.Id} was not found.");
            }

            par.Return(
                request.ReturnDate,
                request.ReceivedByEmployeeId,
                request.ReceivedByEmployeeName,
                request.ReturnRemarks);

            // Unassign custodian from each asset in the PAR (assets returned)
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
                        "Asset {PropertyCode} returned and custodian unassigned via PAR {PARNumber}",
                        lineItem.PropertyCode,
                        par.PARNumber);
                }
            }

            await parRepository.UpdateAsync(par, cancellationToken);
            await parRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PAR {PARNumber} returned successfully", par.PARNumber);

            return new ReturnPARResponse(
                par.Id,
                par.PARNumber,
                par.Status.ToString(),
                par.LastModified);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing return for PAR {Id}", request.Id);
            throw;
        }
    }
}
