using Ardalis.Specification;
using AMIS.WebApi.Catalog.Application.AssetRequisitions.Search.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AssetRequisitions.Search.v1;

public sealed class SearchAssetRequisitionsSpecs : Specification<AssetRequisition, AssetRequisitionDto>
{
    public SearchAssetRequisitionsSpecs(SearchAssetRequisitionsCommand request)
    {
        Query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderByDescending(x => x.Created);

        Query
            .Select(r => new AssetRequisitionDto(
                r.Id,
                r.EmployeeId,
                r.IssuanceId,
                r.RequisitionDate,
                r.Status.ToString(),
                r.ExpirationDate,
                r.IsExpired));
    }
}
