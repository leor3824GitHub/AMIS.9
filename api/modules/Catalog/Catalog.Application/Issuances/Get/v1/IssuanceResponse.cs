using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
public sealed record IssuanceResponse(Guid? Id, DateTime IssuanceDate, decimal TotalAmount, bool IsClosed, Guid EmployeeId, EmployeeResponse? Employee);
