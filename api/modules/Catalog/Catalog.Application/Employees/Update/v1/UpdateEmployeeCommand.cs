using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Update.v1;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string Name,
    string Designation,
    string ResponsibilityCode) : IRequest<UpdateEmployeeResponse>;
