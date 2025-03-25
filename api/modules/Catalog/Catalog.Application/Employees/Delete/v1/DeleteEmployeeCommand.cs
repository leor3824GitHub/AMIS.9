using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Delete.v1;
public sealed record DeleteEmployeeCommand(
    Guid Id) : IRequest;
