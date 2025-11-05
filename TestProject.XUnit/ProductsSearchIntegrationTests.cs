using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Application.Products.Search.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class ProductsSearchIntegrationTests : IClassFixture<ProductsWebAppFactory>
{
    private readonly ProductsWebAppFactory _factory;

    public ProductsSearchIntegrationTests(ProductsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchProducts_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchProductsCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/products/search", request);
        response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<ProductResponse>>();
    PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchProducts_PaginationEdgeDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchProductsCommand
        {
            PageNumber = 0,
            PageSize = 0
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/products/search", request);
        response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<ProductResponse>>();
    PaginationAssert.AssertDefaults(payload);
    }
}

public class ProductsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchProductsCommand, PagedList<ProductResponse>, TestSearchProductsHandler>(services);
    }
}

internal class TestSearchProductsHandler : IRequestHandler<SearchProductsCommand, PagedList<ProductResponse>>
{
    public Task<PagedList<ProductResponse>> Handle(SearchProductsCommand request, CancellationToken cancellationToken)
    {
        var item = new ProductResponse(
            Guid.NewGuid(),
            "Test Product",
            "Test Description",
            1m,
            UnitOfMeasure.Piece,
            null,
            null,
            null);

        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(new[] { item }, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
