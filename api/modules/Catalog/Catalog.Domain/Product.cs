using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Sku { get; private set; }
    public UnitOfMeasure Unit { get; private set; } = UnitOfMeasure.Piece;
    public string? ImagePath { get; private set; }
    public Guid? CategoryId { get; private set; }
    public virtual Category? Category { get; private set; }


    private Product() { }

    private Product(Guid id, string name, string? description, decimal sku, UnitOfMeasure unit, string? imagePath, Guid? categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        Sku = sku;
        Unit = unit;
        ImagePath = imagePath;
        CategoryId = categoryId;


        QueueDomainEvent(new ProductCreated { Product = this });
    }

    public static Product Create(string name, string? description, decimal sku, UnitOfMeasure unit, string? imagePath, Guid? categoryId)
    {
        return new Product(Guid.NewGuid(), name, description, sku, unit, imagePath, categoryId);
    }

    public Product Update(string? name, string? description, decimal? sku, UnitOfMeasure? unit, string? imagePath, Guid? categoryId)
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

        if (sku.HasValue && Sku != sku.Value)
        {
            Sku = sku.Value;
            isUpdated = true;
        }

        if (categoryId.HasValue && categoryId.Value != Guid.Empty && CategoryId != categoryId.Value)
        {
            CategoryId = categoryId.Value;
            isUpdated = true;
        }

        if (unit.HasValue && Unit != unit.Value)
        {
            Unit = unit.Value;
            isUpdated = true;
        }

        if (!string.Equals(ImagePath, imagePath, StringComparison.OrdinalIgnoreCase))
        {
            ImagePath = imagePath;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ProductUpdated { Product = this });
        }

        return this;
    }

    public Product ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }
}


