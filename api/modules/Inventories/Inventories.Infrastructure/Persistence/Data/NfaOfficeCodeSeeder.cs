using System.Data;
using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Data;

public static class NfaOfficeCodeSeeder
{
    public static async Task SeedDefaultsAsync(InventoriesDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        if (!await NfaOfficeCodeTableExistsAsync(context, cancellationToken))
        {
            logger.LogWarning("Skipping NFA office code seed because table {Table} does not exist.", "inventories.NfaOfficeCodes");
            return;
        }

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

    private static async Task<bool> NfaOfficeCodeTableExistsAsync(InventoriesDbContext context, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM information_schema.tables
                WHERE table_schema = 'inventories'
                  AND table_name = 'NfaOfficeCodes'
            );
            """;

        var connection = context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is true;
    }
}
