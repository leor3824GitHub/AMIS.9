using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;
using AMIS.WebApi.Catalog.Application.InspectionItems.Search.v1;
using MediatR;
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
    public async Task SearchInspectionItems_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInspectionItemsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspectionItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionItemResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchInspectionItems_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInspectionItemsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspectionItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionItemResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class InspectionItemsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchInspectionItemsCommand, PagedList<InspectionItemResponse>, TestSearchInspectionItemsHandler>(services);
    }
}

internal class TestSearchInspectionItemsHandler : IRequestHandler<SearchInspectionItemsCommand, PagedList<InspectionItemResponse>>
{
    public Task<PagedList<InspectionItemResponse>> Handle(SearchInspectionItemsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<InspectionItemResponse>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10, 9, 1, null, null, null)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
