using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;

public class GetAcceptanceRequest : IRequest<AcceptanceResponse>
{
    public Guid Id { get; set; }

    public GetAcceptanceRequest(Guid id) => Id = id;
}
