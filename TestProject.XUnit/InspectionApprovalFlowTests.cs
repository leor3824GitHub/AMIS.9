using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Events;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Ardalis.Specification;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AMIS.Tests.Catalog
{
    public class InspectionApprovalFlowTests
    {
        [Fact]
        public async Task Approval_AddsInventory_And_CreatesPurchaseTransaction()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var purchaseId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var purchaseItem = PurchaseItem.Create(purchaseId, productId, qty: 10, unitPrice: 100m, itemstatus: PurchaseStatus.Submitted);

            var inspection = Inspection.Create(purchaseId, employeeId, inspectedOn: DateTime.UtcNow, approved: false, remarks: null);
            var item = inspection.AddItem(purchaseItemId: purchaseItem.Id, qtyInspected: 5, qtyPassed: 5, qtyFailed: 0, remarks: null, inspectionItemStatus: InspectionItemStatus.Passed);

            // Inject navigation for unit test via reflection (normally EF Core does this on Include)
            var piProp = typeof(InspectionItem).GetProperty("PurchaseItem", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(piProp);
            piProp!.SetValue(item, purchaseItem);

            var inspectionReadRepo = new Mock<IReadRepository<Inspection>>();
            inspectionReadRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Inspection>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(inspection);

            var inspectionRequestRepo = new Mock<IRepository<InspectionRequest>>();
            // No request returned for simplicity; handler will skip status update
            inspectionRequestRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<InspectionRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InspectionRequest?)null);

            var inventoryRepo = new Mock<IRepository<Inventory>>();
            inventoryRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Inventory>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Inventory?)null);
            inventoryRepo
                .Setup(r => r.AddAsync(It.IsAny<Inventory>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Inventory inv, CancellationToken ct) => inv);

            var inventoryTxnRepo = new Mock<IRepository<InventoryTransaction>>();
            inventoryTxnRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<InventoryTransaction>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InventoryTransaction?)null);
            inventoryTxnRepo
                .Setup(r => r.AddAsync(It.IsAny<InventoryTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InventoryTransaction tx, CancellationToken ct) => tx);

            var logger = new Mock<ILogger<InspectionApprovedHandler>>();

            var handler = new InspectionApprovedHandler(
                inspectionReadRepo.Object,
                inspectionRequestRepo.Object,
                inventoryRepo.Object,
                inventoryTxnRepo.Object,
                logger.Object);

            var @event = new InspectionApproved
            {
                InspectionId = inspection.Id,
                PurchaseId = purchaseId,
                EmployeeId = employeeId,
                ApprovedOn = DateTime.UtcNow
            };

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert: inventory was created with product and qty passed
            inventoryRepo.Verify(r => r.AddAsync(
                It.Is<Inventory>(inv => inv.ProductId == productId && inv.Qty == 5 && inv.AvePrice == 100m),
                It.IsAny<CancellationToken>()),
                Times.Once);

            // Assert: inventory transaction recorded as Purchase with correct details
            inventoryTxnRepo.Verify(r => r.AddAsync(
                It.Is<InventoryTransaction>(t => t.ProductId == productId
                    && t.Qty == 5
                    && t.UnitCost == 100m
                    && t.TransactionType == TransactionType.Purchase
                    && t.SourceId == inspection.Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
