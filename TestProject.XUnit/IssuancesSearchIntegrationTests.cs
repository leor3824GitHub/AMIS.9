using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using AMIS.WebApi.Catalog.Application.Issuances.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;

namespace TestProject.XUnit;

public class IssuancesSearchIntegrationTests : IClassFixture<IssuancesWebAppFactory>
{
    private readonly IssuancesWebAppFactory _factory;

    public IssuancesSearchIntegrationTests(IssuancesWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchIssuances_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchIssuancesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/issuances/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<IssuanceResponse>>();
        Assert.NotNull(payload);
        Assert.True(payload!.Items?.Count >= 1);
    }

    [Fact]
    public async Task SearchIssuances_PaginationEdgeDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = new SearchIssuancesCommand
        {
            PageNumber = 0,
            PageSize = 0
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/issuances/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<IssuanceResponse>>();
        Assert.NotNull(payload);
        Assert.Equal(1, payload!.PageNumber);
        Assert.Equal(10, payload.PageSize);
    }
}

public class IssuancesWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchIssuancesCommand, PagedList<IssuanceResponse>, TestSearchIssuancesHandler>(services);
    }
}

internal class TestSearchIssuancesHandler : IRequestHandler<SearchIssuancesCommand, PagedList<IssuanceResponse>>
{
    public Task<PagedList<IssuanceResponse>> Handle(SearchIssuancesCommand request, CancellationToken cancellationToken)
    {
        var item = new IssuanceResponse(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            55m,
            false,
            null);

        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(new[] { item }, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
