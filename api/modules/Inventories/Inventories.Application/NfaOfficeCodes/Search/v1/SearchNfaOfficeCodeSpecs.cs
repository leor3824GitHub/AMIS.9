using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Search.v1;

public sealed class SearchNfaOfficeCodeSpecs : EntitiesByPaginationFilterSpec<NfaOfficeCode, NfaOfficeCodeResponse>
{
    public SearchNfaOfficeCodeSpecs(SearchNfaOfficeCodesCommand command)
        : base(command)
    {
        if (!string.IsNullOrWhiteSpace(command.Keyword))
        {
            var keyword = command.Keyword.Trim();
            Query.Where(x =>
                x.Code.Contains(keyword) ||
                x.OfficeName.Contains(keyword) ||
                (x.Description != null && x.Description.Contains(keyword)));
        }

        Query
            .OrderBy(x => x.Code, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.OfficeName.Contains(command.OfficeName!), !string.IsNullOrWhiteSpace(command.OfficeName))
            .Where(x => x.ParentOfficeCode == command.ParentOfficeCode, !string.IsNullOrWhiteSpace(command.ParentOfficeCode))
            .Where(x => x.IsActive == command.IsActive, command.IsActive.HasValue);
    }
}
