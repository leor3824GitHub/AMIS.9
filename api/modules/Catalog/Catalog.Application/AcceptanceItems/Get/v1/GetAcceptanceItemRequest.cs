using MediatR;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;

public class GetAcceptanceItemRequest : IRequest<AcceptanceItemResponse>
{
    public Guid Id { get; set; }

    public GetAcceptanceItemRequest(Guid id) => Id = id;
}
