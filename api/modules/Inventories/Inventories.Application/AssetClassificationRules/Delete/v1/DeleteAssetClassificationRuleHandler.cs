using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Delete.v1;

public sealed class DeleteAssetClassificationRuleHandler(
    [FromKeyedServices("inventories:assetclassificationrules")] IRepository<AssetClassificationRule> repository)
    : IRequestHandler<DeleteAssetClassificationRuleCommand>
{
    public async Task Handle(
        DeleteAssetClassificationRuleCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AssetClassificationRuleNotFoundException(request.Id);

        await repository.DeleteAsync(rule, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
