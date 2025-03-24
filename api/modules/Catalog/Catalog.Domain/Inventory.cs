using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Inventory : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public string? Location { get; private set; }
    public decimal Qty { get; private set; }
    public decimal AvePrice { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private Inventory() { }

    private Inventory(Guid id, string name, string? description, decimal price, Guid? brandId)
    {
        Id = id;
        ProductId = id;
        Location = name;
        Qty = price;
        AvePrice = price;
        Inventory = new Inventory { Name = name, Description = description, Price = price, BrandId = brandId };

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public static Inventory Create(string name, string? description, decimal price, Guid? brandId)
    {
        return new Inventory(Guid.NewGuid(), name, description, price, brandId);
    }

    public Inventory Update(string? name, string? description, decimal? price, Guid? brandId)
    {
        bool isUpdated = false;

        if (!string.Equals(Product.Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Inventory.Description = description;
            isUpdated = true;
        }

        if (price.HasValue && Product.Price != price.Value)
        {
            Inventory.Price = price.Value;
            isUpdated = true;
        }

        if (brandId.HasValue && brandId.Value != Guid.Empty && Inventory.BrandId != brandId.Value)
        {
            Inventory.BrandId = brandId.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryUpdated { Inventory = this });
        }

        return this;
    }
}

