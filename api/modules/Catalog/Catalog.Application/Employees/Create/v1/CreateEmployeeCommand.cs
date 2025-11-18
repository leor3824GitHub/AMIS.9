using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Create.v1;
public sealed record CreateEmployeeCommand(
    [property: DefaultValue("Sample Employee")] string Name,
    [property: DefaultValue("Designation")] string Designation,
    [property: DefaultValue("AGS")] string? ResponsibilityCode,
    Guid? UserId) : IRequest<CreateEmployeeResponse>;

