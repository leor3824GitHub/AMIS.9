using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;

public class GetAssetClassificationRuleRequest : IRequest<AssetClassificationRuleResponse>
{
    public Guid Id { get; set; }
    public GetAssetClassificationRuleRequest(Guid id) => Id = id;
}
