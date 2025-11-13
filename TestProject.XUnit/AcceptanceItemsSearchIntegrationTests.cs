using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
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
        var request = new SearchAcceptancesCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/acceptances/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchAcceptanceItems_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchAcceptancesCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/acceptances/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class AcceptanceItemsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchAcceptancesCommand, PagedList<AcceptanceResponse>, TestSearchAcceptanceItemsHandler>(services);
    }
}

internal class TestSearchAcceptanceItemsHandler : IRequestHandler<SearchAcceptancesCommand, PagedList<AcceptanceResponse>>
{
    public Task<PagedList<AcceptanceResponse>> Handle(SearchAcceptancesCommand request, CancellationToken cancellationToken)
    {
        var items = new List<AcceptanceResponse>
        {
            new(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                "Test Remarks",
                new EmployeeResponse(Guid.NewGuid(), "John Doe", "Officer", "RC", Guid.NewGuid()),
                new List<AcceptanceItemResponse>
                {
                    new AcceptanceItemResponse(Guid.NewGuid(), Guid.NewGuid(), 10, null)
                },
                false,
                null,
                default
            )
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
