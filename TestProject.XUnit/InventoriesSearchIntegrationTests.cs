using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Application.Inventories.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class InventoriesSearchIntegrationTests : IClassFixture<InventoriesWebAppFactory>
{
    private readonly InventoriesWebAppFactory _factory;

    public InventoriesSearchIntegrationTests(InventoriesWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchInventories_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInventoriesCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inventories/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InventoryResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchInventories_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInventoriesCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inventories/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InventoryResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class InventoriesWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchInventoriesCommand, PagedList<InventoryResponse>, TestSearchInventoriesHandler>(services);
    }
}

internal class TestSearchInventoriesHandler : IRequestHandler<SearchInventoriesCommand, PagedList<InventoryResponse>>
{
    public Task<PagedList<InventoryResponse>> Handle(SearchInventoriesCommand request, CancellationToken cancellationToken)
    {
        var items = new List<InventoryResponse>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), 10, 3.75m, null)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
