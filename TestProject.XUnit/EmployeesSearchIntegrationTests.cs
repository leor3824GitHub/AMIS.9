using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class EmployeesSearchIntegrationTests : IClassFixture<EmployeesWebAppFactory>
{
    private readonly EmployeesWebAppFactory _factory;

    public EmployeesSearchIntegrationTests(EmployeesWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchEmployees_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchEmployeesCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/employees/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<EmployeeResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchEmployees_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchEmployeesCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/employees/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<EmployeeResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class EmployeesWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchEmployeesCommand, PagedList<EmployeeResponse>, TestSearchEmployeesHandler>(services);
    }
}

internal class TestSearchEmployeesHandler : IRequestHandler<SearchEmployeesCommand, PagedList<EmployeeResponse>>
{
    public Task<PagedList<EmployeeResponse>> Handle(SearchEmployeesCommand request, CancellationToken cancellationToken)
    {
        var items = new List<EmployeeResponse>
        {
            new(Guid.NewGuid(), "Jane Doe", "Manager", "RC-01", null)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
