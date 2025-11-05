using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Brands.Get.v1;
using AMIS.WebApi.Catalog.Application.Brands.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class BrandsSearchIntegrationTests : IClassFixture<BrandsWebAppFactory>
{
    private readonly BrandsWebAppFactory _factory;

    public BrandsSearchIntegrationTests(BrandsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchBrands_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchBrandsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/brands/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<BrandResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchBrands_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchBrandsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/brands/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<BrandResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class BrandsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchBrandsCommand, PagedList<BrandResponse>, TestSearchBrandsHandler>(services);
    }
}

internal class TestSearchBrandsHandler : IRequestHandler<SearchBrandsCommand, PagedList<BrandResponse>>
{
    public Task<PagedList<BrandResponse>> Handle(SearchBrandsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<BrandResponse>
        {
            new(Guid.NewGuid(), "Northwind", "Legacy brand")
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
