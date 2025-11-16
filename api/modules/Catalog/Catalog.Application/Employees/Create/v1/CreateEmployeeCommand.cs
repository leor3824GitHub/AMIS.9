using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Create.v1;
public sealed record CreateEmployeeCommand(
    [property: DefaultValue("Sample Employee")] string Name,
    [property: DefaultValue("Designation")] string Designation,
    [property: DefaultValue("AGS")] string ResponsibilityCode,
    [property: DefaultValue("IT")] string Department,
    [property: DefaultValue("employee@example.com")] string Email,
    [property: DefaultValue("+1234567890")] string PhoneNumber,
    DateTime? HireDate,
    Guid? SupervisorId,
    Guid? UserId) : IRequest<CreateEmployeeResponse>;

