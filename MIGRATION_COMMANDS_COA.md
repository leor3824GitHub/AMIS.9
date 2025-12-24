# Database Migration Commands for COA Compliance Features

## Required Migration (Run from api/server directory)

### PostgreSQL Migration
```bash
cd ./api/server
dotnet ef migrations add "Add_SemiExpendableInventory_and_JournalEntryVoucher" \
    --project ../../migrations/PostgreSQL/ \
    --context CatalogDbContext \
    -o Catalog
```

### Apply Migration
```bash
cd ./api/server
dotnet ef database update --context CatalogDbContext
```

## What's Included in This Migration

1. **SemiExpendableInventories Table**
   - PropertyCode (unique identifier)
   - ProductId (FK to Products)
   - Description
   - UnitCost, Quantity, WeightedAverageCost
   - Location
   - EstimatedUsefulLifeMonths
   - CurrentCustodianId (FK to Employees)
   - IsIssued (accountability tracking)

2. **JournalEntryVouchers Table**
   - JEVNumber (unique)
   - TransactionDate
   - Description
   - SourceTransactionId, SourceDocumentType, SourceDocumentNumber
   - TotalDebit, TotalCredit
   - IsPosted, PostedDate, PostedBy
   - Notes

3. **JournalEntryLines Table**
   - JournalEntryVoucherId (FK)
   - AccountCode (RCA 2019)
   - AccountTitle
   - DebitAmount, CreditAmount
   - Particulars

## Notes
- All tables include multi-tenancy support via Finbuckle
- All tables include audit fields (CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
- Proper indexes are created for foreign keys and frequently queried fields
