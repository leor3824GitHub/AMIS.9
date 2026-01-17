using AMIS.WebApi.Inventories.Application.Employees.Get.v1;

namespace AMIS.WebApi.Inventories.Application.Issuances.Get.v1;
public sealed record IssuanceResponse(Guid? Id, Guid EmployeeId, DateTime IssuanceDate, decimal TotalAmount, bool IsClosed, EmployeeResponse? Employee);

