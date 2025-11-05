-- Phase 2 Database Migration Script
-- IMPORTANT: Review and test in development environment before production deployment
-- Backup database before executing

-- ============================================================================
-- PART 1: ADD NEW COLUMNS TO INVENTORY TABLE
-- ============================================================================

-- Add new columns for inventory management
ALTER TABLE catalog.Inventory 
ADD StockStatus INT NOT NULL DEFAULT 0,  -- StockStatus.Available
    CostingMethod INT NOT NULL DEFAULT 0, -- CostingMethod.WeightedAverage
    ReservedQty INT NOT NULL DEFAULT 0,
    Location NVARCHAR(255) NULL,
    LastCountedDate DATETIME2 NULL;

-- Add comments for documentation (PostgreSQL syntax)
-- COMMENT ON COLUMN catalog.Inventory.StockStatus IS 'Inventory availability status (0=Available, 1=Reserved, 2=Quarantined, etc.)';
-- COMMENT ON COLUMN catalog.Inventory.CostingMethod IS 'Inventory valuation method (0=WeightedAverage, 1=FIFO, 2=LIFO, etc.)';
-- COMMENT ON COLUMN catalog.Inventory.ReservedQty IS 'Quantity reserved for orders/production';
-- COMMENT ON COLUMN catalog.Inventory.Location IS 'Physical warehouse location identifier';
-- COMMENT ON COLUMN catalog.Inventory.LastCountedDate IS 'Last cycle count date';

-- ============================================================================
-- PART 2: UPDATE PRODUCT TABLE - UNIT FIELD MIGRATION
-- ============================================================================

-- Step 1: Add new UnitOfMeasure column
ALTER TABLE catalog.Product 
ADD UnitOfMeasure INT NOT NULL DEFAULT 0; -- UnitOfMeasure.Piece

-- Step 2: Migrate existing string units to enum values
-- Mapping based on UnitOfMeasure enum:
-- 0=Piece, 1=Each, 10=Kilogram, 11=Gram, 20=Liter, 30=Meter, 40=Box, 50=Hour

