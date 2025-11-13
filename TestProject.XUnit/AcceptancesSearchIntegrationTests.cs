using System.Net.Http.Json;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestProject.XUnit.Testing;
using TestProject.XUnit.Testing.Assertions;

namespace TestProject.XUnit;

public class AcceptancesSearchIntegrationTests : IClassFixture<AcceptancesWebAppFactory>
{
    private readonly AcceptancesWebAppFactory _factory;

    private const string Route = "/api/v1/catalog/acceptances/search";

    public AcceptancesSearchIntegrationTests(AcceptancesWebAppFactory factory)
    {
        _factory = factory;
    }

    private static SearchAcceptancesCommand BuildDefaultCommand(Action<SearchAcceptancesCommand>? mutate = null)
    {
        var cmd = new SearchAcceptancesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };
        mutate?.Invoke(cmd);
        return cmd;
    }

    private static Task<HttpResponseMessage> PostSearchAsync(HttpClient client, SearchAcceptancesCommand cmd, CancellationToken ct = default)
        => client.PostAsJsonAsync(Route, cmd, ct);

    [Fact]
    public async Task SearchAcceptances_HappyPath_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = BuildDefaultCommand();

        var response = await PostSearchAsync(client, request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchAcceptances_InvalidDateRange_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var request = BuildDefaultCommand(c =>
        {
            c.FromDate = new DateTime(2024, 2, 1);
            c.ToDate = new DateTime(2024, 1, 1);
        });

        var response = await PostSearchAsync(client, request);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SearchAcceptances_PaginationEdgeDefaults_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var request = BuildDefaultCommand(c =>
        {
            c.PageNumber = 0;
            c.PageSize = 0;
        });

        var response = await PostSearchAsync(client, request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceResponse>>();
        PaginationAssert.AssertDefaults(payload);
    }

    [Fact]
    public async Task SearchAcceptances_WithOrderByAndFilters_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var officerId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        var request = BuildDefaultCommand(c =>
        {
            c.OrderBy = new[] { "acceptanceDate desc", "id asc" };
            c.SupplyOfficerId = officerId;
            c.FromDate = new DateTime(2023, 1, 1);
            c.ToDate = new DateTime(2024, 12, 31);
        });

        var response = await PostSearchAsync(client, request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedList<AcceptanceResponse>>();
        PaginationAssert.AssertHasItems(payload);
    }

    [Fact]
    public async Task SearchAcceptances_EnumSerializedAsString()
    {
        var client = _factory.CreateClient();
        var response = await PostSearchAsync(client, BuildDefaultCommand());
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"status\":\"Pending\"", json, StringComparison.OrdinalIgnoreCase);
    }
}

public class AcceptancesWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<SearchAcceptancesCommand, PagedList<AcceptanceResponse>, TestSearchAcceptancesHandler>(services);
    }
}

internal sealed class TestSearchAcceptancesHandler : IRequestHandler<SearchAcceptancesCommand, PagedList<AcceptanceResponse>>
{
    private static readonly Guid AcceptanceId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid PurchaseIdConst = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid SupplyOfficerIdConst = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    private static readonly DateTime AcceptanceDateConst = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    public Task<PagedList<AcceptanceResponse>> Handle(SearchAcceptancesCommand request, CancellationToken cancellationToken)
    {
        var item = new AcceptanceResponse(
            Id: AcceptanceId,
            PurchaseId: PurchaseIdConst,
            SupplyOfficerId: SupplyOfficerIdConst,
            AcceptanceDate: AcceptanceDateConst,
            Remarks: string.Empty,
            SupplyOfficer: new EmployeeResponse(
                Id: SupplyOfficerIdConst,
                Name: "Test Officer",
                Designation: "Officer",
                ResponsibilityCode: "SO",
                UserId: null
            ),
            Items: Array.Empty<AcceptanceItemResponse>(),
            IsPosted: false,
            PostedOn: null,
            Status: AcceptanceStatus.Pending
        );

        var paged = TestProject.XUnit.Testing.Paging.TestPagedList.Build(new[] { item }, request.PageNumber, request.PageSize, 1);
        return Task.FromResult(paged);
    }
}
