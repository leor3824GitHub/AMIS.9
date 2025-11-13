using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using AMIS.WebApi.Catalog.Application.Issuances.Search.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

// Renamed intent: this test now targets Issuances search (there is no separate IssuanceItems search endpoint)
public class IssuanceItemsSearchIntegrationTests : IClassFixture<IssuanceItemsWebAppFactory>
{
    private readonly IssuanceItemsWebAppFactory _factory;

    public IssuanceItemsSearchIntegrationTests(IssuanceItemsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchIssuances_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchIssuancesCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/issuances/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<IssuanceResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchIssuances_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchIssuancesCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/issuances/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<IssuanceResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class IssuanceItemsWebAppFactory : BaseWebAppFactory
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
        var items = new List<IssuanceResponse>
        {
            new(
                Id: Guid.NewGuid(),
                EmployeeId: Guid.NewGuid(),
                IssuanceDate: DateTime.UtcNow,
                TotalAmount: 100m,
                IsClosed: false,
                Employee: new EmployeeResponse(Guid.NewGuid(), "Issuer A", "Clerk", "ISS", null)
            )
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
