using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Sku { get; private set; }
    public string Unit { get; private set; } = "pcs";
    public string? ImagePath { get; private set; }
    public Guid? CategoryId { get; private set; }
    public PropertyClassification PropertyClassification { get; private set; } = PropertyClassification.Consumable;
    public int EstimatedUsefulLife { get; private set; } = 12; // in months, default 1 year
    public virtual Category? Category { get; private set; }


    private Product() { }

    private Product(Guid id, string name, string? description, decimal sku, string unit, string? imagePath, Guid? categoryId, PropertyClassification classification, int estimatedUsefulLife)
    {
        Id = id;
        Name = name;
        Description = description;
        Sku = sku;
        Unit = unit;
        ImagePath = imagePath;
        CategoryId = categoryId;
        PropertyClassification = classification;
        EstimatedUsefulLife = estimatedUsefulLife;


        QueueDomainEvent(new ProductCreated { Product = this });
    }

    public static Product Create(string name, string? description, decimal sku, string unit, string? imagePath, Guid? categoryId, PropertyClassification? classification = null, int? estimatedUsefulLife = null)
    {
        // Auto-determine classification based on SKU/cost if not provided
        var autoClassification = classification ?? DetermineClassification(sku);
        var usefulLife = estimatedUsefulLife ?? (autoClassification == PropertyClassification.Consumable ? 12 : 36);

        return new Product(Guid.NewGuid(), name, description, sku, unit, imagePath, categoryId, autoClassification, usefulLife);
    }

    public Product Update(string? name, string? description, decimal? sku, string? unit, string? imagePath, Guid? categoryId, PropertyClassification? classification = null, int? estimatedUsefulLife = null)
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
            // Auto-update classification if SKU changes significantly
            if (!classification.HasValue)
            {
                var newClassification = DetermineClassification(sku.Value);
                if (newClassification != PropertyClassification)
                {
                    PropertyClassification = newClassification;
                    isUpdated = true;
                }
            }
            isUpdated = true;
        }

        if (classification.HasValue && PropertyClassification != classification.Value)
        {
            PropertyClassification = classification.Value;
            isUpdated = true;
        }

        if (estimatedUsefulLife.HasValue && EstimatedUsefulLife != estimatedUsefulLife.Value)
        {
            EstimatedUsefulLife = estimatedUsefulLife.Value;
            isUpdated = true;
        }

        if (categoryId.HasValue && categoryId.Value != Guid.Empty && CategoryId != categoryId.Value)
        {
            CategoryId = categoryId.Value;
            isUpdated = true;
        }

        if (!string.Equals(Unit, unit, StringComparison.OrdinalIgnoreCase))
        {
            Unit = unit;
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

    /// <summary>
    /// Auto-determine property classification based on cost (COA/DBM 2023-2025 Standards)
    /// </summary>
    private static PropertyClassification DetermineClassification(decimal cost)
    {
        if (cost > 50000)
        {
            return PropertyClassification.PropertyPlantEquipment;
        }
        else if (cost > 1000)
        {
            // Between ?1,000 and ?50,000 - default to Semi-Expendable
            // Can be overridden if item is consumable in nature
            return PropertyClassification.SemiExpendable;
        }
        else
        {
            return PropertyClassification.Consumable;
        }
    }
}


