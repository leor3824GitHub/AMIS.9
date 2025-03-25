namespace AMIS.WebApi.Catalog.Application.Employees.Get.v1;
public sealed record EmployeeResponse(Guid id, string name, string designation, string responsibilitycode, Guid? UserId);
