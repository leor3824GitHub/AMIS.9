using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Application.Suppliers.Search.v1;
using MediatR;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class SuppliersSearchIntegrationTests : IClassFixture<SuppliersWebAppFactory>
{
    private readonly SuppliersWebAppFactory _factory;

    public SuppliersSearchIntegrationTests(SuppliersWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchSuppliers_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchSuppliersCommand { PageNumber = 1, PageSize = 10 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/suppliers/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<SupplierResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchSuppliers_PaginationDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new SearchSuppliersCommand { PageNumber = 0, PageSize = 0 };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/suppliers/search", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<SupplierResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }
}

public class SuppliersWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchSuppliersCommand, PagedList<SupplierResponse>, TestSearchSuppliersHandler>(services);
    }
}

internal class TestSearchSuppliersHandler : IRequestHandler<SearchSuppliersCommand, PagedList<SupplierResponse>>
{
    public Task<PagedList<SupplierResponse>> Handle(SearchSuppliersCommand request, CancellationToken cancellationToken)
    {
        var items = new List<SupplierResponse>
        {
            new(Guid.NewGuid(), "Acme Corp", "Street 1", "123-456", "VAT", "555-0100", "info@acme.test")
        };
        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(items, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