UPDATE catalog.Product 
SET UnitOfMeasure = CASE 
    -- Quantity Units
    WHEN LOWER(Unit) IN ('pcs', 'piece', 'pieces', 'pc') THEN 0  -- Piece
    WHEN LOWER(Unit) IN ('each', 'ea') THEN 1  -- Each
    WHEN LOWER(Unit) IN ('set', 'sets') THEN 2  -- Set
    WHEN LOWER(Unit) IN ('pair', 'pairs', 'pr') THEN 3  -- Pair
    WHEN LOWER(Unit) IN ('dozen', 'doz') THEN 4  -- Dozen
    
    -- Weight Units
    WHEN LOWER(Unit) IN ('kg', 'kilogram', 'kgs') THEN 10  -- Kilogram
    WHEN LOWER(Unit) IN ('g', 'gram', 'grams', 'gm') THEN 11  -- Gram
    WHEN LOWER(Unit) IN ('ton', 'tons', 'tonne') THEN 12  -- Ton
    WHEN LOWER(Unit) IN ('lb', 'pound', 'pounds', 'lbs') THEN 13  -- Pound
    WHEN LOWER(Unit) IN ('oz', 'ounce', 'ounces') THEN 14  -- Ounce
    
    -- Volume Units
    WHEN LOWER(Unit) IN ('l', 'liter', 'litre', 'liters') THEN 20  -- Liter
    WHEN LOWER(Unit) IN ('ml', 'milliliter', 'millilitre') THEN 21  -- Milliliter
    WHEN LOWER(Unit) IN ('gal', 'gallon', 'gallons') THEN 22  -- Gallon
    
    -- Length Units
    WHEN LOWER(Unit) IN ('m', 'meter', 'metre', 'meters') THEN 30  -- Meter
    WHEN LOWER(Unit) IN ('cm', 'centimeter', 'centimetre') THEN 31  -- Centimeter
    WHEN LOWER(Unit) IN ('ft', 'foot', 'feet') THEN 32  -- Foot
    WHEN LOWER(Unit) IN ('in', 'inch', 'inches') THEN 33  -- Inch
    
    -- Area Units
    WHEN LOWER(Unit) IN ('m2', 'sqm', 'square meter') THEN 34  -- SquareMeter
    WHEN LOWER(Unit) IN ('ft2', 'sqft', 'square foot') THEN 35  -- SquareFoot
    
    -- Packaging Units
    WHEN LOWER(Unit) IN ('box', 'boxes', 'bx') THEN 40  -- Box
    WHEN LOWER(Unit) IN ('case', 'cases', 'cs') THEN 41  -- Case
    WHEN LOWER(Unit) IN ('pallet', 'pallets', 'plt') THEN 42  -- Pallet
    WHEN LOWER(Unit) IN ('drum', 'drums') THEN 43  -- Drum
    WHEN LOWER(Unit) IN ('roll', 'rolls') THEN 44  -- Roll
    WHEN LOWER(Unit) IN ('carton', 'cartons', 'ctn') THEN 45  -- Carton
    WHEN LOWER(Unit) IN ('bag', 'bags') THEN 46  -- Bag
    WHEN LOWER(Unit) IN ('bundle', 'bundles') THEN 47  -- Bundle
    
    -- Time Units
    WHEN LOWER(Unit) IN ('hour', 'hours', 'hr', 'hrs') THEN 50  -- Hour
    WHEN LOWER(Unit) IN ('day', 'days') THEN 51  -- Day
    WHEN LOWER(Unit) IN ('month', 'months', 'mo') THEN 52  -- Month
    
    -- Other Units
    WHEN LOWER(Unit) IN ('percent', '%', 'pct') THEN 60  -- Percent
    WHEN LOWER(Unit) IN ('lot', 'lots') THEN 61  -- Lot
    WHEN LOWER(Unit) IN ('sheet', 'sheets', 'sht') THEN 62  -- Sheet
    
    -- Default to Piece if no match
    ELSE 0  -- Piece (default)
END;

-- Step 3: Verify migration (check for unmapped units)
-- SELECT DISTINCT Unit, UnitOfMeasure 
-- FROM catalog.Product 
-- WHERE UnitOfMeasure = 0 AND LOWER(Unit) NOT IN ('pcs', 'piece', 'pieces', 'pc')
-- ORDER BY Unit;

-- Step 4: Drop old Unit column (only after verification!)
-- UNCOMMENT AFTER VERIFYING MIGRATION IS CORRECT
-- ALTER TABLE catalog.Product DROP COLUMN Unit;

-- Step 5: Rename new column to Unit (optional, maintains API compatibility)
-- UNCOMMENT AFTER DROPPING OLD COLUMN
-- EXEC sp_rename 'catalog.Product.UnitOfMeasure', 'Unit', 'COLUMN';  -- SQL Server
-- ALTER TABLE catalog.Product RENAME COLUMN UnitOfMeasure TO Unit;  -- PostgreSQL

-- ============================================================================
-- PART 3: UPDATE ENUM VALUE DEFAULTS (IF NEEDED)
-- ============================================================================

-- Purchase table - ensure Draft is default (value 0)
-- No action needed if PurchaseStatus.Draft = 0

-- Inspection table - update default to Scheduled (value 0)
-- IF your current default is InProgress, update existing records first:
-- UPDATE catalog.Inspection 
-- SET Status = 1  -- InProgress
-- WHERE Status = 0 AND InspectedOn IS NOT NULL;

-- Then new inspections will use Scheduled = 0 as default

-- ============================================================================
-- PART 4: CREATE INDEXES FOR PERFORMANCE
-- ============================================================================

-- Index for inventory stock status queries
CREATE INDEX IX_Inventory_StockStatus 
ON catalog.Inventory(StockStatus) 
INCLUDE (ProductId, Qty, ReservedQty);

