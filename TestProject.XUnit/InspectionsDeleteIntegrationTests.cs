using System.Net.Http.Json;
using AMIS.WebApi.Catalog.Application.Inspections.Delete.v1;
using MediatR;
using TestProject.XUnit.Testing;

namespace TestProject.XUnit;

public class InspectionsDeleteIntegrationTests : IClassFixture<InspectionsDeleteWebAppFactory>
{
    private readonly InspectionsDeleteWebAppFactory _factory;

    public InspectionsDeleteIntegrationTests(InspectionsDeleteWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteInspection_HappyPath_ReturnsOkAndEchoesId()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();

        // Act
        var response = await client.DeleteAsync($"/api/v1/catalog/inspection/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<DeleteInspectionResponse>();
        Assert.NotNull(payload);
        Assert.Equal(id, payload!.Id);
    }
}

public class InspectionsDeleteWebAppFactory : BaseWebAppFactory
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplaceHandler<DeleteInspectionCommand, DeleteInspectionResponse, TestDeleteInspectionHandler>(services);
    }
}

internal class TestDeleteInspectionHandler : IRequestHandler<DeleteInspectionCommand, DeleteInspectionResponse>
{
    public Task<DeleteInspectionResponse> Handle(DeleteInspectionCommand request, CancellationToken cancellationToken)
    {
        // Simply echo back the id to validate routing and payload shape
        return Task.FromResult(new DeleteInspectionResponse(request.Id));
    }
}
