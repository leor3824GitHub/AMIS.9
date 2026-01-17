using MediatR;

namespace AMIS.WebApi.Inventories.Application.Issuances.Get.v1;
public class GetIssuanceRequest : IRequest<IssuanceResponse>
{
    public Guid Id { get; set; }
    public GetIssuanceRequest(Guid id) => Id = id;
}

