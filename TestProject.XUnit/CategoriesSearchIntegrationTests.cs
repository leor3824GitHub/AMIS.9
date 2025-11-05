using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Categories.Get.v1;
using AMIS.WebApi.Catalog.Application.Categories.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class CategoriesSearchIntegrationTests : IClassFixture<CategoriesWebAppFactory>
{
    private readonly CategoriesWebAppFactory _factory;

    public CategoriesSearchIntegrationTests(CategoriesWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchCategories_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchCategorysCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/categories/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<CategoryResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchCategories_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchCategorysCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/categories/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<CategoryResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class CategoriesWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchCategorysCommand, PagedList<CategoryResponse>, TestSearchCategoriesHandler>(services);
    }
}

internal class TestSearchCategoriesHandler : IRequestHandler<SearchCategorysCommand, PagedList<CategoryResponse>>
{
    public Task<PagedList<CategoryResponse>> Handle(SearchCategorysCommand request, CancellationToken cancellationToken)
    {
        var items = new List<CategoryResponse>
        {
            new(Guid.NewGuid(), "Hardware", "Nuts and bolts")
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
