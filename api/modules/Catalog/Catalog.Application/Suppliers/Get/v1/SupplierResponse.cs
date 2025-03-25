namespace AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
public sealed record SupplierResponse(Guid? id, string name, string? address, string? tin, bool isVAT, string? contactNo, string? emailadd);
