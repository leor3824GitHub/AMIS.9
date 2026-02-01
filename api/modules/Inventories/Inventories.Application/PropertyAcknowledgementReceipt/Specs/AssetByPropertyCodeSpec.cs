using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;

public sealed class AssetByPropertyCodeSpec : Specification<PhysicalAsset>
{
    public AssetByPropertyCodeSpec(string propertyCode)
    {
        Query.Where(a => a.PropertyCode == propertyCode);
    }
}
