using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class PurchasesSearchIntegrationTests : IClassFixture<PurchasesWebAppFactory>
{
    private readonly PurchasesWebAppFactory _factory;

    public PurchasesSearchIntegrationTests(PurchasesWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchPurchases_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchPurchasesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/purchases/search", request);
        response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<PurchaseResponse>>();
    PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchPurchases_PaginationEdgeDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchPurchasesCommand
        {
            PageNumber = 0,
            PageSize = 0
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/purchases/search", request);
        response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<PurchaseResponse>>();
    PaginationAssert.AssertDefaults(payload);
    }
}

public class PurchasesWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchPurchasesCommand, PagedList<PurchaseResponse>, TestSearchPurchasesHandler>(services);
    }
}

internal class TestSearchPurchasesHandler : IRequestHandler<SearchPurchasesCommand, PagedList<PurchaseResponse>>
{
    public Task<PagedList<PurchaseResponse>> Handle(SearchPurchasesCommand request, CancellationToken cancellationToken)
    {
        var item = new PurchaseResponse(
            Guid.NewGuid(),
            null,
     DateTime.UtcNow.Date,
            100m,
      null,
 null,
          null,
            null, // ReferenceNumber
            null, // CreatedOn
  null, // Notes
            null  // Currency
    );

   var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(new[] { item }, request.PageNumber, request.PageSize, 1);
      return Task.FromResult(paged);
    }
}
