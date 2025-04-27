namespace AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
public sealed record SupplierResponse(Guid? Id, string Name, string? Address, string? Tin, string TaxClassification, string? ContactNo, string? Emailadd);
