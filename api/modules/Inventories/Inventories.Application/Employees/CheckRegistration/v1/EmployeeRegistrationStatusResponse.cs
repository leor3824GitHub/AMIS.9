namespace AMIS.WebApi.Inventories.Application.Employees.CheckRegistration.v1;

public sealed record EmployeeRegistrationStatusResponse(
    bool IsRegistered,
    Guid? EmployeeId,
    string? Message);

