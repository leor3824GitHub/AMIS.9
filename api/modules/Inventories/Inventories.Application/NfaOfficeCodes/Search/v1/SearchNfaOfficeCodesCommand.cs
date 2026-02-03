using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Search.v1;

public sealed class SearchNfaOfficeCodesCommand : PaginationFilter, IRequest<PagedList<NfaOfficeCodeResponse>>
{
    public string? Code { get; set; }
    public string? OfficeName { get; set; }
    public string? ParentOfficeCode { get; set; }
    public bool? IsActive { get; set; }
}
