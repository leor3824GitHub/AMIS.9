using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string UnitOfMeasure { get; private set; } = "piece"; // Default unit of measure for product
    public int EstimatedUsefulLife { get; private set; } // in months
    public Guid? CategoryId { get; private set; }
    public virtual Category? Category { get; private set; }


    private Product() { }

    private Product(Guid id, string name, string? description, string unitOfMeasure, int estimatedUsefulLife, Guid? categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        UnitOfMeasure = unitOfMeasure;
        EstimatedUsefulLife = estimatedUsefulLife;
        CategoryId = categoryId;

        QueueDomainEvent(new ProductCreated { Product = this });
    }

    public static Product Create(string name, string? description, string unitOfMeasure = "piece", int estimatedUsefulLife = 12, Guid? categoryId = null)
    {
        return new Product(Guid.NewGuid(), name, description, unitOfMeasure, estimatedUsefulLife, categoryId);
    }

    public Product Update(string? name, string? description, string? unitOfMeasure = null, int? estimatedUsefulLife = null, Guid? categoryId = null)
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

        if (!string.IsNullOrWhiteSpace(unitOfMeasure) && !string.Equals(UnitOfMeasure, unitOfMeasure, StringComparison.OrdinalIgnoreCase))
        {
            UnitOfMeasure = unitOfMeasure;
            isUpdated = true;
        }

        if (estimatedUsefulLife.HasValue && estimatedUsefulLife > 0 && EstimatedUsefulLife != estimatedUsefulLife.Value)
        {
            EstimatedUsefulLife = estimatedUsefulLife.Value;
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


