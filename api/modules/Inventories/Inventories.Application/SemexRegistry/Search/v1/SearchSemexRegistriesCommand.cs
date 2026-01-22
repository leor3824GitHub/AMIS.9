using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.SemexRegistry.Search.v1;

public sealed record SearchSemexRegistriesCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? ItemCode = null,
    string? Description = null,
    string? Location = null) : IRequest<PagedList<SemexRegistryResponse>>;
