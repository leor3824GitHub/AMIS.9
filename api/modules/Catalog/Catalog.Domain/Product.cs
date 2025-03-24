using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal SKU { get; private set; }
    public Guid? CategoryId { get; private set; }
    public virtual Category Category { get; private set; } = default!;

    private Product() { }

    private Product(Guid id, string name, string? description, decimal sku, Guid? categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        SKU = sku;
        CategoryId = categoryId;

        QueueDomainEvent(new ProductCreated { Product = this });
    }

    public static Product Create(string name, string? description, decimal sku, Guid? categoryId)
    {
        return new Product(Guid.NewGuid(), name, description, sku, categoryId);
    }

    public Product Update(string? name, string? description, decimal? sku, Guid? categoryId)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (sku.HasValue && SKU != sku.Value)
        {
            SKU = sku.Value;
            isUpdated = true;
        }

        if (categoryId.HasValue && categoryId.Value != Guid.Empty && CategoryId != categoryId.Value)
        {
            CategoryId = categoryId.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ProductUpdated { Product = this });
        }

        return this;
    }
}

