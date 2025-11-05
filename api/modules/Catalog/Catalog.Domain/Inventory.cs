using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class Inventory : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal AvePrice { get; private set; }
    public StockStatus StockStatus { get; private set; } = StockStatus.Available;
    public CostingMethod CostingMethod { get; private set; } = CostingMethod.WeightedAverage;
    public int ReservedQty { get; private set; }
    public int AvailableQty => Qty - ReservedQty;
    public string? Location { get; private set; }
    public DateTime? LastCountedDate { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private Inventory() { }

    private Inventory(Guid id, Guid? productId, int qty, decimal purchasePrice)
    {
        Id = id;
        ProductId = productId;
        Qty = qty;
        AvePrice = purchasePrice;
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public static Inventory Create(Guid? productId, int qty, decimal purchasePrice)
    {
        ValidateStock(qty, purchasePrice);
        return new Inventory(Guid.NewGuid(), productId, qty, purchasePrice);
    }

    public Inventory Update(Guid? productId, int qty, decimal purchasePrice)
    {
        ValidateStock(qty, purchasePrice);

        bool isUpdated = false;

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (Qty != qty)
        {
            Qty = qty;
            isUpdated = true;
        }

        if (AvePrice != purchasePrice)
        {
            AvePrice = purchasePrice;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryUpdated { Inventory = this });
        }

        return this;
    }
    // add Qty, calculate aveprice and assigned it to AvePrice
    public void AddStock(int qty, decimal purchasePrice)
    {
        ValidateStock(qty, purchasePrice);

        // Update the average price
        AvePrice = ((AvePrice * Qty) + (purchasePrice * qty)) / (Qty + qty);
        Qty += qty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }
    // edit Qty, remove oldqty and add newqty, calculate aveprice
    public void UpdateStock(int oldQty, int newQty, decimal purchasePrice)
    {
        ValidateStock(newQty, purchasePrice);
        if (oldQty < 0) throw new ArgumentException("Old quantity must be zero or greater.");

        int totalQty = Qty - oldQty + newQty;
        AvePrice = totalQty > 0 ? ((AvePrice * Qty) - (AvePrice * oldQty) + (purchasePrice * newQty)) / totalQty : 0;
        Qty = totalQty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }
    // add Qty only
    public void UpdateStock(int qty)
    {
        if (qty < 0) throw new ArgumentException("Quantity must be zero or greater.");

        Qty += qty;
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }
    // deduct Qty only
    public void DeductStock(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (Qty < qty) throw new InvalidOperationException("Not enough stock available.");

        Qty -= qty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }
    // deduct Qty, calculate aveprice and assign it to AvePrice
    public void DeductStock(int qty, decimal unitPrice)
    {
        ValidateStock(qty, unitPrice);

        if (Qty < qty) throw new InvalidOperationException("Not enough stock available.");

        int totalQty = Qty - qty;
        AvePrice = totalQty > 0 ? ((AvePrice * Qty) - (unitPrice * qty)) / totalQty : 0;
        Qty = totalQty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    private static void ValidateStock(int qty, decimal price)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (price <= 0) throw new ArgumentException("Price must be greater than zero.");
    }

    // Stock Status Management
    public void SetStockStatus(StockStatus status)
    {
        if (StockStatus != status)
        {
            StockStatus = status;
            QueueDomainEvent(new InventoryUpdated { Inventory = this });
        }
    }

    public void MarkAsQuarantined(string? location = null)
    {
        SetStockStatus(StockStatus.Quarantined);
        if (!string.IsNullOrWhiteSpace(location))
        {
            Location = location;
        }
    }

    public void ReleaseFromQuarantine()
    {
        if (StockStatus != StockStatus.Quarantined)
            throw new InvalidOperationException("Inventory is not quarantined.");
        
        SetStockStatus(StockStatus.Available);
    }

    public void MarkAsDamaged()
    {
        SetStockStatus(StockStatus.Damaged);
    }

    public void MarkAsObsolete()
    {
        SetStockStatus(StockStatus.Obsolete);
    }

    // Reservation Management
    public void ReserveStock(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (AvailableQty < qty) throw new InvalidOperationException("Not enough available stock to reserve.");

        ReservedQty += qty;
        if (ReservedQty > 0 && StockStatus == StockStatus.Available)
        {
            SetStockStatus(StockStatus.Reserved);
        }
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public void ReleaseReservation(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (ReservedQty < qty) throw new InvalidOperationException("Cannot release more than reserved quantity.");

        ReservedQty -= qty;
        if (ReservedQty == 0 && StockStatus == StockStatus.Reserved)
        {
            SetStockStatus(StockStatus.Available);
        }
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public void AllocateToProduction(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (AvailableQty < qty) throw new InvalidOperationException("Not enough available stock.");

        ReservedQty += qty;
        SetStockStatus(StockStatus.AllocatedToProduction);
    }

    // Costing Method Management
    public void SetCostingMethod(CostingMethod method)
    {
        if (CostingMethod != method)
        {
            CostingMethod = method;
            QueueDomainEvent(new InventoryUpdated { Inventory = this });
        }
    }

    // Location Management
    public void SetLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be empty.");

        Location = location;
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    // Cycle Count
    public void RecordCycleCount(int countedQty, DateTime countDate)
    {
        if (countedQty < 0) throw new ArgumentException("Counted quantity cannot be negative.");

        int variance = countedQty - Qty;
        if (variance != 0)
        {
            Qty = countedQty;
            if (variance < 0)
            {
                SetStockStatus(StockStatus.UnderCount);
            }
        }

        LastCountedDate = countDate;
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }
}
