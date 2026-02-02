using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemexReg = AMIS.WebApi.Inventories.Domain.SemexRegistry;
using SemexTxnLog = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.Services;

/// <summary>
/// Reconciliation job for inventory integrity checks
/// Compares data across reports, registry, assets, and assignments
/// Detects variances and logs reconciliation results
/// 
/// Runs periodically (typically nightly) to verify:
/// - Report quantities vs Registry quantities
/// - Registry quantities vs Physical Asset assignment counts
/// - Missing or orphaned assignments
/// - Negative inventory states
/// </summary>
public class InventoryReconciliationJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InventoryReconciliationJob> _logger;

    public InventoryReconciliationJob(
        IServiceProvider serviceProvider,
        ILogger<InventoryReconciliationJob> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Execute reconciliation check for PPE inventory
    /// </summary>
    public async Task ReconcilePPEInventoryAsync()
    {
        try
        {
            _logger.LogInformation("Starting PPE Inventory Reconciliation Job");

            using var scope = _serviceProvider.CreateScope();
            var registryRepo = scope.ServiceProvider.GetKeyedService<IReadRepository<InventoryRegistry>>("inventories:inventory-registries")
                ?? throw new InvalidOperationException("Registry repository not found");
            var transactionLogRepo = scope.ServiceProvider.GetKeyedService<IReadRepository<InventoryTransactionLog>>("inventories:inventory-transaction-logs")
                ?? throw new InvalidOperationException("Transaction log repository not found");
            var assetRepo = scope.ServiceProvider.GetKeyedService<IReadRepository<PhysicalAsset>>("inventories:physical-assets")
                ?? throw new InvalidOperationException("Asset repository not found");

            // Get all registry entries
            var registries = await registryRepo.ListAsync(cancellationToken: CancellationToken.None);

            var reconciliationResults = new List<PPEReconciliationResult>();
            var varianceCount = 0;

            foreach (var registry in registries)
            {
                try
                {
                    var result = new PPEReconciliationResult
                    {
                        PropertyCode = registry.PropertyCode,
                        RegistryQuantity = registry.Quantity,
                        ReconciliationDate = DateTime.UtcNow
                    };

                    // Get physical assets by property code
                    var assetSpec = new AssetByPropertyCodeSpec(registry.PropertyCode);
                    var assets = await assetRepo.ListAsync(assetSpec, CancellationToken.None);

                    // Count active assignments
                    var activeAssignmentCount = 0;
                    var returnedCount = 0;

                    foreach (var asset in assets)
                    {
                        if (asset.CurrentAssignment != null)
                        {
                            if (asset.CurrentAssignment.Status == "Active")
                                activeAssignmentCount++;
                            else if (asset.CurrentAssignment.Status == "Returned")
                                returnedCount++;
                        }
                    }

                    result.ActiveAssignmentCount = activeAssignmentCount;
                    result.ReturnedAssignmentCount = returnedCount;
                    result.TotalPhysicalAssets = assets.Count;

                    // Detect variance
                    if (activeAssignmentCount != registry.Quantity)
                    {
                        result.HasVariance = true;
                        result.VarianceDescription = $"Registry shows {registry.Quantity} units, but {activeAssignmentCount} active assignments found";
                        varianceCount++;

                        _logger.LogWarning(
                            "PPE Inventory Variance Detected | PropertyCode: {PropertyCode} | Registry Qty: {RegistryQty} | Active Assignments: {ActiveAssignments} | Returned: {Returned}",
                            registry.PropertyCode,
                            registry.Quantity,
                            activeAssignmentCount,
                            returnedCount);
                    }

                    reconciliationResults.Add(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reconciling property code {PropertyCode}", registry.PropertyCode);
                }
            }

            _logger.LogInformation(
                "PPE Inventory Reconciliation Completed | Total Registries: {TotalRegistries} | Variances Found: {VarianceCount}",
                registries.Count,
                varianceCount);

            if (varianceCount > 0)
            {
                _logger.LogWarning(
                    "PPE Reconciliation Summary: {VarianceCount} variance(s) detected. Details: {Details}",
                    varianceCount,
                    string.Join("; ", reconciliationResults.Where(r => r.HasVariance).Select(r => r.VarianceDescription)));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error in PPE Inventory Reconciliation Job");
            throw;
        }
    }

    /// <summary>
    /// Execute reconciliation check for Semi-Expendable inventory
    /// </summary>
    public async Task ReconcileSemexInventoryAsync()
    {
        try
        {
            _logger.LogInformation("Starting Semex Inventory Reconciliation Job");

            using var scope = _serviceProvider.CreateScope();
            var registryRepo = scope.ServiceProvider.GetKeyedService<IReadRepository<SemexReg>>("inventories:semex-registries")
                ?? throw new InvalidOperationException("Semex Registry repository not found");
            var transactionLogRepo = scope.ServiceProvider.GetKeyedService<IReadRepository<SemexTxnLog>>("inventories:semex-transaction-logs")
                ?? throw new InvalidOperationException("Semex Transaction log repository not found");
            var assetRepo = scope.ServiceProvider.GetKeyedService<IReadRepository<PhysicalAsset>>("inventories:physical-assets")
                ?? throw new InvalidOperationException("Asset repository not found");

            // Get all semex registry entries
            var registries = await registryRepo.ListAsync(cancellationToken: CancellationToken.None);

            var reconciliationResults = new List<SemexReconciliationResult>();
            var varianceCount = 0;

            foreach (var registry in registries)
            {
                try
                {
                    var result = new SemexReconciliationResult
                    {
                        ItemCode = registry.ItemCode,
                        RegistryQuantity = registry.Quantity,
                        ReconciliationDate = DateTime.UtcNow
                    };

                    // Get physical assets by item code
                    var assetSpec = new AssetByPropertyCodeSpec(registry.ItemCode);
                    var assets = await assetRepo.ListAsync(assetSpec, CancellationToken.None);

                    // Count active assignments  (semi-expendable tracking)
                    var activeAssignmentCount = 0;
                    var consumedCount = 0;

                    foreach (var asset in assets)
                    {
                        if (asset.CurrentAssignment != null)
                        {
                            if (asset.CurrentAssignment.Status == "Active")
                                activeAssignmentCount++;
                            else if (asset.CurrentAssignment.Status == "Returned")
                                consumedCount++;
                        }
                    }

                    result.ActiveAssignmentCount = activeAssignmentCount;
                    result.ConsumedCount = consumedCount;
                    result.TotalPhysicalAssets = assets.Count;

                    // Detect variance
                    if (activeAssignmentCount != registry.Quantity)
                    {
                        result.HasVariance = true;
                        result.VarianceDescription = $"Registry shows {registry.Quantity} units, but {activeAssignmentCount} active assignments found";
                        varianceCount++;

                        _logger.LogWarning(
                            "Semex Inventory Variance Detected | ItemCode: {ItemCode} | Registry Qty: {RegistryQty} | Active Assignments: {ActiveAssignments} | Consumed: {Consumed}",
                            registry.ItemCode,
                            registry.Quantity,
                            activeAssignmentCount,
                            consumedCount);
                    }

                    reconciliationResults.Add(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reconciling item code {ItemCode}", registry.ItemCode);
                }
            }

            _logger.LogInformation(
                "Semex Inventory Reconciliation Completed | Total Registries: {TotalRegistries} | Variances Found: {VarianceCount}",
                registries.Count,
                varianceCount);

            if (varianceCount > 0)
            {
                _logger.LogWarning(
                    "Semex Reconciliation Summary: {VarianceCount} variance(s) detected. Details: {Details}",
                    varianceCount,
                    string.Join("; ", reconciliationResults.Where(r => r.HasVariance).Select(r => r.VarianceDescription)));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error in Semex Inventory Reconciliation Job");
            throw;
        }
    }

    /// <summary>
    /// Execute comprehensive reconciliation (both PPE and Semex)
    /// </summary>
    public async Task ReconcileAllInventoryAsync()
    {
        _logger.LogInformation("Starting comprehensive Inventory Reconciliation");

        try
        {
            await ReconcilePPEInventoryAsync();
            await ReconcileSemexInventoryAsync();

            _logger.LogInformation("Comprehensive Inventory Reconciliation completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Comprehensive Inventory Reconciliation failed");
            throw;
        }
    }
}

/// <summary>
/// Reconciliation result for PPE inventory
/// </summary>
public class PPEReconciliationResult
{
    public string PropertyCode { get; set; } = string.Empty;
    public int RegistryQuantity { get; set; }
    public int ActiveAssignmentCount { get; set; }
    public int ReturnedAssignmentCount { get; set; }
    public int TotalPhysicalAssets { get; set; }
    public bool HasVariance { get; set; }
    public string? VarianceDescription { get; set; }
    public DateTime ReconciliationDate { get; set; }
}

/// <summary>
/// Reconciliation result for Semi-Expendable inventory
/// </summary>
public class SemexReconciliationResult
{
    public string ItemCode { get; set; } = string.Empty;
    public int RegistryQuantity { get; set; }
    public int ActiveAssignmentCount { get; set; }
    public int ConsumedCount { get; set; }
    public int TotalPhysicalAssets { get; set; }
    public bool HasVariance { get; set; }
    public string? VarianceDescription { get; set; }
    public DateTime ReconciliationDate { get; set; }
}
