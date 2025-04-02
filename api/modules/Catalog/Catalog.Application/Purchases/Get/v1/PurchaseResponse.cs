using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
public sealed record PurchaseResponse(Guid? Id, Guid SupplierId, DateTime PurchaseDate, decimal TotalAmount, string Status, SupplierResponse? Supplier);
