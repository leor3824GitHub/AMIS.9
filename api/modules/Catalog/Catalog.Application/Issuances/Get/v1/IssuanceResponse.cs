using AMIS.WebApi.Catalog.Application.Employees.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
public sealed record IssuanceResponse(Guid? Id, Guid EmployeeId, DateTime IssuanceDate, decimal TotalAmount, bool IsClosed, EmployeeResponse? Employee);
