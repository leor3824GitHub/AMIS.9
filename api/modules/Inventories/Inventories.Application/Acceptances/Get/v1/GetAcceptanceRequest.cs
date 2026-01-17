using MediatR;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Get.v1;

public class GetAcceptanceRequest : IRequest<AcceptanceResponse>
{
    public Guid Id { get; set; }

    public GetAcceptanceRequest(Guid id) => Id = id;
}

