using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkInProgress.v1;

public sealed record MarkInProgressCommand(Guid PurchaseId) : IRequest<MarkInProgressResponse>;
