using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.MarkAsObsolete.v1;

public sealed record MarkAsObsoleteCommand(Guid InventoryId) : IRequest<MarkAsObsoleteResponse>;
