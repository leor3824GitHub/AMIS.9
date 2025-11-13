using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Application.Inspections.Search.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class InspectionItemsSearchIntegrationTests : IClassFixture<InspectionItemsWebAppFactory>
{
    private readonly InspectionItemsWebAppFactory _factory;

    public InspectionItemsSearchIntegrationTests(InspectionItemsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchInspections_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInspectionsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspection/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchInspections_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInspectionsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspection/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class InspectionItemsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchInspectionsCommand, PagedList<InspectionResponse>, TestSearchInspectionsHandler>(services);
    }
}

internal class TestSearchInspectionsHandler : IRequestHandler<SearchInspectionsCommand, PagedList<InspectionResponse>>
{
    public Task<PagedList<InspectionResponse>> Handle(SearchInspectionsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<InspectionResponse>
        {
            new(
                Id: Guid.NewGuid(),
                InspectedOn: DateTime.UtcNow,
                EmployeeId: Guid.NewGuid(),
                PurchaseId: Guid.NewGuid(),
                Remarks: "Test",
                Employee: new EmployeeResponse(Guid.NewGuid(), "Inspector A", "Inspector", "INSP", null),
                Purchase: new PurchaseResponse(Guid.NewGuid(), null, DateTime.UtcNow, 0m, null, null, null),
                Approved: false
            )
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
