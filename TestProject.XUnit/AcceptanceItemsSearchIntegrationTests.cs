using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Search.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class AcceptanceItemsSearchIntegrationTests : IClassFixture<AcceptanceItemsWebAppFactory>
{
    private readonly AcceptanceItemsWebAppFactory _factory;

    public AcceptanceItemsSearchIntegrationTests(AcceptanceItemsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchAcceptanceItems_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchAcceptanceItemsCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/acceptanceItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceItemResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchAcceptanceItems_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchAcceptanceItemsCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/acceptanceItems/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceItemResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class AcceptanceItemsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchAcceptanceItemsCommand, PagedList<AcceptanceItemResponse>, TestSearchAcceptanceItemsHandler>(services);
    }
}

internal class TestSearchAcceptanceItemsHandler : IRequestHandler<SearchAcceptanceItemsCommand, PagedList<AcceptanceItemResponse>>
{
    public Task<PagedList<AcceptanceItemResponse>> Handle(SearchAcceptanceItemsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<AcceptanceItemResponse>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10, null, null)
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
