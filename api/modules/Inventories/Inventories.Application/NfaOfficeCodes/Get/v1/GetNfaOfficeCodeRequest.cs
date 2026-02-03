using MediatR;

namespace AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;

public sealed class GetNfaOfficeCodeRequest : IRequest<NfaOfficeCodeResponse>
{
    public Guid Id { get; }

    public GetNfaOfficeCodeRequest(Guid id) => Id = id;
}
