using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.Search.v1;
public class SearchPurchaseSpecs : EntitiesByPaginationFilterSpec<Purchase, PurchaseResponse>
{
    public SearchPurchaseSpecs(SearchPurchasesCommand command)
        : base(command)
    {
        Query
            .Include(p => p.Supplier)
            .Include(o => o.Items)
            .OrderBy(c => c.PurchaseDate, !command.HasOrderBy());

        if (command.SupplierId.HasValue)
        {
            Query.Where(p => p.SupplierId == command.SupplierId.Value);
        }

        // Explicit projection to align with new PurchaseItemDto and computed TotalAmount
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
