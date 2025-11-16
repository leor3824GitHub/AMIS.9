using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.CheckRegistration.v1;

public sealed record CheckEmployeeRegistrationQuery(Guid UserId) : IRequest<EmployeeRegistrationStatusResponse>;
