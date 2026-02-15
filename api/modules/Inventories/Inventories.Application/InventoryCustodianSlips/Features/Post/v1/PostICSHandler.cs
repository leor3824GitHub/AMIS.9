using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Post.v1;

public sealed class PostICSHandler(
    ILogger<PostICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<PostICSCommand, PostICSResponse>
{
    public async Task<PostICSResponse> Handle(PostICSCommand request, CancellationToken cancellationToken)
    {
        var ics = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (ics == null)
        {
            logger.LogWarning("ICS with ID {ICSId} not found", request.Id);
            throw new InvalidOperationException($"ICS with ID {request.Id} not found");
        }

        logger.LogInformation("Posting ICS {ICSNumber}", ics.ICSNumber);

        ics.Post();

        await repository.UpdateAsync(ics, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("ICS {ICSNumber} posted successfully", ics.ICSNumber);

        return new PostICSResponse(ics.Id, "ICS posted successfully");
    }
}
