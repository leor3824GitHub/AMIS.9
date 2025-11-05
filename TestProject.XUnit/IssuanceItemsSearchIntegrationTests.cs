using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class IssuanceItemsSearchIntegrationTests : IClassFixture<IssuanceItemsWebAppFactory>
{
    private readonly IssuanceItemsWebAppFactory _factory;

    public IssuanceItemsSearchIntegrationTests(IssuanceItemsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchIssuanceItems_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchIssuanceItemsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/issuanceItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<IssuanceItemResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchIssuanceItems_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchIssuanceItemsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/issuanceItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<IssuanceItemResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class IssuanceItemsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchIssuanceItemsCommand, PagedList<IssuanceItemResponse>, TestSearchIssuanceItemsHandler>(services);
    }
}

internal class TestSearchIssuanceItemsHandler : IRequestHandler<SearchIssuanceItemsCommand, PagedList<IssuanceItemResponse>>
{
    public Task<PagedList<IssuanceItemResponse>> Handle(SearchIssuanceItemsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<IssuanceItemResponse>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2, 7.25m, "Open", null)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
