using MediatR;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Search.v1;

public sealed record SearchPhysicalAssetsCommand(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedList<PhysicalAssetResponse>>;