-- Index for inventory location-based queries
CREATE INDEX IX_Inventory_Location 
ON catalog.Inventory(Location) 
WHERE Location IS NOT NULL;

-- Index for inventory with reservations
CREATE INDEX IX_Inventory_ReservedQty 
ON catalog.Inventory(ReservedQty) 
WHERE ReservedQty > 0;

-- Index for products by unit of measure
CREATE INDEX IX_Product_Unit 
ON catalog.Product(UnitOfMeasure);

-- Index for purchase workflow status queries
CREATE INDEX IX_Purchase_Status 
ON catalog.Purchase(Status) 
INCLUDE (PurchaseDate, TotalAmount);

-- Index for inspection status queries
CREATE INDEX IX_Inspection_Status 
ON catalog.Inspection(Status) 
INCLUDE (InspectionRequestId, InspectedOn);

-- ============================================================================
-- PART 5: DATA VALIDATION QUERIES
-- ============================================================================

-- Verify Inventory enhancements
SELECT 
    COUNT(*) as TotalRecords,
    COUNT(CASE WHEN StockStatus = 0 THEN 1 END) as AvailableStock,
    COUNT(CASE WHEN ReservedQty > 0 THEN 1 END) as ItemsWithReservations,
    COUNT(CASE WHEN Location IS NOT NULL THEN 1 END) as ItemsWithLocation
FROM catalog.Inventory;

-- Verify Product unit migration
SELECT 
    UnitOfMeasure,
    COUNT(*) as ProductCount
FROM catalog.Product
GROUP BY UnitOfMeasure
ORDER BY ProductCount DESC;

-- Check for any migration issues (should return 0 rows if all good)
SELECT Id, Name, Unit as OldUnit, UnitOfMeasure as NewUnit
FROM catalog.Product
WHERE UnitOfMeasure = 0  -- Default/Unmapped
  AND LOWER(Unit) NOT IN ('pcs', 'piece', 'pieces', 'pc', '')
LIMIT 100;

-- ============================================================================
-- ROLLBACK SCRIPT (EMERGENCY USE ONLY)
-- ============================================================================

-- To rollback Part 1 (Inventory):
-- ALTER TABLE catalog.Inventory 
-- DROP COLUMN StockStatus, 
-- DROP COLUMN CostingMethod, 
-- DROP COLUMN ReservedQty, 
-- DROP COLUMN Location, 
-- DROP COLUMN LastCountedDate;

-- To rollback Part 2 (Product) - ONLY IF OLD COLUMN STILL EXISTS:
-- ALTER TABLE catalog.Product DROP COLUMN UnitOfMeasure;

-- ============================================================================
-- POST-MIGRATION CHECKLIST
-- ============================================================================

-- [ ] Backup database completed
-- [ ] Migration script reviewed and customized for environment
-- [ ] Script executed successfully in DEV environment
-- [ ] Data validation queries show correct results
-- [ ] Application layer updated to use new enums
-- [ ] API tests pass with new enum values
-- [ ] EF Core migration generated and applied
-- [ ] Indexes created for performance
-- [ ] Old Product.Unit column dropped (after verification)
-- [ ] Production deployment scheduled
-- [ ] Rollback script tested and ready

-- ============================================================================
-- NOTES
-- ============================================================================

-- 1. This script is for SQL Server syntax. Adjust for PostgreSQL if needed:
--    - Change NVARCHAR to VARCHAR or TEXT
--    - Change DATETIME2 to TIMESTAMP
--    - Use ALTER COLUMN syntax variations
--    - Adjust CASE statement syntax if needed

-- 2. Test thoroughly in DEV before PROD deployment

-- 3. Consider breaking into smaller migrations if deploying to production
--    with minimal downtime (e.g., add columns first, migrate data separately)

-- 4. Monitor application logs after deployment for any enum conversion issues

-- 5. Update Blazor client to refresh API client (NSwag regeneration) after
--    schema changes

-- End of Migration Script
