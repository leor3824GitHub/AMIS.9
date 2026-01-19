CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'inventories') THEN
            CREATE SCHEMA inventories;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AnnualProcurementPlans" (
        "Id" uuid NOT NULL,
        "ControlNumber" character varying(50) NOT NULL,
        "FiscalYear" integer NOT NULL,
        "Status" integer NOT NULL,
        "BudgetType" integer NOT NULL,
        "TotalBudget" numeric(18,2) NOT NULL,
        "PreparedByUserId" uuid NOT NULL,
        "SubmissionDate" timestamp with time zone,
        "ApprovedByUserId" uuid,
        "ApprovalDate" timestamp with time zone,
        "RejectionReason" character varying(1000),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AnnualProcurementPlans" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AssetClassificationRules" (
        "Id" uuid NOT NULL,
        "RuleName" text NOT NULL,
        "Classification" integer NOT NULL,
        "MinimumCost" numeric NOT NULL,
        "MaximumCost" numeric NOT NULL,
        "MinimumUsefulLifeMonths" integer,
        "EffectiveDate" timestamp with time zone NOT NULL,
        "ExpiryDate" timestamp with time zone,
        "RCAAccountCode" text NOT NULL,
        "ExpenseAccountCode" text NOT NULL,
        "DocumentType" text NOT NULL,
        "COAReference" text NOT NULL,
        "IsActive" boolean NOT NULL,
        "Priority" integer NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AssetClassificationRules" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AssetConditionConfigurations" (
        "Id" uuid NOT NULL,
        "Code" character varying(50) NOT NULL,
        "DisplayName" character varying(100) NOT NULL,
        "Description" character varying(500),
        "ColorCode" character varying(20) NOT NULL,
        "SortOrder" integer NOT NULL,
        "IsActive" boolean NOT NULL,
        "AllowsForUse" boolean NOT NULL,
        "RequiresRepair" boolean NOT NULL,
        "RequiresDisposal" boolean NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AssetConditionConfigurations" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Brands" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(1000),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Brands" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Categories" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(1000),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Employees" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Designation" character varying(100) NOT NULL,
        "ResponsibilityCode" text NOT NULL,
        "UserId" uuid,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Employees" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."JournalEntryVouchers" (
        "Id" uuid NOT NULL,
        "VoucherNumber" character varying(25) NOT NULL,
        "VoucherDate" timestamp with time zone NOT NULL,
        "Month" integer NOT NULL,
        "Year" integer NOT NULL,
        "DepreciationMethod" character varying(50) NOT NULL,
        "TotalDebitAmount" numeric(18,2) NOT NULL,
        "TotalCreditAmount" numeric(18,2) NOT NULL,
        "Status" integer NOT NULL,
        "PostedDate" timestamp with time zone,
        "ExportedDate" timestamp with time zone,
        "ExportFormat" character varying(20),
        "ExportFileName" character varying(256),
        "Remarks" character varying(500),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_JournalEntryVouchers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PpeIssuanceReport" (
        "Id" uuid NOT NULL,
        "ReportNumber" character varying(50) NOT NULL,
        "RecipientName" character varying(255) NOT NULL,
        "RecipientAddress" character varying(500) NOT NULL,
        "IssuanceType" text NOT NULL,
        "IssuanceDate" timestamp with time zone NOT NULL,
        "DistributedToVoucher" boolean NOT NULL,
        "DistributedToPMSDS" boolean NOT NULL,
        "DistributedToAccounting" boolean NOT NULL,
        "DistributedToFile" boolean NOT NULL,
        "Notes" character varying(1000),
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        "LineItems" jsonb,
        CONSTRAINT "PK_PpeIssuanceReport" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PpeReceivingReport" (
        "Id" uuid NOT NULL,
        "ReportNumber" character varying(50) NOT NULL,
        "Location" character varying(200) NOT NULL,
        "SourceName" character varying(200) NOT NULL,
        "SourceAddress" character varying(500) NOT NULL,
        "SourceReceiptDate" timestamp with time zone NOT NULL,
        "ReceiptType" text NOT NULL,
        "DistributedToVoucher" boolean NOT NULL,
        "DistributedToPMSDS" boolean NOT NULL,
        "DistributedToAccounting" boolean NOT NULL,
        "DistributedToFile" boolean NOT NULL,
        "Notes" character varying(1000),
        "Created" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        "LineItems" jsonb,
        CONSTRAINT "PK_PpeReceivingReport" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PPETypeAccountMappings" (
        "Id" uuid NOT NULL,
        "PPEType" character varying(100) NOT NULL,
        "RCAAccountCode" character varying(50) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_PPETypeAccountMappings" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PPETypeDefinitions" (
        "Id" uuid NOT NULL,
        "Code" character varying(50) NOT NULL,
        "Name" character varying(200) NOT NULL,
        "Description" character varying(1000),
        "RCAAccountCode" character varying(50) NOT NULL,
        "DepreciationAccountCode" character varying(50) NOT NULL,
        "DefaultDepreciationRate" numeric(5,2) NOT NULL,
        "DefaultUsefulLifeYears" integer NOT NULL,
        "SortOrder" integer NOT NULL,
        "IsActive" boolean NOT NULL,
        "COAReference" character varying(100),
        "Category" character varying(100),
        "IconName" character varying(50),
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_PPETypeDefinitions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ProcurementPlans" (
        "Id" uuid NOT NULL,
        "ControlNumber" character varying(50) NOT NULL,
        "FiscalYear" integer NOT NULL,
        "DepartmentId" uuid NOT NULL,
        "DepartmentName" character varying(200) NOT NULL,
        "Status" integer NOT NULL,
        "IsSupplemental" boolean NOT NULL,
        "BudgetType" integer NOT NULL,
        "TotalBudget" numeric(18,2) NOT NULL,
        "PreparedByUserId" uuid NOT NULL,
        "SubmissionDate" timestamp with time zone,
        "ApprovedByUserId" uuid,
        "ApprovalDate" timestamp with time zone,
        "RejectionReason" character varying(1000),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_ProcurementPlans" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ProcurementProjects" (
        "Id" uuid NOT NULL,
        "PapCode" character varying(50),
        "ProjectTitle" character varying(500) NOT NULL,
        "PmoEndUser" character varying(200) NOT NULL,
        "IsEpa" boolean NOT NULL,
        "Mode" integer NOT NULL,
        "FundSource" character varying(200) NOT NULL,
        "Remarks" character varying(1000),
        "SourcePlanItemId" uuid,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_ProcurementProjects" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."RcaAccountCodes" (
        "Id" uuid NOT NULL,
        "Key" character varying(64) NOT NULL,
        "AccountCode" character varying(50) NOT NULL,
        "Description" character varying(256),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_RcaAccountCodes" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Suppliers" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Address" character varying(1000),
        "Tin" text,
        "TaxClassification" character varying(10) NOT NULL,
        "ContactNo" text,
        "Emailadd" text,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Suppliers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."SuppliesAndMaterialsIssuanceReports" (
        "Id" uuid NOT NULL,
        "SmirNumber" character varying(50) NOT NULL,
        "TransactionDate" timestamp with time zone NOT NULL,
        "Recipient_Name" character varying(200) NOT NULL,
        "Recipient_Address" character varying(500) NOT NULL,
        "Recipient_ContactNumber" character varying(20),
        "IssuanceReason" text NOT NULL,
        "Authorization_IssuingOfficerName" character varying(200) NOT NULL,
        "Authorization_IssuingOfficerSignature" character varying(500) NOT NULL,
        "Authorization_IssuingDate" timestamp with time zone NOT NULL,
        "Authorization_ApprovingOfficerName" character varying(200) NOT NULL,
        "Authorization_ApprovingOfficerSignature" character varying(500) NOT NULL,
        "Authorization_ApprovingDate" timestamp with time zone NOT NULL,
        "Authorization_RecipientName" character varying(200) NOT NULL,
        "Authorization_RecipientSignature" character varying(500) NOT NULL,
        "Authorization_ReceiptDate" timestamp with time zone NOT NULL,
        "Authorization_DriverName" character varying(200),
        "Authorization_DriverSignature" character varying(500),
        "Authorization_BillOfLadingNumber" character varying(100),
        "DistributedToRecipient" boolean NOT NULL DEFAULT FALSE,
        "DistributedToPMSDS" boolean NOT NULL DEFAULT FALSE,
        "DistributedToAccounting" boolean NOT NULL DEFAULT FALSE,
        "DistributedToAccountingAdvice" boolean NOT NULL DEFAULT FALSE,
        "DistributedToFile" boolean NOT NULL DEFAULT FALSE,
        "Notes" character varying(1000),
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_SuppliesAndMaterialsIssuanceReports" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."SuppliesAndMaterialsReceivingReports" (
        "Id" uuid NOT NULL,
        "SmrrNumber" character varying(50) NOT NULL,
        "Location" character varying(100) NOT NULL,
        "Source_Name" character varying(200) NOT NULL,
        "Source_Address" character varying(500) NOT NULL,
        "Source_ReceivingDate" timestamp with time zone NOT NULL,
        "TransactionType" text NOT NULL,
        "Authentication_ReceivedByName" character varying(200) NOT NULL,
        "Authentication_ReceivedBySignature" character varying(500) NOT NULL,
        "Authentication_ReceivedDate" timestamp with time zone NOT NULL,
        "Authentication_NotedByName" character varying(200) NOT NULL,
        "Authentication_NotedBySignature" character varying(500) NOT NULL,
        "Authentication_NotedDate" timestamp with time zone NOT NULL,
        "DistributedToVoucher" boolean NOT NULL DEFAULT FALSE,
        "DistributedToPMSDS" boolean NOT NULL DEFAULT FALSE,
        "DistributedToAccounting" boolean NOT NULL DEFAULT FALSE,
        "DistributedToFile" boolean NOT NULL DEFAULT FALSE,
        "Notes" character varying(1000),
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_SuppliesAndMaterialsReceivingReports" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."UnitsOfMeasure" (
        "Id" uuid NOT NULL,
        "Code" character varying(20) NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Abbreviation" character varying(20),
        "UnitType" integer NOT NULL,
        "IsActive" boolean NOT NULL,
        "SortOrder" integer NOT NULL,
        "IsDefault" boolean NOT NULL,
        "BaseUnitId" uuid,
        "ConversionFactor" numeric(18,6),
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_UnitsOfMeasure" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_UnitsOfMeasure_UnitsOfMeasure_BaseUnitId" FOREIGN KEY ("BaseUnitId") REFERENCES inventories."UnitsOfMeasure" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AnnualProcurementPlanItems" (
        "Id" uuid NOT NULL,
        "PlanHeaderId" uuid NOT NULL,
        "DepartmentId" uuid NOT NULL,
        "DepartmentName" character varying(200) NOT NULL,
        "PapCode" character varying(50),
        "Description" character varying(500) NOT NULL,
        "ProjectType" integer NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitOfMeasure" character varying(50) NOT NULL,
        "UnitCost" numeric(18,2) NOT NULL,
        "EstimatedBudget" numeric(18,2) NOT NULL,
        "Mode" character varying(100) NOT NULL,
        "IsEarlyProcurement" boolean NOT NULL,
        "ScheduleMonth" character varying(20) NOT NULL,
        "FundingSource" character varying(200) NOT NULL,
        "Remarks" character varying(1000),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AnnualProcurementPlanItems" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AnnualProcurementPlanItems_AnnualProcurementPlans_PlanHeade~" FOREIGN KEY ("PlanHeaderId") REFERENCES inventories."AnnualProcurementPlans" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Products" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(1000),
        "Sku" numeric NOT NULL,
        "Unit" text NOT NULL,
        "ImagePath" text,
        "CategoryId" uuid,
        "PropertyClassification" integer NOT NULL DEFAULT 1,
        "EstimatedUsefulLife" integer NOT NULL DEFAULT 12,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES inventories."Categories" ("Id")
    );
    COMMENT ON COLUMN inventories."Products"."PropertyClassification" IS '1=Consumable, 2=SemiExpendable, 3=PPE';
    COMMENT ON COLUMN inventories."Products"."EstimatedUsefulLife" IS 'Estimated useful life in months';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Issuances" (
        "Id" uuid NOT NULL,
        "EmployeeId" uuid NOT NULL,
        "IssuanceDate" timestamp with time zone NOT NULL,
        "TotalAmount" numeric(18,2) NOT NULL,
        "IsClosed" boolean NOT NULL,
        "Type" integer NOT NULL,
        "CustodianId" uuid,
        "Status" integer NOT NULL,
        "AcceptedOn" timestamp with time zone,
        "RejectionReason" character varying(500),
        "AcceptanceSignature_SignatureData" character varying(5000),
        "AcceptanceSignature_SignedOn" timestamp with time zone,
        "AcceptanceSignature_SignedByEmployeeId" uuid,
        "AcceptanceSignature_IpAddress" character varying(45),
        "AcceptanceSignature_UserAgent" character varying(500),
        "AcceptanceSignature_DeviceFingerprint" character varying(256),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Issuances" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Issuances_Employees_EmployeeId" FOREIGN KEY ("EmployeeId") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PurchaseRequests" (
        "Id" uuid NOT NULL,
        "RequestDate" timestamp with time zone NOT NULL,
        "RequestedBy" uuid NOT NULL,
        "Purpose" character varying(512) NOT NULL,
        "Status" integer NOT NULL,
        "ApprovalRemarks" character varying(1024),
        "ApprovedBy" uuid,
        "ApprovedOn" timestamp with time zone,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_PurchaseRequests" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_PurchaseRequests_Employees_RequestedBy" FOREIGN KEY ("RequestedBy") REFERENCES inventories."Employees" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."JournalEntries" (
        "Id" uuid NOT NULL,
        "JournalEntryVoucherId" uuid NOT NULL,
        "LineNumber" integer NOT NULL,
        "AccountCode" character varying(20) NOT NULL,
        "AccountName" character varying(150),
        "DebitAmount" numeric(18,2) NOT NULL,
        "CreditAmount" numeric(18,2) NOT NULL,
        "Description" character varying(500),
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_JournalEntries" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_JournalEntries_JournalEntryVouchers_JournalEntryVoucherId" FOREIGN KEY ("JournalEntryVoucherId") REFERENCES inventories."JournalEntryVouchers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ProcurementPlanItems" (
        "Id" uuid NOT NULL,
        "PlanHeaderId" uuid NOT NULL,
        "PapCode" character varying(50),
        "Description" character varying(500) NOT NULL,
        "ProjectType" integer NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitOfMeasure" character varying(50) NOT NULL,
        "UnitCost" numeric(18,2) NOT NULL,
        "EstimatedBudget" numeric(18,2) NOT NULL,
        "Mode" character varying(100) NOT NULL,
        "IsEarlyProcurement" boolean NOT NULL,
        "ScheduleMonth" character varying(20) NOT NULL,
        "FundingSource" character varying(200) NOT NULL,
        "Remarks" character varying(1000),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_ProcurementPlanItems" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ProcurementPlanItems_ProcurementPlans_PlanHeaderId" FOREIGN KEY ("PlanHeaderId") REFERENCES inventories."ProcurementPlans" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ProcurementSchedules" (
        "Id" uuid NOT NULL,
        "ProjectId" uuid NOT NULL,
        "AdsPosting" date,
        "PreBidConference" date,
        "BidOpening" date,
        "BidEvaluation" date,
        "PostQualification" date,
        "NoticeOfAward" date,
        "ContractSigning" date,
        "NoticeToProceeed" date,
        "DeliveryCompletion" date,
        CONSTRAINT "PK_ProcurementSchedules" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ProcurementSchedules_ProcurementProjects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES inventories."ProcurementProjects" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ProjectBudgets" (
        "Id" uuid NOT NULL,
        "ProjectId" uuid NOT NULL,
        "TotalAmount" numeric(18,2) NOT NULL,
        "MooeAmount" numeric(18,2) NOT NULL,
        "CoAmount" numeric(18,2) NOT NULL,
        CONSTRAINT "PK_ProjectBudgets" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ProjectBudgets_ProcurementProjects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES inventories."ProcurementProjects" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."IssuanceLineItem" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" character varying(200) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "AcquisitionDate" timestamp with time zone NOT NULL,
        "Quantity" numeric(18,4) NOT NULL,
        "Unit" character varying(50) NOT NULL,
        "UnitCost" numeric(18,2) NOT NULL,
        "SuppliesAndMaterialsIssuanceReportId" uuid NOT NULL,
        CONSTRAINT "PK_IssuanceLineItem" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_IssuanceLineItem_SuppliesAndMaterialsIssuanceReports_Suppli~" FOREIGN KEY ("SuppliesAndMaterialsIssuanceReportId") REFERENCES inventories."SuppliesAndMaterialsIssuanceReports" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ReceivingLineItem" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" character varying(200) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "Reference" character varying(100),
        "AcquisitionDate" timestamp with time zone NOT NULL,
        "Quantity" numeric(18,4) NOT NULL,
        "Unit" character varying(50) NOT NULL,
        "UnitCost" numeric(18,2) NOT NULL,
        "SuppliesAndMaterialsReceivingReportId" uuid NOT NULL,
        CONSTRAINT "PK_ReceivingLineItem" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ReceivingLineItem_SuppliesAndMaterialsReceivingReports_Supp~" FOREIGN KEY ("SuppliesAndMaterialsReceivingReportId") REFERENCES inventories."SuppliesAndMaterialsReceivingReports" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."ConsumableInventories" (
        "Id" uuid NOT NULL,
        "StockNumber" character varying(50) NOT NULL,
        "ProductId" uuid NOT NULL,
        "Description" character varying(500) NOT NULL,
        "UnitCost" numeric(18,2) NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitOfMeasure" character varying(50) NOT NULL,
        "WeightedAverageCost" numeric(18,2) NOT NULL,
        "Location" character varying(200),
        "ReorderLevel" integer NOT NULL DEFAULT 0,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_ConsumableInventories" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ConsumableInventories_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Inventories" (
        "Id" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "Qty" integer NOT NULL,
        "AvePrice" numeric NOT NULL,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Inventories" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Inventories_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."InventoryTransactions" (
        "Id" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "Qty" integer NOT NULL,
        "UnitCost" numeric NOT NULL,
        "Location" text,
        "SourceId" uuid NOT NULL,
        "TransactionType" integer NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_InventoryTransactions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_InventoryTransactions_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PhysicalAssets" (
        "Id" uuid NOT NULL,
        "PropertyCode" character varying(50) NOT NULL,
        "ProductId" uuid NOT NULL,
        "Description" character varying(500) NOT NULL,
        "AcquisitionCost" numeric(18,2) NOT NULL,
        "AcquisitionDate" timestamp with time zone NOT NULL,
        "SerialNumber" character varying(100),
        "ModelNumber" character varying(100),
        "Location" character varying(200),
        "Condition" character varying(50) NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitOfMeasure" character varying(50) NOT NULL,
        "EstimatedUsefulLife" integer NOT NULL,
        "DisposalDate" timestamp with time zone,
        "DisposalReason" character varying(500),
        "CurrentClassification" text NOT NULL,
        "PPEType" character varying(100),
        "AccumulatedDepreciation" numeric(18,2) NOT NULL,
        "QRCodeData" character varying(5000),
        "PropertyNumber" character varying(50),
        "QRGeneratedDate" timestamp with time zone,
        "CurrentCustodianId" uuid,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_PhysicalAssets" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_PhysicalAssets_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AssetRequisitions" (
        "Id" uuid NOT NULL,
        "EmployeeId" uuid NOT NULL,
        "IssuanceId" uuid NOT NULL,
        "RequisitionDate" timestamp with time zone NOT NULL,
        "ResponseDate" timestamp with time zone,
        "Status" integer NOT NULL,
        "RejectionReason" character varying(500),
        "ExpirationDate" timestamp with time zone,
        "AcceptanceSignature_SignatureData" character varying(5000),
        "AcceptanceSignature_SignedOn" timestamp with time zone,
        "AcceptanceSignature_SignedByEmployeeId" uuid,
        "AcceptanceSignature_IpAddress" character varying(45),
        "AcceptanceSignature_UserAgent" character varying(500),
        "AcceptanceSignature_DeviceFingerprint" character varying(256),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AssetRequisitions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AssetRequisitions_Employees_EmployeeId" FOREIGN KEY ("EmployeeId") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_AssetRequisitions_Issuances_IssuanceId" FOREIGN KEY ("IssuanceId") REFERENCES inventories."Issuances" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."IssuanceItems" (
        "Id" uuid NOT NULL,
        "IssuanceId" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "Qty" integer NOT NULL,
        "UnitPrice" numeric NOT NULL,
        "Status" text,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_IssuanceItems" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_IssuanceItems_Issuances_IssuanceId" FOREIGN KEY ("IssuanceId") REFERENCES inventories."Issuances" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_IssuanceItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Canvasses" (
        "Id" uuid NOT NULL,
        "PurchaseRequestId" uuid NOT NULL,
        "SupplierId" uuid NOT NULL,
        "ItemDescription" character varying(512) NOT NULL,
        "Quantity" integer NOT NULL,
        "Unit" character varying(50) NOT NULL,
        "QuotedPrice" numeric(18,2) NOT NULL,
        "Remarks" character varying(1024),
        "ResponseDate" timestamp with time zone NOT NULL,
        "IsSelected" boolean NOT NULL DEFAULT FALSE,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Canvasses" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Canvasses_PurchaseRequests_PurchaseRequestId" FOREIGN KEY ("PurchaseRequestId") REFERENCES inventories."PurchaseRequests" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Canvasses_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES inventories."Suppliers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PurchaseRequestItems" (
        "Id" uuid NOT NULL,
        "PurchaseRequestId" uuid NOT NULL,
        "ProductId" uuid,
        "ManualProductName" character varying(256),
        "Qty" integer NOT NULL,
        "Unit" character varying(50) NOT NULL,
        "Description" character varying(512),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_PurchaseRequestItems" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_PurchaseRequestItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id"),
        CONSTRAINT "FK_PurchaseRequestItems_PurchaseRequests_PurchaseRequestId" FOREIGN KEY ("PurchaseRequestId") REFERENCES inventories."PurchaseRequests" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AssetAssignmentHistories" (
        "Id" uuid NOT NULL,
        "AssetId" uuid NOT NULL,
        "AssetNumber" text NOT NULL,
        "EmployeeId" uuid NOT NULL,
        "EmployeeName" text NOT NULL,
        "DocumentNumber" text NOT NULL,
        "DocumentType" integer NOT NULL,
        "AssignmentDate" timestamp with time zone NOT NULL,
        "ReturnDate" timestamp with time zone,
        "AssignmentType" text NOT NULL,
        "Quantity" integer NOT NULL,
        "AssetClassification" integer NOT NULL,
        "Reason" text,
        "Remarks" text,
        "TransferredToEmployeeId" uuid,
        "TransferredToDocumentNumber" text,
        "Status" text NOT NULL,
        "Condition" text,
        "AcceptedBy" uuid,
        "AcceptanceDate" timestamp with time zone,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AssetAssignmentHistories" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AssetAssignmentHistories_Employees_AcceptedBy" FOREIGN KEY ("AcceptedBy") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_AssetAssignmentHistories_Employees_EmployeeId" FOREIGN KEY ("EmployeeId") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_AssetAssignmentHistories_Employees_TransferredToEmployeeId" FOREIGN KEY ("TransferredToEmployeeId") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_AssetAssignmentHistories_PhysicalAssets_AssetId" FOREIGN KEY ("AssetId") REFERENCES inventories."PhysicalAssets" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AssetReclassificationHistories" (
        "Id" uuid NOT NULL,
        "AssetId" uuid NOT NULL,
        "AssetNumber" text NOT NULL,
        "OldClassification" integer NOT NULL,
        "NewClassification" integer NOT NULL,
        "EffectiveDate" timestamp with time zone NOT NULL,
        "Reason" text NOT NULL,
        "AcquisitionCostAtReclassification" numeric NOT NULL,
        "COAReference" text,
        "Remarks" text,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AssetReclassificationHistories" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AssetReclassificationHistories_PhysicalAssets_AssetId" FOREIGN KEY ("AssetId") REFERENCES inventories."PhysicalAssets" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."DepreciationSchedules" (
        "Id" uuid NOT NULL,
        "PhysicalAssetId" uuid NOT NULL,
        "Month" integer NOT NULL,
        "Year" integer NOT NULL,
        "MonthlyDepreciationAmount" numeric(18,2) NOT NULL,
        "AccumulatedDepreciationAmount" numeric(18,2) NOT NULL,
        "Status" integer NOT NULL,
        "JournalEntryVoucherId" uuid,
        "PostedDate" timestamp with time zone,
        "Remarks" character varying(500),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_DepreciationSchedules" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_DepreciationSchedules_JournalEntryVouchers_JournalEntryVouc~" FOREIGN KEY ("JournalEntryVoucherId") REFERENCES inventories."JournalEntryVouchers" ("Id") ON DELETE SET NULL,
        CONSTRAINT "FK_DepreciationSchedules_PhysicalAssets_PhysicalAssetId" FOREIGN KEY ("PhysicalAssetId") REFERENCES inventories."PhysicalAssets" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Purchases" (
        "Id" uuid NOT NULL,
        "PurchaseRequestId" uuid,
        "SelectedCanvassId" uuid,
        "SupplierId" uuid NOT NULL,
        "PurchaseDate" timestamp with time zone NOT NULL,
        "TotalAmount" numeric NOT NULL,
        "Status" integer NOT NULL,
        "ReferenceNumber" text,
        "Remarks" text,
        "DeliveryAddress" character varying(256),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Purchases" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Purchases_Canvasses_SelectedCanvassId" FOREIGN KEY ("SelectedCanvassId") REFERENCES inventories."Canvasses" ("Id"),
        CONSTRAINT "FK_Purchases_PurchaseRequests_PurchaseRequestId" FOREIGN KEY ("PurchaseRequestId") REFERENCES inventories."PurchaseRequests" ("Id"),
        CONSTRAINT "FK_Purchases_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES inventories."Suppliers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."GoodsReceipt" (
        "Id" uuid NOT NULL,
        "PurchaseId" uuid NOT NULL,
        "SupplierId" uuid,
        "ReceivedById" uuid NOT NULL,
        "ReceivedOn" timestamp with time zone NOT NULL,
        "DeliveryNoteNumber" text,
        "Remarks" text,
        "Status" integer NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_GoodsReceipt" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_GoodsReceipt_Employees_ReceivedById" FOREIGN KEY ("ReceivedById") REFERENCES inventories."Employees" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_GoodsReceipt_Purchases_PurchaseId" FOREIGN KEY ("PurchaseId") REFERENCES inventories."Purchases" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_GoodsReceipt_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES inventories."Suppliers" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."InspectionRequests" (
        "Id" uuid NOT NULL,
        "PurchaseId" uuid NOT NULL,
        "InspectorId" uuid,
        "Status" integer NOT NULL,
        "DateCreated" timestamp with time zone NOT NULL,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_InspectionRequests" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_InspectionRequests_Employees_InspectorId" FOREIGN KEY ("InspectorId") REFERENCES inventories."Employees" ("Id"),
        CONSTRAINT "FK_InspectionRequests_Purchases_PurchaseId" FOREIGN KEY ("PurchaseId") REFERENCES inventories."Purchases" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Inspections" (
        "Id" uuid NOT NULL,
        "PurchaseId" uuid,
        "EmployeeId" uuid NOT NULL,
        "InspectedOn" timestamp with time zone NOT NULL,
        "Approved" boolean NOT NULL,
        "Status" character varying(32) NOT NULL,
        "Remarks" character varying(200),
        "IARDocumentPath" text,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Inspections" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Inspections_Employees_EmployeeId" FOREIGN KEY ("EmployeeId") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Inspections_Purchases_PurchaseId" FOREIGN KEY ("PurchaseId") REFERENCES inventories."Purchases" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."PurchaseItems" (
        "Id" uuid NOT NULL,
        "PurchaseId" uuid NOT NULL,
        "ProductId" uuid,
        "Qty" integer NOT NULL,
        "UnitPrice" numeric NOT NULL,
        "ItemStatus" integer NOT NULL,
        "InspectionStatus" integer NOT NULL,
        "AcceptanceStatus" integer NOT NULL,
        "QtyInspected" integer NOT NULL,
        "QtyPassed" integer NOT NULL,
        "QtyFailed" integer NOT NULL,
        "QtyAccepted" integer NOT NULL,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_PurchaseItems" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_PurchaseItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES inventories."Products" ("Id"),
        CONSTRAINT "FK_PurchaseItems_Purchases_PurchaseId" FOREIGN KEY ("PurchaseId") REFERENCES inventories."Purchases" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."Acceptances" (
        "Id" uuid NOT NULL,
        "PurchaseId" uuid NOT NULL,
        "SupplyOfficerId" uuid NOT NULL,
        "InspectionId" uuid,
        "GoodsReceiptId" uuid,
        "AcceptanceDate" timestamp with time zone NOT NULL,
        "Remarks" character varying(500),
        "IsPosted" boolean NOT NULL,
        "PostedOn" timestamp with time zone,
        "Status" integer NOT NULL,
        "PurchaseId1" uuid,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_Acceptances" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Acceptances_Employees_SupplyOfficerId" FOREIGN KEY ("SupplyOfficerId") REFERENCES inventories."Employees" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Acceptances_GoodsReceipt_GoodsReceiptId" FOREIGN KEY ("GoodsReceiptId") REFERENCES inventories."GoodsReceipt" ("Id") ON DELETE SET NULL,
        CONSTRAINT "FK_Acceptances_Inspections_InspectionId" FOREIGN KEY ("InspectionId") REFERENCES inventories."Inspections" ("Id") ON DELETE SET NULL,
        CONSTRAINT "FK_Acceptances_Purchases_PurchaseId" FOREIGN KEY ("PurchaseId") REFERENCES inventories."Purchases" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Acceptances_Purchases_PurchaseId1" FOREIGN KEY ("PurchaseId1") REFERENCES inventories."Purchases" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."GoodsReceiptItem" (
        "Id" uuid NOT NULL,
        "GoodsReceiptId" uuid NOT NULL,
        "PurchaseItemId" uuid NOT NULL,
        "QtyReceived" integer NOT NULL,
        "Condition" text,
        "Remarks" text,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_GoodsReceiptItem" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_GoodsReceiptItem_GoodsReceipt_GoodsReceiptId" FOREIGN KEY ("GoodsReceiptId") REFERENCES inventories."GoodsReceipt" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_GoodsReceiptItem_PurchaseItems_PurchaseItemId" FOREIGN KEY ("PurchaseItemId") REFERENCES inventories."PurchaseItems" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."InspectionItems" (
        "Id" uuid NOT NULL,
        "InspectionId" uuid NOT NULL,
        "PurchaseItemId" uuid NOT NULL,
        "QtyInspected" integer NOT NULL,
        "QtyPassed" integer NOT NULL,
        "QtyFailed" integer NOT NULL,
        "Remarks" character varying(500),
        "InspectionItemStatus" integer NOT NULL,
        "InspectionId1" uuid,
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_InspectionItems" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_InspectionItems_Inspections_InspectionId" FOREIGN KEY ("InspectionId") REFERENCES inventories."Inspections" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_InspectionItems_Inspections_InspectionId1" FOREIGN KEY ("InspectionId1") REFERENCES inventories."Inspections" ("Id"),
        CONSTRAINT "FK_InspectionItems_PurchaseItems_PurchaseItemId" FOREIGN KEY ("PurchaseItemId") REFERENCES inventories."PurchaseItems" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE TABLE inventories."AcceptanceItems" (
        "Id" uuid NOT NULL,
        "AcceptanceId" uuid NOT NULL,
        "PurchaseItemId" uuid NOT NULL,
        "QtyAccepted" integer NOT NULL,
        "Remarks" character varying(500),
        "TenantId" character varying(64) NOT NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" uuid NOT NULL,
        "LastModified" timestamp with time zone NOT NULL,
        "LastModifiedBy" uuid,
        "Deleted" timestamp with time zone,
        "DeletedBy" uuid,
        CONSTRAINT "PK_AcceptanceItems" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_AcceptanceItems_QtyAccepted_NonNegative" CHECK ("QtyAccepted" >= 0),
        CONSTRAINT "FK_AcceptanceItems_Acceptances_AcceptanceId" FOREIGN KEY ("AcceptanceId") REFERENCES inventories."Acceptances" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_AcceptanceItems_PurchaseItems_PurchaseItemId" FOREIGN KEY ("PurchaseItemId") REFERENCES inventories."PurchaseItems" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    INSERT INTO inventories."AssetConditionConfigurations" ("Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder")
    VALUES ('29bfc207-dead-40c7-a11e-9c415c0c0203', TRUE, 'Good', '#28a745', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, 'Asset is in excellent working condition', 'Good', TRUE, TIMESTAMPTZ '-infinity', NULL, FALSE, FALSE, 1);
    INSERT INTO inventories."AssetConditionConfigurations" ("Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder")
    VALUES ('3ba703de-c726-4551-9a0e-ad17dd905332', TRUE, 'Fair', '#ffc107', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, 'Asset has minor wear but still functional', 'Fair', TRUE, TIMESTAMPTZ '-infinity', NULL, FALSE, FALSE, 2);
    INSERT INTO inventories."AssetConditionConfigurations" ("Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder")
    VALUES ('57005b5e-6e1f-412a-a32e-6eba726f3a02', TRUE, 'Poor', '#fd7e14', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, 'Asset has significant wear, may need repair', 'Poor', TRUE, TIMESTAMPTZ '-infinity', NULL, FALSE, TRUE, 3);
    INSERT INTO inventories."AssetConditionConfigurations" ("Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder")
    VALUES ('de048632-a321-42e0-9539-27b4c845fc2a', FALSE, 'ForDisposal', '#6c757d', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, 'Asset is beyond repair and should be disposed', 'For Disposal', TRUE, TIMESTAMPTZ '-infinity', NULL, TRUE, FALSE, 5);
    INSERT INTO inventories."AssetConditionConfigurations" ("Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder")
    VALUES ('e9363f2a-d5f1-4ba0-8d49-b2be5f1bd5b7', FALSE, 'Unserviceable', '#dc3545', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, 'Asset is not functional, requires major repair', 'Unserviceable', TRUE, TIMESTAMPTZ '-infinity', NULL, FALSE, TRUE, 4);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    INSERT INTO inventories."PPETypeDefinitions" ("Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder")
    VALUES ('279c5996-2c49-4a03-9b7c-4532866f86d7', 'COA Circular 2022-002', 'General', 'OTHER', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', 10.0, 10, NULL, NULL, '10699010', 'Other property, plant and equipment', 'box', TRUE, TIMESTAMPTZ '-infinity', NULL, 'Other PPE', '10699990', 5);
    INSERT INTO inventories."PPETypeDefinitions" ("Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder")
    VALUES ('6cdbac2b-5059-4432-af9d-0f34fcddf797', 'COA Circular 2022-002', 'Production', 'MACHINERY', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', 10.0, 10, NULL, NULL, '10699010', 'Industrial machinery, tools, and equipment', 'gear', TRUE, TIMESTAMPTZ '-infinity', NULL, 'Machinery and Equipment', '10604010', 1);
    INSERT INTO inventories."PPETypeDefinitions" ("Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder")
    VALUES ('d20db417-b9bb-4501-979f-421fdfabf1dc', 'COA Circular 2022-002', 'Technology', 'ICT', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', 33.33, 3, NULL, NULL, '10699010', 'Computers, servers, network equipment', 'desktop', TRUE, TIMESTAMPTZ '-infinity', NULL, 'ICT Equipment', '10607010', 4);
    INSERT INTO inventories."PPETypeDefinitions" ("Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder")
    VALUES ('d323626a-39f7-469f-ba74-6de8ad1828b7', 'COA Circular 2022-002', 'Office', 'FURNITURE', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', 10.0, 10, NULL, NULL, '10699010', 'Office furniture, fixtures, and reference books', 'chair', TRUE, TIMESTAMPTZ '-infinity', NULL, 'Furniture, Fixtures and Books', '10606010', 3);
    INSERT INTO inventories."PPETypeDefinitions" ("Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder")
    VALUES ('ddf77535-144d-4cdc-b170-ccd1ac8fdf5a', 'COA Circular 2022-002', 'Transportation', 'TRANSPORTATION', TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', 20.0, 5, NULL, NULL, '10699010', 'Vehicles, motorcycles, boats', 'car', TRUE, TIMESTAMPTZ '-infinity', NULL, 'Transportation Equipment', '10605010', 2);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('013711c7-392a-43bb-ba3c-cf74118c51f3', 'L', NULL, 'L', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Liter', 20, 3);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('03ad19d1-a793-4dc0-a85f-b4c744b0c4c7', 'm', NULL, 'M', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Meter', 30, 4);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('3b836ff8-ab07-4eaa-b900-24649ed1af7a', 'can', NULL, 'CAN', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Can', 53, 6);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('56d921c7-d036-4bdb-8c1a-4e84903aa1b7', 'box', NULL, 'BOX', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Box', 50, 6);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('8e9ed494-654f-4bea-a97f-bac76bf92495', 'mm', NULL, 'MM', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Millimeter', 32, 4);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('949887b8-aea3-459d-80cb-26377cbc96fb', 'kg', NULL, 'KG', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Kilogram', 10, 2);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('974c58d0-0992-4638-84ae-530c9c22e75b', 'pair', NULL, 'PAIR', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Pair', 4, 1);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('98cd553f-3f8d-4dbe-9493-e1640db349ec', 'gal', NULL, 'GAL', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Gallon', 22, 3);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('a291c6dc-d2bc-4269-9de4-ee2a355207dc', 'MT', NULL, 'MT', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Metric Ton', 12, 2);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('a492716d-49ea-4b91-9e25-18f36b0a4bed', 'pc', NULL, 'PC', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, TRUE, TIMESTAMPTZ '-infinity', NULL, 'Piece', 1, 1);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('abe33f55-a670-413f-9ed8-63f7dfc2f3a7', 'pack', NULL, 'PACK', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Pack', 51, 6);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('aef13e59-6ca0-4347-852f-f303429d2b28', 'cm', NULL, 'CM', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Centimeter', 31, 4);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('b60cf50e-138d-4dee-8e62-35354e04d2ae', 'm²', NULL, 'SQM', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Square Meter', 40, 5);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('bfe7cc9c-50f4-405b-a118-5d45e460e78a', 'btl', NULL, 'BOTTLE', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Bottle', 52, 6);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('c3746b2c-9d07-455f-8a66-aab33ae90606', 'g', NULL, 'G', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Gram', 11, 2);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('d542f410-65ba-49b2-bb07-49416ba235df', 'unit', NULL, 'UNIT', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Unit', 3, 1);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('f329162f-fe04-45fc-96e7-8afab3a22e24', 'set', NULL, 'SET', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Set', 2, 1);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('f4ccfb87-72b2-41eb-b2c4-63fe9c627ed7', 'ft', NULL, 'FT', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Feet', 33, 4);
    INSERT INTO inventories."UnitsOfMeasure" ("Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType")
    VALUES ('fe1fb17e-7b77-4fe9-beaf-fd2aa94156a8', 'mL', NULL, 'ML', NULL, TIMESTAMPTZ '-infinity', '00000000-0000-0000-0000-000000000000', NULL, NULL, TRUE, FALSE, TIMESTAMPTZ '-infinity', NULL, 'Milliliter', 21, 3);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_AcceptanceItems_AcceptanceId_PurchaseItemId" ON inventories."AcceptanceItems" ("AcceptanceId", "PurchaseItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_AcceptanceItems_PurchaseItemId" ON inventories."AcceptanceItems" ("PurchaseItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_GoodsReceiptId" ON inventories."Acceptances" ("GoodsReceiptId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_InspectionId" ON inventories."Acceptances" ("InspectionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_IsPosted" ON inventories."Acceptances" ("IsPosted");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_PurchaseId" ON inventories."Acceptances" ("PurchaseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_PurchaseId1" ON inventories."Acceptances" ("PurchaseId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_Status" ON inventories."Acceptances" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Acceptances_SupplyOfficerId" ON inventories."Acceptances" ("SupplyOfficerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AnnualProcurementPlanItems_PlanHeaderId" ON inventories."AnnualProcurementPlanItems" ("PlanHeaderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_AnnualProcurementPlans_TenantId_ControlNumber" ON inventories."AnnualProcurementPlans" ("TenantId", "ControlNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetAssignmentHistories_AcceptedBy" ON inventories."AssetAssignmentHistories" ("AcceptedBy");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetAssignmentHistories_AssetId" ON inventories."AssetAssignmentHistories" ("AssetId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetAssignmentHistories_EmployeeId" ON inventories."AssetAssignmentHistories" ("EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetAssignmentHistories_TransferredToEmployeeId" ON inventories."AssetAssignmentHistories" ("TransferredToEmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_AssetConditionConfigurations_Code" ON inventories."AssetConditionConfigurations" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetReclassificationHistories_AssetId" ON inventories."AssetReclassificationHistories" ("AssetId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetRequisitions_EmployeeId" ON inventories."AssetRequisitions" ("EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetRequisitions_ExpirationDate" ON inventories."AssetRequisitions" ("ExpirationDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetRequisitions_IssuanceId" ON inventories."AssetRequisitions" ("IssuanceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_AssetRequisitions_Status" ON inventories."AssetRequisitions" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Canvass_PurchaseRequestId" ON inventories."Canvasses" ("PurchaseRequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_Canvass_PurchaseRequestId_SupplierId" ON inventories."Canvasses" ("PurchaseRequestId", "SupplierId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Canvass_SupplierId" ON inventories."Canvasses" ("SupplierId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_ConsumableInventories_ProductId" ON inventories."ConsumableInventories" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_ConsumableInventories_StockNumber" ON inventories."ConsumableInventories" ("StockNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_DepreciationSchedules_JournalEntryVoucherId" ON inventories."DepreciationSchedules" ("JournalEntryVoucherId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_DepreciationSchedules_PhysicalAssetId_Year_Month" ON inventories."DepreciationSchedules" ("PhysicalAssetId", "Year", "Month");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_DepreciationSchedules_Status" ON inventories."DepreciationSchedules" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_GoodsReceipt_PurchaseId" ON inventories."GoodsReceipt" ("PurchaseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_GoodsReceipt_ReceivedById" ON inventories."GoodsReceipt" ("ReceivedById");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_GoodsReceipt_SupplierId" ON inventories."GoodsReceipt" ("SupplierId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_GoodsReceiptItem_GoodsReceiptId" ON inventories."GoodsReceiptItem" ("GoodsReceiptId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_GoodsReceiptItem_PurchaseItemId" ON inventories."GoodsReceiptItem" ("PurchaseItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_InspectionItems_InspectionId" ON inventories."InspectionItems" ("InspectionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_InspectionItems_InspectionId1" ON inventories."InspectionItems" ("InspectionId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_InspectionItems_PurchaseItemId" ON inventories."InspectionItems" ("PurchaseItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_InspectionRequests_InspectorId" ON inventories."InspectionRequests" ("InspectorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_InspectionRequests_PurchaseId" ON inventories."InspectionRequests" ("PurchaseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Inspections_EmployeeId" ON inventories."Inspections" ("EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Inspections_PurchaseId" ON inventories."Inspections" ("PurchaseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Inventories_ProductId" ON inventories."Inventories" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_InventoryTransactions_ProductId" ON inventories."InventoryTransactions" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_IssuanceItems_IssuanceId" ON inventories."IssuanceItems" ("IssuanceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_IssuanceItems_ProductId" ON inventories."IssuanceItems" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_IssuanceLineItem_SuppliesAndMaterialsIssuanceReportId" ON inventories."IssuanceLineItem" ("SuppliesAndMaterialsIssuanceReportId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Issuances_CustodianId" ON inventories."Issuances" ("CustodianId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Issuances_EmployeeId" ON inventories."Issuances" ("EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Issuances_Status" ON inventories."Issuances" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_JournalEntries_AccountCode" ON inventories."JournalEntries" ("AccountCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_JournalEntries_JournalEntryVoucherId_LineNumber" ON inventories."JournalEntries" ("JournalEntryVoucherId", "LineNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_JournalEntryVouchers_Status" ON inventories."JournalEntryVouchers" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_JournalEntryVouchers_VoucherNumber" ON inventories."JournalEntryVouchers" ("VoucherNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_JournalEntryVouchers_Year_Month" ON inventories."JournalEntryVouchers" ("Year", "Month");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PhysicalAssets_CurrentClassification" ON inventories."PhysicalAssets" ("CurrentClassification");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PhysicalAssets_CurrentCustodianId" ON inventories."PhysicalAssets" ("CurrentCustodianId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PhysicalAssets_DisposalDate" ON inventories."PhysicalAssets" ("DisposalDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PhysicalAssets_ProductId" ON inventories."PhysicalAssets" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_PhysicalAssets_PropertyCode" ON inventories."PhysicalAssets" ("PropertyCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_PhysicalAssets_PropertyNumber" ON inventories."PhysicalAssets" ("PropertyNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_PpeIssuanceReport_ReportNumber" ON inventories."PpeIssuanceReport" ("ReportNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_PpeReceivingReport_ReportNumber" ON inventories."PpeReceivingReport" ("ReportNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_PPETypeAccountMappings_PPEType" ON inventories."PPETypeAccountMappings" ("PPEType") WHERE "IsActive" = true AND "Deleted" IS NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_PPETypeDefinitions_Code" ON inventories."PPETypeDefinitions" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_ProcurementPlanItems_PlanHeaderId" ON inventories."ProcurementPlanItems" ("PlanHeaderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_ProcurementPlans_TenantId_ControlNumber" ON inventories."ProcurementPlans" ("TenantId", "ControlNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_ProcurementSchedules_ProjectId" ON inventories."ProcurementSchedules" ("ProjectId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Products_CategoryId" ON inventories."Products" ("CategoryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Products_PropertyClassification" ON inventories."Products" ("PropertyClassification");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_ProjectBudgets_ProjectId" ON inventories."ProjectBudgets" ("ProjectId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PurchaseItems_ProductId" ON inventories."PurchaseItems" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PurchaseItems_PurchaseId" ON inventories."PurchaseItems" ("PurchaseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PurchaseRequestItems_ProductId" ON inventories."PurchaseRequestItems" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PurchaseRequestItems_PurchaseRequestId" ON inventories."PurchaseRequestItems" ("PurchaseRequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_PurchaseRequests_RequestedBy" ON inventories."PurchaseRequests" ("RequestedBy");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Purchases_PurchaseRequestId" ON inventories."Purchases" ("PurchaseRequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Purchases_SelectedCanvassId" ON inventories."Purchases" ("SelectedCanvassId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_Purchases_SupplierId" ON inventories."Purchases" ("SupplierId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_RcaAccountCodes_Key" ON inventories."RcaAccountCodes" ("Key");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_ReceivingLineItem_SuppliesAndMaterialsReceivingReportId" ON inventories."ReceivingLineItem" ("SuppliesAndMaterialsReceivingReportId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_SuppliesAndMaterialsIssuanceReports_SmirNumber" ON inventories."SuppliesAndMaterialsIssuanceReports" ("SmirNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_SuppliesAndMaterialsReceivingReports_SmrrNumber" ON inventories."SuppliesAndMaterialsReceivingReports" ("SmrrNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE INDEX "IX_UnitsOfMeasure_BaseUnitId" ON inventories."UnitsOfMeasure" ("BaseUnitId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    CREATE UNIQUE INDEX "IX_UnitsOfMeasure_Code" ON inventories."UnitsOfMeasure" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260117143824_Add Inventories Schema') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260117143824_Add Inventories Schema', '9.0.2');
    END IF;
END $EF$;
COMMIT;

