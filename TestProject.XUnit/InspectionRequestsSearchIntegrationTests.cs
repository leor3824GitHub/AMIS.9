using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class InspectionRequestsSearchIntegrationTests : IClassFixture<InspectionRequestsWebAppFactory>
{
    private readonly InspectionRequestsWebAppFactory _factory;

    public InspectionRequestsSearchIntegrationTests(InspectionRequestsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchInspectionRequests_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();

     var request = new SearchInspectionRequestsCommand
        {
     PageNumber = 1,
          PageSize = 10
      };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspectionRequests/search", request);
        response.EnsureSuccessStatusCode();

  var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionRequestResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchInspectionRequests_InvalidDateRange_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var request = new SearchInspectionRequestsCommand
     {
     FromDate = new DateTime(2024, 2, 1),
        ToDate = new DateTime(2024, 1, 1)
        };

   var response = await client.PostAsJsonAsync("/api/v1/catalog/inspectionRequests/search", request);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SearchInspectionRequests_PaginationEdgeDefaults_ReturnsOk()
    {
  var client = _factory.CreateClient();

        var request = new SearchInspectionRequestsCommand
      {
   PageNumber = 0,
    PageSize = 0
        };

        var response = await client.PostAsJsonAsync("/api/v1/catalog/inspectionRequests/search", request);
      response.EnsureSuccessStatusCode();

    var payload = await response.Content.ReadFromJsonAsync<PagedList<InspectionRequestResponse>>();
    PaginationAssert.AssertDefaults(payload);
    }
}

public class InspectionRequestsWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
 {
    ReplaceHandler<SearchInspectionRequestsCommand, PagedList<InspectionRequestResponse>, TestSearchInspectionRequestsHandler>(services);
    }
}

internal class TestSearchInspectionRequestsHandler : IRequestHandler<SearchInspectionRequestsCommand, PagedList<InspectionRequestResponse>>
{
    public Task<PagedList<InspectionRequestResponse>> Handle(SearchInspectionRequestsCommand request, CancellationToken cancellationToken)
    {
        // Create test response using record constructor
  var item = new InspectionRequestResponse(
            Id: Guid.NewGuid(),
        PurchaseId: null,
InspectorId: null,
         Status: InspectionRequestStatus.Pending,
  DateCreated: DateTime.UtcNow,
   Purchase: null!,
      Inspector: null!
        );

        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(new[] { item }, request.PageNumber, request.PageSize, 1);
 return Task.FromResult(paged);
    }
}
