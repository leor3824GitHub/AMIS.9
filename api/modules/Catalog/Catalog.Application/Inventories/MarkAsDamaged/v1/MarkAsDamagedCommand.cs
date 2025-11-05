using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.MarkAsDamaged.v1;

public sealed record MarkAsDamagedCommand(Guid InventoryId) : IRequest<MarkAsDamagedResponse>;
