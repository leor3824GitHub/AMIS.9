using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Domain.Exceptions; // in case specific exceptions are added later

namespace AMIS.WebApi.Catalog.Application.Acceptances.Post.v1;

public sealed class PostAcceptanceHandler(
    ILogger<PostAcceptanceHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> repository)
    : IRequestHandler<PostAcceptanceCommand, PostAcceptanceResponse>
{
    public async Task<PostAcceptanceResponse> Handle(PostAcceptanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var acceptance = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"Acceptance {request.Id} not found");
        acceptance.PostAcceptance();
        await repository.UpdateAsync(acceptance, cancellationToken);
        logger.LogInformation("Acceptance {AcceptanceId} posted.", acceptance.Id);
        return new PostAcceptanceResponse(acceptance.Id);
    }
}
