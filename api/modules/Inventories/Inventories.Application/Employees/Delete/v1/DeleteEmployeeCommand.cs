using MediatR;

namespace AMIS.WebApi.Inventories.Application.Employees.Delete.v1;
public sealed record DeleteEmployeeCommand(
    Guid Id) : IRequest;

