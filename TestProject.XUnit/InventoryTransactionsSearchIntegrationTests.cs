using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class InventoryTransactionsSearchIntegrationTests : IClassFixture<InventoryTransactionsWebAppFactory>
{
    private readonly InventoryTransactionsWebAppFactory _factory;

    public InventoryTransactionsSearchIntegrationTests(InventoryTransactionsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchInventoryTransactions_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInventoryTransactionsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inventoryTranscation/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InventoryTransactionResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchInventoryTransactions_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchInventoryTransactionsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inventoryTranscation/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<InventoryTransactionResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class InventoryTransactionsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchInventoryTransactionsCommand, PagedList<InventoryTransactionResponse>, TestSearchInventoryTransactionsHandler>(services);
    }
}

internal class TestSearchInventoryTransactionsHandler : IRequestHandler<SearchInventoryTransactionsCommand, PagedList<InventoryTransactionResponse>>
{
    public Task<PagedList<InventoryTransactionResponse>> Handle(SearchInventoryTransactionsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<InventoryTransactionResponse>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), 3, 1.5m, Guid.NewGuid(), "WH-1", AMIS.WebApi.Catalog.Domain.ValueObjects.TransactionType.Purchase)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
