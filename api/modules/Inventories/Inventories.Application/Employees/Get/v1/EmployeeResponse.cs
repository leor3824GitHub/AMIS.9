namespace AMIS.WebApi.Inventories.Application.Employees.Get.v1;
public sealed record EmployeeResponse(Guid? Id, string Name, string Designation, string ResponsibilityCode, Guid? UserId);

