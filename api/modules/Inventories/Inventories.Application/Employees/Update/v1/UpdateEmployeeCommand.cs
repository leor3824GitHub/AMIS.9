using MediatR;

namespace AMIS.WebApi.Inventories.Application.Employees.Update.v1;
public sealed record UpdateEmployeeCommand(
    Guid Id,
    string Name,
    string Designation,
    string ResponsibilityCode,
    Guid? UserId) : IRequest<UpdateEmployeeResponse>;

