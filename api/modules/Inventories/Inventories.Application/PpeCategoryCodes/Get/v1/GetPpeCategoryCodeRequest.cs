using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;

public sealed class GetPpeCategoryCodeRequest : IRequest<PpeCategoryCodeResponse>
{
    public Guid Id { get; }

    public GetPpeCategoryCodeRequest(Guid id) => Id = id;
}
