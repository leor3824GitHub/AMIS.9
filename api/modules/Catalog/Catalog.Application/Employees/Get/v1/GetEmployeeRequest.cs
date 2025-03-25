using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Get.v1;
public class GetEmployeeRequest : IRequest<EmployeeResponse>
{
    public Guid Id { get; set; }
    public GetEmployeeRequest(Guid id) => Id = id;
}
