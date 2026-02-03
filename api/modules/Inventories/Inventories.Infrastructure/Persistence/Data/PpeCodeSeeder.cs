using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Data;

public static class PpeCodeSeeder
{
    public static async Task SeedDefaultsAsync(InventoriesDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        if (!await PpeCategoryCodeTableExistsAsync(context, cancellationToken))
        {
            logger.LogWarning("Skipping PPE category code seed because table {Table} does not exist.", "inventories.PPECategoryCodes");
            return;
        }

        if (await context.PpeCategoryCodes.AnyAsync(cancellationToken))
        {
            logger.LogInformation("PPE category codes already exist. Skipping seed.");
            return;
        }

        var categories = new List<PpeCategoryCode>
        {
            PpeCategoryCode.Create("LL", "10601010", "Land", 1, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("LI", "10602990", "Land Improvements", 2, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("BS", "10604010", "Buildings and Other Structures", 3, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("OS", "10604990", "Other Structures", 4, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("FM", "10605010", "Machinery and Equipment", 5, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("OE", "10605020", "Office Equipment", 6, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("DP", "10605030", "ICT Equipment", 7, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("CM", "10605070", "Communication Equipment", 8, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("FR", "10605090", "Disaster Response and Rescue Equipment", 9, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("MD", "10605110", "Medical/Dental/Laboratory Equipment", 10, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("SP", "10605130", "Sports Equipment", 11, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("TS", "10605140", "Technical and Scientific Equipment", 12, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("LT", "10606010", "Motor Vehicles", 14, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("AC", "10606030", "Aircrafts and Aircrafts Ground Equipments", 15, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("WC", "10606040", "Watercrafts", 16, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("OT", "10606990", "Other Transportation Equipment", 17, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("FF", "10607010", "Furniture and Fixtures", 18, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("LB", "10607020", "Books", 19, coaReference: "COA Annex D"),
            PpeCategoryCode.Create("OP", "10698990", "Other Property, Plant and Equipment", 20, coaReference: "COA Annex D")
        };

        context.PpeCategoryCodes.AddRange(categories);
        await context.SaveChangesAsync(cancellationToken);

        var typeCodes = new List<PpeTypeCode>
        {
            PpeTypeCode.Create(categories[0].Id, "01", "Land", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[1].Id, "01", "Land Improvements", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[2].Id, "01", "Buildings", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[3].Id, "01", "Warehouses", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[4].Id, "01", "Farm Machinery Equipment", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[5].Id, "01", "Adding Machine", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[6].Id, "01", "Personal Computers", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[7].Id, "01", "Radio Equipment", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[8].Id, "01", "Rescue Team Equipment", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[9].Id, "01", "Medical Equipment", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[10].Id, "01", "Sports Equipment", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[11].Id, "01", "Drafting Equipment", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[12].Id, "01", "Bus", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[13].Id, "01", "Airplanes", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[14].Id, "01", "Motorboat", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[15].Id, "01", "Hydraulic Jack", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[16].Id, "01", "Bed", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[17].Id, "01", "Books", 1, coaReference: "COA Annex D"),
            PpeTypeCode.Create(categories[18].Id, "01", "Handtools", 1, coaReference: "COA Annex D")
        };

        context.PpeTypeCodes.AddRange(typeCodes);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {CategoryCount} PPE categories and {TypeCount} PPE types.", categories.Count, typeCodes.Count);
    }

    private static async Task<bool> PpeCategoryCodeTableExistsAsync(InventoriesDbContext context, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM information_schema.tables
                WHERE table_schema = 'inventories'
                  AND table_name = 'PPECategoryCodes'
            );
            """;

        var connection = context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is true;
    }
}
