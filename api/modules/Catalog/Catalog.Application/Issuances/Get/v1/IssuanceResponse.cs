using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
public sealed record IssuanceResponse(Guid? Id, Guid ProductId, DateTime IssuanceDate, decimal TotalAmount, string? Status, EmployeeResponse? Employee);
