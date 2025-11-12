using Ardalis.Specification;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

public class GetPurchaseSpecs : Specification<Purchase, PurchaseResponse>
{
    public GetPurchaseSpecs(Guid id)
    {
        Query.Where(p => p.Id == id);
        Query.Include(p => p.Supplier);
        Query.Include(p => p.Items);
        Query.Select(p => new PurchaseResponse(
                p.Id,
                p.SupplierId,
                p.PurchaseDate,
                p.Items.Sum(i => (decimal?)(i.Qty * i.UnitPrice)),
                p.Status,
                p.Supplier == null ? null : new SupplierResponse(
                    p.Supplier.Id,
                    p.Supplier.Name,
                    p.Supplier.Address,
                    p.Supplier.Tin,
                    p.Supplier.TaxClassification,
                    p.Supplier.ContactNo,
                    p.Supplier.Emailadd
                ),
                p.Items.Select(i => new PurchaseItemDto(
                    i.Id,
                    i.ProductId,
                    i.Qty,
                    i.UnitPrice,
                    i.Qty * i.UnitPrice,
                    i.InspectionStatus,
                    i.AcceptanceStatus,
                    i.QtyInspected,
                    i.QtyPassed,
                    i.QtyFailed,
                    i.QtyAccepted
                )).ToList(),
                p.ReferenceNumber,
                p.Created.DateTime,
                p.Notes,
                p.Currency
            ));
    }
}
