using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Search.v1;

public sealed record SearchAssetRequisitionsCommand(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedList<AssetRequisitionDto>>;

public sealed record AssetRequisitionDto(
    Guid Id,
    Guid EmployeeId,
    Guid IssuanceId,
    DateTime RequisitionDate,
    string Status,
    DateTime? ExpirationDate,
    bool IsExpired);

