using AMIS.Blazor.Client.Components.EntityTable;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace AMIS.Blazor.Client.Pages.Catalog;

public partial class Employees
{
    [Inject]
    protected IApiClient _client { get; set; } = default!;

    protected EntityServerTableContext<EmployeeResponse, Guid, EmployeeViewModel> Context { get; set; } = default!;

    private EntityTable<EmployeeResponse, Guid, EmployeeViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Employee",
            entityNamePlural: "Employees",
            entityResource: FshResources.Employees,
            fields: new()
            {
                new(employee => employee.Name, "Name", "Name"),
                new(employee => employee.Designation, "Designation", "Designation"),
                new(employee => employee.ResponsibilityCode, "Responsibilitycode", "Responsibilitycode")
                // Removed UserId field as it's now automatically set from current user
            },
            enableAdvancedSearch: true,
            idFunc: employee => employee.Id!.Value,
            searchFunc: async filter =>
            {
                var employeeFilter = filter.Adapt<SearchEmployeesCommand>();
                var result = await _client.SearchEmployeesEndpointAsync("1", employeeFilter);
                return result.Adapt<PaginationResponse<EmployeeResponse>>();
            },
            createFunc: async employee =>
            {
                await _client.CreateEmployeeEndpointAsync("1", employee.Adapt<CreateEmployeeCommand>());
            },
            updateFunc: async (id, employee) =>
            {
                await _client.UpdateEmployeeEndpointAsync("1", id, employee.Adapt<UpdateEmployeeCommand>());
            },
            deleteFunc: async id => await _client.DeleteEmployeeEndpointAsync("1", id));
}

public class EmployeeViewModel : UpdateEmployeeCommand
{
}
