using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReleaseFromQuarantine.v1;

public sealed record ReleaseFromQuarantineCommand(Guid InventoryId) : IRequest<ReleaseFromQuarantineResponse>;
