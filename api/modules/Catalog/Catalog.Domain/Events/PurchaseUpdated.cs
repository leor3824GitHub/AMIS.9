using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record PurchaseUpdated : DomainEvent
{
    public Purchase? Purchase { get; set; }
}
