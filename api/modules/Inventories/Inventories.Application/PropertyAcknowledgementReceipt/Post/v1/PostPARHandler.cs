using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Post.v1;

public sealed class PostPARHandler(
    ILogger<PostPARHandler> logger,
    [FromKeyedServices("inventories:par")] IRepository<Domain.PropertyAcknowledgementReceipt> parRepository,
    [FromKeyedServices("inventories:physical-assets")] IRepository<PhysicalAsset> assetRepository,
    IAuthorizationService authorizationService)
    : IRequestHandler<PostPARCommand, PostPARResponse>
{
    public async Task<PostPARResponse> Handle(PostPARCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Post}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized post attempt for PAR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to post Property Accountability Receipts.");
        }

        try
        {
            var par = await parRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (par is null)
            {
                throw new InvalidOperationException($"PAR with Id {request.Id} was not found.");
            }

            par.Post();

            // Assign custodian to each asset in the PAR
            foreach (var lineItem in par.LineItems)
            {
                var spec = new AssetByPropertyCodeSpec(lineItem.PropertyCode);
                var asset = await assetRepository
                    .FirstOrDefaultAsync(spec, cancellationToken)
                    .ConfigureAwait(false);

                if (asset is null)
                {
                    logger.LogWarning(
                        "Asset with PropertyCode {PropertyCode} not found for PAR {PARNumber}",
                        lineItem.PropertyCode,
                        par.PARNumber);
                    throw new InvalidOperationException(
                        $"Asset with PropertyCode '{lineItem.PropertyCode}' was not found. Cannot assign custodian.");
                }

                // Assign the asset to the employee custodian
                asset.AssignToCustodian(par.EmployeeId);
                await assetRepository.UpdateAsync(asset, cancellationToken).ConfigureAwait(false);

                logger.LogInformation(
                    "Asset {PropertyCode} assigned to custodian {EmployeeName} via PAR {PARNumber}",
                    lineItem.PropertyCode,
                    par.EmployeeName,
                    par.PARNumber);
            }

            await parRepository.UpdateAsync(par, cancellationToken);
            await parRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PAR {PARNumber} posted successfully with {Count} assets assigned",
                par.PARNumber, par.LineItems.Count);

            return new PostPARResponse(
                par.Id,
                par.PARNumber,
                par.Status.ToString(),
                par.LastModified.DateTime);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting PAR {Id}", request.Id);
            throw;
        }
    }
}
