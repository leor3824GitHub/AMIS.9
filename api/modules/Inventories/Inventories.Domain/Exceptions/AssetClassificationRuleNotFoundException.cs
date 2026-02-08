using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;

public sealed class AssetClassificationRuleNotFoundException : NotFoundException
{
    public AssetClassificationRuleNotFoundException(Guid id)
        : base($"asset classification rule with id {id} not found")
    {
    }
}
