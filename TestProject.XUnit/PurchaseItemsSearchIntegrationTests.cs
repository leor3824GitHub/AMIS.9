using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class PurchaseItemsSearchIntegrationTests : IClassFixture<PurchaseItemsWebAppFactory>
{
    private readonly PurchaseItemsWebAppFactory _factory;

    public PurchaseItemsSearchIntegrationTests(PurchaseItemsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchPurchaseItems_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchPurchaseItemsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/purchaseItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<PurchaseItemResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchPurchaseItems_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchPurchaseItemsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/purchaseItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<PurchaseItemResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class PurchaseItemsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchPurchaseItemsCommand, PagedList<PurchaseItemResponse>, TestSearchPurchaseItemsHandler>(services);
    }
}

internal class TestSearchPurchaseItemsHandler : IRequestHandler<SearchPurchaseItemsCommand, PagedList<PurchaseItemResponse>>
{
    public Task<PagedList<PurchaseItemResponse>> Handle(SearchPurchaseItemsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<PurchaseItemResponse>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5, 12.5m, null, null)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
