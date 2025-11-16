using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.SelfRegister.v1;

public sealed record SelfRegisterEmployeeCommand(
    [property: DefaultValue("John Doe")] string Name,
    [property: DefaultValue("Software Engineer")] string Designation,
    [property: DefaultValue("DEV")] string ResponsibilityCode,
    [property: DefaultValue("IT Department")] string Department,
    [property: DefaultValue("john.doe@example.com")] string Email,
    [property: DefaultValue("+1234567890")] string PhoneNumber,
    DateTime? HireDate,
    Guid? SupervisorId) : IRequest<SelfRegisterEmployeeResponse>;
