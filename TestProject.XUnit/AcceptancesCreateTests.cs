using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Acceptances.Create.v1;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject.XUnit;

public class AcceptancesCreateTests
{
    [Fact]
    public async Task CreateAcceptanceWithInspectionIdDerivesPurchaseSucceeds()
    {
        // Arrange
        var logger = Mock.Of<ILogger<CreateAcceptanceHandler>>();

    var acceptanceRepo = new Mock<IRepository<Acceptance>>();
    var inspectionRequestRepo = new Mock<IReadRepository<InspectionRequest>>();
    var inspectionRepo = new Mock<IReadRepository<Inspection>>();
    var purchaseReadRepo = new Mock<IReadRepository<Purchase>>();

        var inspectionId = Guid.NewGuid();
        var derivedPurchaseId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        // Create an inspection linked to a purchase
        var inspection = Inspection.Create(derivedPurchaseId, employeeId);

        // Return this inspection when the handler requests the one by id
        inspectionRepo
            .Setup(r => r.GetByIdAsync(inspectionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspection);

        // Provide an inspection request in a completed state for the derived purchase
        var inspRequest = InspectionRequest.Create(derivedPurchaseId);
        inspRequest.MarkCompleted();

        inspectionRequestRepo
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<InspectionRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspRequest);

        Acceptance? captured = null;
        acceptanceRepo
            .Setup(r => r.AddAsync(It.IsAny<Acceptance>(), It.IsAny<CancellationToken>()))
            .Callback<Acceptance, CancellationToken>((a, _) => captured = a)
            .ReturnsAsync((Acceptance a, CancellationToken _) => a);

        var handler = new CreateAcceptanceHandler(
            logger,
            acceptanceRepo.Object,
            inspectionRequestRepo.Object,
            inspectionRepo.Object,
            purchaseReadRepo.Object);

        var cmd = new CreateAcceptanceCommand(
            AcceptanceDate: DateTime.UtcNow,
            SupplyOfficerId: Guid.NewGuid(),
            PurchaseId: Guid.NewGuid(), // Should be ignored when InspectionId is provided
            Remarks: "test",
            InspectionId: inspectionId,
            PostToInventory: false,
            Items: null);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotNull(captured);
        Assert.Equal(inspectionId, captured!.InspectionId);
        Assert.Equal(derivedPurchaseId, captured.PurchaseId);

        acceptanceRepo.Verify(r => r.AddAsync(It.IsAny<Acceptance>(), It.IsAny<CancellationToken>()), Times.Once);
        inspectionRepo.Verify(r => r.GetByIdAsync(inspectionId, It.IsAny<CancellationToken>()), Times.Once);
        inspectionRequestRepo.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<InspectionRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
