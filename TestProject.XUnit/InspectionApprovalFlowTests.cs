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

            var purchaseItem = PurchaseItem.Create(purchaseId, productId, qty: 10, unitPrice: 100m, itemStatus: PurchaseStatus.Submitted);

            var inspection = Inspection.Create(purchaseId, employeeId, inspectedOn: DateTime.UtcNow, remarks: null);
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

            var purchaseRepo = new Mock<IRepository<Purchase>>();
            purchaseRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Purchase?)null);
            purchaseRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Purchase>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<InspectionApprovedHandler>>();

            var handler = new InspectionApprovedHandler(
                inspectionReadRepo.Object,
                inspectionRequestRepo.Object,
                purchaseRepo.Object,
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

        [Fact]
    public async Task E2EInspectApproveInventoryUpdated()
        {
            // Arrange: purchase and inspection with passed qty
            var productId = Guid.NewGuid();
            var purchaseId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var purchaseItem = PurchaseItem.Create(purchaseId, productId, qty: 3, unitPrice: 50m, itemStatus: PurchaseStatus.Submitted);

            var inspection = Inspection.Create(purchaseId, employeeId);
            var item = inspection.AddItem(purchaseItem.Id, qtyInspected: 3, qtyPassed: 3, qtyFailed: 0, remarks: null, inspectionItemStatus: InspectionItemStatus.Passed);

            // Wire navigation for handler consumption
            var piProp = typeof(InspectionItem).GetProperty("PurchaseItem", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(piProp);
            piProp!.SetValue(item, purchaseItem);

            // E2E: invoke domain approval to simulate real flow (emits event)
            inspection.Approve();

            // Repos: start empty so handler must create inventory and transaction
            var inspectionReadRepo = new Mock<IReadRepository<Inspection>>();
            inspectionReadRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Inspection>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(inspection);

            var inspectionRequestRepo = new Mock<IRepository<InspectionRequest>>();
            inspectionRequestRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<InspectionRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InspectionRequest?)null);

            var capturedInventory = default(Inventory);
            var inventoryRepo = new Mock<IRepository<Inventory>>();
            inventoryRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Inventory>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Inventory?)null);
            inventoryRepo
                .Setup(r => r.AddAsync(It.IsAny<Inventory>(), It.IsAny<CancellationToken>()))
                .Callback((Inventory inv, CancellationToken _) => capturedInventory = inv)
                .ReturnsAsync((Inventory inv, CancellationToken _) => inv);

            var capturedTxn = default(InventoryTransaction);
            var inventoryTxnRepo = new Mock<IRepository<InventoryTransaction>>();
            inventoryTxnRepo
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<InventoryTransaction>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InventoryTransaction?)null);
            inventoryTxnRepo
                .Setup(r => r.AddAsync(It.IsAny<InventoryTransaction>(), It.IsAny<CancellationToken>()))
                .Callback((InventoryTransaction tx, CancellationToken _) => capturedTxn = tx)
                .ReturnsAsync((InventoryTransaction tx, CancellationToken _) => tx);

            var purchaseRepo = new Mock<IRepository<Purchase>>();
            purchaseRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Purchase?)null);
            purchaseRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Purchase>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<InspectionApprovedHandler>>();

            var handler = new InspectionApprovedHandler(
                inspectionReadRepo.Object,
                inspectionRequestRepo.Object,
                purchaseRepo.Object,
                inventoryRepo.Object,
                inventoryTxnRepo.Object,
                logger.Object);

            var evt = new InspectionApproved
            {
                InspectionId = inspection.Id,
                PurchaseId = purchaseId,
                EmployeeId = employeeId,
                ApprovedOn = DateTime.UtcNow
            };

            // Act: handle approval event (inventory + transaction)
            await handler.Handle(evt, CancellationToken.None);

            // Assert inventory created and matches passed qty and unit price
            Assert.NotNull(capturedInventory);
            Assert.Equal(productId, capturedInventory!.ProductId);
            Assert.Equal(3, capturedInventory.Qty);
            Assert.Equal(50m, capturedInventory.AvePrice);

            // Assert transaction recorded as Purchase referencing inspection
            Assert.NotNull(capturedTxn);
            Assert.Equal(productId, capturedTxn!.ProductId);
            Assert.Equal(3, capturedTxn.Qty);
            Assert.Equal(50m, capturedTxn.UnitCost);
            Assert.Equal(TransactionType.Purchase, capturedTxn.TransactionType);
            Assert.Equal(inspection.Id, capturedTxn.SourceId);
        }
    }
}
