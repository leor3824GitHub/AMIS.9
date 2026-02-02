using Hangfire;
using AMIS.WebApi.Inventories.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.BackgroundJobs;

/// <summary>
/// Registers and schedules recurring inventory reconciliation jobs
/// </summary>
public static class ReconciliationJobScheduler
{
    /// <summary>
    /// Schedule the inventory reconciliation jobs
    /// Runs daily at 02:00 AM (UTC)
    /// </summary>
    public static WebApplication ScheduleInventoryReconciliationJobs(this WebApplication app)
    {
        var backgroundJobClient = app.Services.GetService<IBackgroundJobClient>();
        if (backgroundJobClient == null)
        {
            // Hangfire not configured, skip scheduling
            return app;
        }

        var recurringJobManager = app.Services.GetService<IRecurringJobManager>();
        if (recurringJobManager == null)
        {
            // Hangfire not configured, skip scheduling
            return app;
        }

        try
        {
            // Schedule PPE reconciliation - runs daily at 2 AM
            RecurringJob.AddOrUpdate<InventoryReconciliationJob>(
                "inventory-reconciliation-ppe",
                job => job.ReconcilePPEInventoryAsync(),
                "0 2 * * *");  // Cron: Daily at 02:00 AM

            // Schedule Semex reconciliation - runs daily at 2:15 AM
            RecurringJob.AddOrUpdate<InventoryReconciliationJob>(
                "inventory-reconciliation-semex",
                job => job.ReconcileSemexInventoryAsync(),
                "15 2 * * *");  // Cron: Daily at 02:15 AM

            // Schedule comprehensive reconciliation - runs daily at 2:30 AM
            RecurringJob.AddOrUpdate<InventoryReconciliationJob>(
                "inventory-reconciliation-all",
                job => job.ReconcileAllInventoryAsync(),
                "30 2 * * *");  // Cron: Daily at 02:30 AM
        }
        catch (Exception ex)
        {
            // Log error but don't fail startup
            var logger = app.Services.GetService<ILogger<object>>();
            logger?.LogWarning(ex, "Failed to schedule inventory reconciliation jobs");
        }

        return app;
    }
}
