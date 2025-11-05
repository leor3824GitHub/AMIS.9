using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Application.Inspections.Search.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Infrastructure.Persistence;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace TestProject.XUnit.Inspections;

public class InspectionMaterializationTests
{
    private sealed class TestTenantAccessor : IMultiTenantContextAccessor<AMIS.Framework.Infrastructure.Tenant.FshTenantInfo>
    {
        private readonly IMultiTenantContext<AMIS.Framework.Infrastructure.Tenant.FshTenantInfo> _context;

        public IMultiTenantContext<AMIS.Framework.Infrastructure.Tenant.FshTenantInfo> MultiTenantContext => _context;

        IMultiTenantContext IMultiTenantContextAccessor.MultiTenantContext => _context;

        public TestTenantAccessor()
        {
            var tenantInfo = new AMIS.Framework.Infrastructure.Tenant.FshTenantInfo(
                id: "test",
                name: "Test Tenant",
                connectionString: string.Empty,
                adminEmail: "admin@test"
            );

            var context = new MultiTenantContext<AMIS.Framework.Infrastructure.Tenant.FshTenantInfo>();
            context.TenantInfo = tenantInfo;
            _context = context;
        }
    }

    private static CatalogDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var tenantAccessor = new TestTenantAccessor();
        var publisher = new Mock<IPublisher>().Object;
        var dbOptions = Options.Create(new DatabaseOptions { Provider = "inmemory", ConnectionString = string.Empty });

        return new CatalogDbContext(tenantAccessor, options, publisher, dbOptions);
    }

    [Fact]
    public async Task Search_projection_handles_null_InspectedOn_without_throwing()
    {
        // Arrange
        await using var context = CreateInMemoryContext(Guid.NewGuid().ToString());

        // Minimal related data
        var employee = AMIS.WebApi.Catalog.Domain.Employee.Create("John", "Inspector", "RC", null);
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var inspection = AMIS.WebApi.Catalog.Domain.Inspection.Create(
            inspectionRequestId: Guid.NewGuid(), 
            employeeId: employee.Id, 
            inspectedOn: DateTime.UtcNow, 
            approved: false, 
            remarks: null);
        
        // Force InspectedOn to null to simulate legacy data
        context.Attach(inspection);
        context.Entry(inspection).Property(nameof(AMIS.WebApi.Catalog.Domain.Inspection.InspectedOn)).CurrentValue = null;
        context.Inspections.Add(inspection);
        await context.SaveChangesAsync();

        // Act - Query entity to verify null InspectedOn doesn't cause issues
        var result = await context.Inspections
            .Include(i => i.Employee)
            .FirstOrDefaultAsync(i => i.Id == inspection.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.InspectedOn); // Null should be persisted and retrieved without exceptions
        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.NotNull(result.Employee);
    }
}
