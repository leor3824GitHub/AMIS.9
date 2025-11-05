using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Application.Inspections.Search.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class InspectionsSearchIntegrationTests : IClassFixture<InspectionsWebAppFactory>
{
    private readonly InspectionsWebAppFactory _factory;

    public InspectionsSearchIntegrationTests(InspectionsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchInspections_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchInspectionsCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspection/search", request);
        response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionResponse>>();
    PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchInspections_InvalidDateRange_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var request = new SearchInspectionsCommand
        {
            FromDate = new DateTime(2024, 2, 1),
            ToDate = new DateTime(2024, 1, 1)
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspection/search", request);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SearchInspections_PaginationEdgeDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchInspectionsCommand
        {
            PageNumber = 0,
            PageSize = 0
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspection/search", request);
        response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionResponse>>();
    PaginationAssert.AssertDefaults(payload);
    }
}

public class InspectionsWebAppFactory : BaseWebAppFactory
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
        // Return a simple one-item page for happy path
        var item = new InspectionResponse(
            Id: Guid.NewGuid(),
            InspectedOn: DateTime.UtcNow,
            EmployeeId: Guid.NewGuid(),
            PurchaseId: null,
            Remarks: "Test",
            Employee: new EmployeeResponse(Guid.NewGuid(), "John Doe", "Inspector", "INSP", null),
            Purchase: null,
            Approved: false,
            Status: InspectionStatus.Scheduled
        );

        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(new[] { item }, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}

