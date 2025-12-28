using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain.ProcurementProjects;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Search.v1;

public sealed class SearchProcurementProjectsSpec : Specification<ProcurementProject>
{
    public SearchProcurementProjectsSpec(SearchProcurementProjectsCommand command)
    {
        Query.OrderByDescending(x => x.Created);

        if (!string.IsNullOrWhiteSpace(command.ProjectTitle))
        {
            Query.Where(x => x.ProjectTitle.Contains(command.ProjectTitle));
        }

        if (!string.IsNullOrWhiteSpace(command.PmoEndUser))
        {
            Query.Where(x => x.PmoEndUser.Contains(command.PmoEndUser));
        }

        if (!string.IsNullOrWhiteSpace(command.FundSource))
        {
            Query.Where(x => x.FundSource.Contains(command.FundSource));
        }
    }
}
