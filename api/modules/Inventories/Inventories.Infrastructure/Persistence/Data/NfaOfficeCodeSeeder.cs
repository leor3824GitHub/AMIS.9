using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Data;

public static class NfaOfficeCodeSeeder
{
    public static async Task SeedDefaultsAsync(InventoriesDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        if (await context.NfaOfficeCodes.AnyAsync(cancellationToken))
        {
            logger.LogInformation("NFA office codes already exist. Skipping seed.");
            return;
        }

        var codes = new List<NfaOfficeCode>
        {
            NfaOfficeCode.Create("6000", "Central Office", 1, parentOfficeCode: null),
            NfaOfficeCode.Create("6001", "Plant Industry Department", 2, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6002", "Food Assistance Department", 3, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6003", "Operational Control Management", 4, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6004", "Quality Assurance Department", 5, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6005", "Procurement Management Department", 6, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6006", "Corporate Planning Department", 7, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6007", "Corporate Services Department", 8, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6008", "Accounting Department", 9, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6009", "Legal Department", 10, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6010", "Commodities Inventory Department", 11, parentOfficeCode: "6000"),
            NfaOfficeCode.Create("6800", "Locations", 100, parentOfficeCode: null)
        };

        context.NfaOfficeCodes.AddRange(codes);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {Count} NFA office codes.", codes.Count);
    }
}
