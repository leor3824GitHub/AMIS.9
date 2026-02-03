using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Search.v1;

public sealed class SearchPpeCategoryCodeSpecs : EntitiesByPaginationFilterSpec<PpeCategoryCode, PpeCategoryCodeResponse>
{
    public SearchPpeCategoryCodeSpecs(SearchPpeCategoryCodesCommand command)
        : base(command)
    {
        if (!string.IsNullOrWhiteSpace(command.Keyword))
        {
            var keyword = command.Keyword.Trim();
            Query.Where(x =>
                x.Code.Contains(keyword) ||
                x.Name.Contains(keyword) ||
                x.AccountCode.Contains(keyword) ||
                (x.Description != null && x.Description.Contains(keyword)));
        }

        Query
            .OrderBy(x => x.Code, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.Name.Contains(command.Name!), !string.IsNullOrWhiteSpace(command.Name))
            .Where(x => x.AccountCode.Contains(command.AccountCode!), !string.IsNullOrWhiteSpace(command.AccountCode))
            .Where(x => x.IsActive == command.IsActive, command.IsActive.HasValue);
    }
}
