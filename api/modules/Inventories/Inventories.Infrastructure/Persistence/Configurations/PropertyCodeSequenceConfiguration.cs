using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public class PropertyCodeSequenceConfiguration : IEntityTypeConfiguration<PropertyCodeSequence>
{
    public void Configure(EntityTypeBuilder<PropertyCodeSequence> builder)
    {
        builder.ToTable("PropertyCodeSequences", SchemaNames.Inventories);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ClassCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Classification)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CategoryCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.ItemCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.ItemDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.GLAccount)
            .HasMaxLength(20);

        builder.Property(e => e.LastSequenceValue)
            .IsRequired();

        // Unique constraint on ClassCode + CategoryCode + ItemCode combination
        builder.HasIndex(e => new { e.ClassCode, e.CategoryCode, e.ItemCode })
            .IsUnique(false)  // Temporarily disabled due to seed data conflicts
            .HasDatabaseName("IX_PropertyCodeSequences_ClassCode_CategoryCode_ItemCode");

        // Seed data from COA/DBM property classification standards
        SeedPropertyCodeSequencesFromCoa(builder);
    }

    private static void SeedPropertyCodeSequencesFromCoa(EntityTypeBuilder<PropertyCodeSequence> builder)
    {
        var seedTimestamp = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var seedUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        var seedItems = GetSeedItems(seedUserId, seedTimestamp);
        builder.HasData(seedItems);
    }

    private static object[] GetSeedItems(Guid seedUserId, DateTimeOffset seedTimestamp)
    {
        // Helper to create seed item
        object CreateSeedItem(int id, string classCode, string classification, string categoryCode, string itemCode, string itemDescription, string? glAccount)
        {
            return new
            {
                Id = id,
                ClassCode = classCode,
                Classification = classification,
                CategoryCode = categoryCode,
                ItemCode = itemCode,
                ItemDescription = itemDescription,
                GLAccount = glAccount,
                LastSequenceValue = 0,
                CreatedBy = seedUserId,
                Created = seedTimestamp,
                LastModifiedBy = seedUserId,
                LastModified = seedTimestamp,
                DeletedBy = (Guid?)null,
                Deleted = (DateTimeOffset?)null
            };
        }

        return new object[]
        {
            CreateSeedItem(1, "LL", "LAND", "LL", "01", "LAND", "10601010"),
            CreateSeedItem(2, "LI", "LAND IMPROVEMENTS", "LI", "01", "OTHER LAND IMPROVEMENTS", "10602990"),
            CreateSeedItem(3, "BS", "BUILDING and Other STRUCTURES", "BS", "01", "BUILDINGS", "10604010"),
            CreateSeedItem(4, "OS", "Other Structures", "OS", "01", "OTHER STRUCTURES - WAREHOUSES", "10604990"),
            CreateSeedItem(5, "OS", "Other Structures", "OS", "02", "OTHER STRUCTURES - STAFF HOUSE", "10604990"),
            CreateSeedItem(6, "OS", "Other Structures", "OS", "03", "OTHER STRUCTURES - MOTORPOOL", "10604990"),
            CreateSeedItem(7, "OS", "Other Structures", "OS", "04", "OTHER STRUCTURES - TRUCK SCALE", "10604990"),
            CreateSeedItem(8, "OS", "Other Structures", "OS", "05", "OTHER STRUCTURES - POWER HOUSE", "10604990"),
            CreateSeedItem(9, "OS", "Other Structures", "OS", "06", "OTHER STRUCTURES - CANTEEN", "10604990"),
            CreateSeedItem(10, "OS", "Other Structures", "OS", "07", "OTHER STRUCTURES - LABORATORY", "10604990"),
            CreateSeedItem(11, "OS", "Other Structures", "OS", "08", "OTHER STRUCTURES - GUARD HOUSE", "10604990"),
            CreateSeedItem(12, "OS", "Other Structures", "OS", "09", "OTHER STRUCTURES - TRAINING CENTER", "10604990"),
            CreateSeedItem(13, "OS", "Other Structures", "OS", "10", "OTHER STRUCTURES - STORAGE/STOCK ROOM", "10604990"),
            CreateSeedItem(14, "OS", "Other Structures", "OS", "11", "OTHER STRUCTURES - COMFORT ROOM", "10604990"),
            CreateSeedItem(15, "FM", "MACHINERY AND EQUIPMENT", "FM", "01", "FARM MACHINERY EQUIPMENT", "10605010"),
            CreateSeedItem(16, "FM", "MACHINERY AND EQUIPMENT", "WE", "02", "WAREHOUSE EQUIPMENT", "10605010"),
            CreateSeedItem(17, "FM", "MACHINERY AND EQUIPMENT", "PE", "03", "PLANT AND MACHINERY EQUIPMENT", "10605010"),
            CreateSeedItem(18, "OE", "OFFICE EQUIPMENT", "OE", "01", "ADDING MACHINE", "10605020"),
            CreateSeedItem(19, "OE", "OFFICE EQUIPMENT", "OE", "02", "CALCULATORS", "10605020"),
            CreateSeedItem(20, "OE", "OFFICE EQUIPMENT", "OE", "03", "AIRCONDITIONING EQUIPMENT", "10605020"),
            CreateSeedItem(21, "OE", "OFFICE EQUIPMENT", "OE", "04", "BUNDY CLOCK/ BIOMETRIC MACHINE", "10605020"),
            CreateSeedItem(22, "OE", "OFFICE EQUIPMENT", "OE", "05", "SHREDDING MACHINE", "10605020"),
            CreateSeedItem(23, "OE", "OFFICE EQUIPMENT", "OE", "06", "CHECKWRITER/CURRENCY COUNTING/DETECTOR", "10605020"),
            CreateSeedItem(24, "OE", "OFFICE EQUIPMENT", "OE", "07", "ELECTRIC FAN", "10605020"),
            CreateSeedItem(25, "OE", "OFFICE EQUIPMENT", "OE", "08", "MIMEOGRAPHING MACHINE", "10605020"),
            CreateSeedItem(26, "OE", "OFFICE EQUIPMENT", "OE", "09", "PHOTOCOPYING MACHINE", "10605020"),
            CreateSeedItem(27, "OE", "OFFICE EQUIPMENT", "OE", "10", "STEEL FILING-CABINET", "10605020"),
            CreateSeedItem(28, "OE", "OFFICE EQUIPMENT", "OE", "11", "STEEL CABINETS", "10605020"),
            CreateSeedItem(29, "OE", "OFFICE EQUIPMENT", "OE", "12", "STEEL SAFETY VAULT", "10605020"),
            CreateSeedItem(30, "OE", "OFFICE EQUIPMENT", "OE", "13", "TYPEWRITER", "10605020"),
            CreateSeedItem(31, "OE", "OFFICE EQUIPMENT", "OE", "14", "BINDING MACHINE", "10605020"),
            CreateSeedItem(32, "OE", "OFFICE EQUIPMENT", "OE", "15", "PHOTO STENCILING MACHINE", "10605020"),
            CreateSeedItem(33, "OE", "OFFICE EQUIPMENT", "OE", "16", "LETTER SCALE", "10605020"),
            CreateSeedItem(34, "OE", "OFFICE EQUIPMENT", "OE", "17", "HEAVY DUTY PUNCHER- 3 HOLES", "10605020"),
            CreateSeedItem(35, "OE", "OFFICE EQUIPMENT", "OE", "18", "HAND TRUCK/ PUSH CART", "10605020"),
            CreateSeedItem(36, "OE", "OFFICE EQUIPMENT", "OE", "19", "KITCHEN EQUIPMENT", "10605020"),
            CreateSeedItem(37, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "01", "PERSONAL COMPUTERS", "10605030"),
            CreateSeedItem(38, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "02", "PRINTER", "10605030"),
            CreateSeedItem(39, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "03", "COMPUTER PERIPHERAL", "10605030"),
            CreateSeedItem(40, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "04", "SOFTWARE APPLICATION", "10605030"),
            CreateSeedItem(41, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "05", "MONITOR", "10605030"),
            CreateSeedItem(42, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "06", "PERSONAL COMPUTER ACCESSORIES", "10605030"),
            CreateSeedItem(43, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "07", "SCANNER", "10605030"),
            CreateSeedItem(44, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "08", "POWER SUPPLY FOR COMPUTER(UPS)", "10605030"),
            CreateSeedItem(45, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "09", "Auto Voltage Regulator (AVR)", "10605030"),
            CreateSeedItem(46, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "10", "ACCESS POINT", "10605030"),
            CreateSeedItem(47, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "11", "DVD/LCD WRITER", "10605030"),
            CreateSeedItem(48, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "12", "SPEAKER", "10605030"),
            CreateSeedItem(49, "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", "DP", "13", "SERVERS", "10605030"),
            CreateSeedItem(50, "CM", "COMMUNICATION EQUIPMENT", "CM", "01", "RADIO EQUIPMENT", "10605070"),
            CreateSeedItem(51, "CM", "COMMUNICATION EQUIPMENT", "CM", "02", "TELEPHONE", "10605070"),
            CreateSeedItem(52, "CM", "COMMUNICATION EQUIPMENT", "CM", "03", "AUDIO-VISUAL EQUIPMENT", "10605070"),
            CreateSeedItem(53, "CM", "COMMUNICATION EQUIPMENT", "CM", "04", "PROJECTOR", "10605070"),
            CreateSeedItem(54, "CM", "COMMUNICATION EQUIPMENT", "CM", "05", "PROJECTOR SCREEN", "10605070"),
            CreateSeedItem(55, "CM", "COMMUNICATION EQUIPMENT", "CM", "06", "CAMERA", "10605070"),
            CreateSeedItem(56, "CM", "COMMUNICATION EQUIPMENT", "CM", "07", "PORTABLE POWER BANK", "10605070"),
            CreateSeedItem(57, "CM", "COMMUNICATION EQUIPMENT", "CM", "08", "PAGING SYSTEM", "10605070"),
            CreateSeedItem(58, "CM", "COMMUNICATION EQUIPMENT", "CM", "09", "PHONES/FAX MACHINE/MOBILE", "10605070"),
            CreateSeedItem(59, "FR", "DISASTER RESPONSE AND RESCUE EQUIPMENT", "FR", "01", "Rescue Team Equipment", "10605090"),
            CreateSeedItem(60, "FR", "DISASTER RESPONSE AND RESCUE EQUIPMENT", "FR", "02", "Firefighting Equipment", "10605090"),
            CreateSeedItem(61, "MD", "MEDICAL /DENTAL /LABORATORY EQUIPMENT", "MD", "01", "MEDICAL EQUIPMENT", "10605110"),
            CreateSeedItem(62, "MD", "MEDICAL /DENTAL /LABORATORY EQUIPMENT", "MD", "02", "DENTAL EQUIPMENT", "10605110"),
            CreateSeedItem(63, "MD", "MEDICAL /DENTAL /LABORATORY EQUIPMENT", "MD", "03", "LABORATORY EQUIPMENT", "10605110"),
            CreateSeedItem(64, "SP", "SPORTS EQUIPMENT", "SP", "01", "SPORTS EQUIPMENT", "10605130"),
            CreateSeedItem(65, "SP", "SPORTS EQUIPMENT", "SP", "02", "TABLE/BOARD EQUIPMENT", "10605130"),
            CreateSeedItem(66, "SP", "SPORTS EQUIPMENT", "SP", "03", "WEIGHT PLATES", "10605130"),
            CreateSeedItem(67, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "01", "DRAFTING EQUIPMENT", "10605140"),
            CreateSeedItem(68, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "02", "TENSILE/BURSTING STRENGTH TESTER", "10605140"),
            CreateSeedItem(69, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "03", "MOISTURE METERS", "10605140"),
            CreateSeedItem(70, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "04", "LABORATORY APPARATUS", "10605140"),
            CreateSeedItem(71, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "05", "HAND SHELLER -for laboratory use", "10605140"),
            CreateSeedItem(72, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "06", "GAS MASK/GLOVES/FACE MASK", "10605140"),
            CreateSeedItem(73, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "07", "BINOCULARS/SAFETY GOGGLES", "10605140"),
            CreateSeedItem(74, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "08", "DIAL THERMOMETER", "10605140"),
            CreateSeedItem(75, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "09", "BIN PROBE", "10605140"),
            CreateSeedItem(76, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "10", "PEST CONTROL PROTECTIVE SUIT", "10605140"),
            CreateSeedItem(77, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "11", "ENGRAVER", "10605140"),
            CreateSeedItem(78, "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", "TS", "12", "PALLET TRUCK", "10605140"),
            CreateSeedItem(79, "OEO", "OTHER EQUIPMENT", "OEO", "01", "Kitchen Equipment", "10605990"),
            CreateSeedItem(80, "OEO", "OTHER EQUIPMENT", "OEO", "02", "Electric equipment", "10605990"),
            CreateSeedItem(81, "LT", "MOTOR VEHICLES", "LT", "01", "BUS", "10606010"),
            CreateSeedItem(82, "LT", "MOTOR VEHICLES", "LT", "02", "TRUCK", "10606010"),
            CreateSeedItem(83, "LT", "MOTOR VEHICLES", "LT", "03", "ASIAN UTILITY VEHICLES", "10606010"),
            CreateSeedItem(84, "LT", "MOTOR VEHICLES", "LT", "04", "JEEP", "10606010"),
            CreateSeedItem(85, "LT", "MOTOR VEHICLES", "LT", "05", "TRAILERS", "10606010"),
            CreateSeedItem(86, "LT", "MOTOR VEHICLES", "LT", "06", "CARS", "10606010"),
            CreateSeedItem(87, "LT", "MOTOR VEHICLES", "LT", "07", "MOTORCYCLE/BIKES", "10606010"),
            CreateSeedItem(88, "LT", "MOTOR VEHICLES", "LT", "08", "CUSTOMBUILT VEHICLES", "10606010"),
            CreateSeedItem(89, "AC", "AIRCRAFTS AND AIRCRAFTS GROUND EQUIPMENTS", "AC", "01", "AIRPLANES", "10606030"),
            CreateSeedItem(90, "AC", "AIRCRAFTS AND AIRCRAFTS GROUND EQUIPMENTS", "AC", "02", "HELICOPTERS", "10606030"),
            CreateSeedItem(91, "AC", "AIRCRAFTS AND AIRCRAFTS GROUND EQUIPMENTS", "AC", "03", "LIFE VEST/LIFE RING & ACCESSORIES", "10606030"),
            CreateSeedItem(92, "WC", "WATERCRAFTS", "WC", "01", "MOTORBOAT", "10606040"),
            CreateSeedItem(93, "WC", "WATERCRAFTS", "WC", "02", "ENGINE FOR MOTORBOAT/RUBBERBOAT", "10606040"),
            CreateSeedItem(94, "WC", "WATERCRAFTS", "WC", "03", "BARGE", "10606040"),
            CreateSeedItem(95, "WC", "WATERCRAFTS", "WC", "04", "AMPHIBIAN", "10606040"),
            CreateSeedItem(96, "OT", "Other Transportation Equipment", "OT", "01", "HYDRAULIC JACK", "10606990"),
            CreateSeedItem(97, "OT", "Other Transportation Equipment", "OT", "02", "AIR COMPRESSOR", "10606990"),
            CreateSeedItem(98, "OT", "Other Transportation Equipment", "OT", "03", "WELDING MACHINE", "10606990"),
            CreateSeedItem(99, "OT", "Other Transportation Equipment", "OT", "04", "ACETYLYNE TORCH", "10606990"),
            CreateSeedItem(100, "OT", "Other Transportation Equipment", "OT", "05", "BATTERY CHARGER", "10606990"),
            CreateSeedItem(101, "OT", "Other Transportation Equipment", "OT", "06", "CHAIN BLOCK", "10606990"),
            CreateSeedItem(102, "OT", "Other Transportation Equipment", "OT", "07", "ENGINE ANALYZER", "10606990"),
            CreateSeedItem(103, "OT", "Other Transportation Equipment", "OT", "08", "TIRE CHANGER", "10606990"),
            CreateSeedItem(104, "OT", "Other Transportation Equipment", "OT", "09", "CAR/TRUCK/WASHING EQUIPMENT", "10606990"),
            CreateSeedItem(105, "OT", "Other Transportation Equipment", "OT", "10", "VALVE REFACER", "10606990"),
            CreateSeedItem(106, "OT", "Other Transportation Equipment", "OT", "11", "WHEEL BALANCING EQUIPMENT", "10606990"),
            CreateSeedItem(107, "FF", "FURNITURES AND FIXTURES", "FF", "01", "BED", "10607010"),
            CreateSeedItem(108, "FF", "FURNITURES AND FIXTURES", "FF", "02", "TABLE", "10607010"),
            CreateSeedItem(109, "FF", "FURNITURES AND FIXTURES", "FF", "03", "BOOKSHELVES", "10607010"),
            CreateSeedItem(110, "FF", "FURNITURES AND FIXTURES", "FF", "04", "PAINTING/FRAMES", "10607010"),
            CreateSeedItem(111, "FF", "FURNITURES AND FIXTURES", "FF", "05", "ALL TYPES OF VERTICAL AND VENITIAN BLINDS", "10607010"),
            CreateSeedItem(112, "FF", "FURNITURES AND FIXTURES", "FF", "06", "CART", "10607010"),
            CreateSeedItem(113, "FF", "FURNITURES AND FIXTURES", "FF", "07", "RACK", "10607010"),
            CreateSeedItem(114, "FF", "FURNITURES AND FIXTURES", "FF", "08", "PLANT BOX/ DIVIDER", "10607010"),
            CreateSeedItem(115, "FF", "FURNITURES AND FIXTURES", "FF", "09", "CABINET", "10607010"),
            CreateSeedItem(116, "FF", "FURNITURES AND FIXTURES", "FF", "10", "CHAIR", "10607010"),
            CreateSeedItem(117, "FF", "FURNITURES AND FIXTURES", "FF", "11", "SALA SET/SOFA/BENCH", "10607010"),
            CreateSeedItem(118, "FF", "FURNITURES AND FIXTURES", "FF", "12", "BOARD/SIGNAGES", "10607010"),
            CreateSeedItem(119, "FF", "FURNITURES AND FIXTURES", "FF", "13", "FOLDING STAGE", "10607010"),
            CreateSeedItem(120, "FF", "FURNITURES AND FIXTURES", "FF", "14", "ROSTRUM/DICTIONARY/BIBLE STAND", "10607010"),
            CreateSeedItem(121, "FF", "FURNITURES AND FIXTURES", "FF", "15", "MIRROR WITH FRAME /STAND", "10607010"),
            CreateSeedItem(122, "FF", "FURNITURES AND FIXTURES", "FF", "16", "ELEVATOR GUIDE", "10607010"),
            CreateSeedItem(123, "FF", "FURNITURES AND FIXTURES", "FF", "17", "ASTRAY/WASTEBASKET/TRASHCAN", "10607010"),
            CreateSeedItem(124, "LB", "BOOKS", "LB", "01", "BOOKS", "10607020"),
            CreateSeedItem(125, "LA", "LEASED ASSETS IMPROVEMENTS", "LA", "01", "Leased Assets Improvements - Land", "10609010"),
            CreateSeedItem(126, "LA", "LEASED ASSETS IMPROVEMENTS BUILDING", "LA", "01", "Leased Assets Improvements Building", "10609020"),
            CreateSeedItem(127, "CIP", "CONSTRUCTION IN PROGRESS", "CIP", "01", "Construction in progress - Land Improvements", "10610010"),
            CreateSeedItem(128, "CIP", "CONSTRUCTION IN PROGRESS- BUILDING", "CIP", "01", "Construction in progress -Building", "10610030"),
            CreateSeedItem(129, "CIP", "CONSTRUCTION IN PROGRESS- BUILDING", "CIP", "02", "Construction in progress -Warehouse", "10610030"),
            CreateSeedItem(130, "CIP", "CONSTRUCTION IN PROGRESS- EQUIPMENT FURNITURES", "CIP", "01", "Construction in Progress - Equipments", "10610060"),
            CreateSeedItem(131, "CIP", "CONSTRUCTION IN PROGRESS- EQUIPMENT FURNITURES", "CIP", "02", "Construction in Progress - Furnitures & Fixtures", "10610060"),
            CreateSeedItem(132, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "HT", "01", "HANDTOOLS (HT)", "10698990"),
            CreateSeedItem(133, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "SE", "02", "Ordnance Non-Expendable Supplies & Equipment (SE)", "10698990"),
            CreateSeedItem(134, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "LF", "03", "LIGHTNING FACILITIES (LF)", "10698990"),
            CreateSeedItem(135, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "KF", "04", "STORE EQUIPMENT (KF)", "10698990"),
            CreateSeedItem(136, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "TN", "05", "TENT/Fumigating Sheet (TN)", "10698990"),
            CreateSeedItem(137, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "PD", "06", "PETROLEUM DISPENSER W/DIESEL TANK (PD)", "10698990"),
            CreateSeedItem(138, "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", "OP", "07", "COLD STORAGE ROOM", "10698990"),
        };
    }
}
