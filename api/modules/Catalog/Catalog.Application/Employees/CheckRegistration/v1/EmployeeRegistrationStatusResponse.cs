namespace AMIS.WebApi.Catalog.Application.Employees.CheckRegistration.v1;

public sealed record EmployeeRegistrationStatusResponse(
    bool IsRegistered,
    Guid? EmployeeId,
    string? Message);
