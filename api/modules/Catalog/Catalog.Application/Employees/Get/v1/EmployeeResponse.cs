using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Employees.Get.v1;
public sealed record EmployeeResponse(
    Guid? Id,
    string Name,
    string Designation,
    string ResponsibilityCode,
    string Department,
    string Email,
    string PhoneNumber,
    EmploymentStatus Status,
    DateTime? HireDate,
    DateTime? TerminationDate,
    Guid? SupervisorId,
    Guid? UserId);
