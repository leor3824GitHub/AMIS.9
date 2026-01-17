using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Employees.SelfRegister.v1;

public sealed record SelfRegisterEmployeeCommand(
    [property: DefaultValue("John Doe")] string Name,
    [property: DefaultValue("Software Engineer")] string Designation,
    [property: DefaultValue("DEV")] string ResponsibilityCode) : IRequest<SelfRegisterEmployeeResponse>;

