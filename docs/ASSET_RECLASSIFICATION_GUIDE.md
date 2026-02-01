# Asset Reclassification Process for Threshold Changes

## Overview

When COA/DBM updates the PPE threshold (e.g., from ₱50,000 to ₱100,000), the system provides automated bulk reclassification of affected assets with full audit trail.

## Historical Threshold Changes

- **Before 2022**: ₱15,000
- **2022-Present**: ₱50,000  
- **Future**: Could be increased to ₱100,000 or higher

## Reclassification Scenarios

### Scenario 1: Threshold Increase (₱50,000 → ₱100,000)

**Assets Affected**: Those with acquisition cost between ₱50,001 and ₱100,000

**Current Classification**: Property, Plant & Equipment (PPE)  
**New Classification**: Semi-Expendable Property

**Impact**:
- Assets move from PPE registry to SEMEX registry
- Depreciation stops (semi-expendables are not depreciated)
- Document type changes from PAR to ICS
- Full audit trail maintained via `PhysicalAsset.ReclassificationHistory`

### Scenario 2: Threshold Decrease (₱50,000 → ₱30,000)

**Assets Affected**: Those with acquisition cost between ₱30,001 and ₱50,000

**Current Classification**: Semi-Expendable Property  
**New Classification**: Property, Plant & Equipment (PPE)

**Impact**:
- Assets move from SEMEX registry to PPE registry
- Depreciation starts (requires PPE Type assignment)
- Document type changes from ICS to PAR

## How to Execute Reclassification

### Step 1: Prepare the Command

```json
POST /api/v1/inventories/asset-management/reclassify
Content-Type: application/json
Authorization: Bearer {token}

{
  "newPPEThreshold": 100000,
  "effectiveDate": "2026-01-01",
  "coaReference": "COA Circular 2025-015",
  "reason": "Updated PPE threshold per COA Circular 2025-015"
}
```

### Step 2: Review Response

```json
{
  "assetsReclassified": 245,
  "oldThreshold": 50000,
  "newThreshold": 100000,
  "reclassificationDetails": [
    {
      "assetId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "propertyCode": "PPE-202401-00123",
      "description": "Desktop Computer",
      "acquisitionCost": 65000,
      "oldClassification": "PropertyPlantEquipment",
      "newClassification": "SemiExpendable"
    },
    // ... more assets
  ]
}
```

## What the System Does Automatically

1. **Updates Classification Rules**
   - Creates new `AssetClassificationRule` with new threshold
   - Sets expiry date on old rules
   - Maintains historical rule changes for audit

2. **Reclassifies Affected Assets**
   - Queries all `PhysicalAsset` records in the threshold range
   - Calls `asset.Reclassify()` with reason and effective date
   - Updates `CurrentClassification` and adds to `ReclassificationHistory`

3. **Transfers Registry Entries**
   - For PPE → Semi-Expendable:
     - Creates/updates entry in `SemexRegistry`
     - Maintains reference to original `PhysicalAsset`
   - For Semi-Expendable → PPE:
     - Requires PPE Type assignment (may need manual intervention)
     - Updates depreciation schedule

4. **Maintains Audit Trail**
   - All changes logged with:
     - Old and new classifications
     - Reason for change
     - COA/DBM reference
     - Effective date
     - User who triggered the change

## Database Schema Impact

### PhysicalAsset Table
```sql
-- Updated fields
CurrentClassification (enum: 1=Consumable, 2=SemiExpendable, 3=PPE)
PPEType (nullable - cleared when downgraded from PPE)

-- Audit trail (JSON column)
ReclassificationHistory: [
  {
    "Date": "2026-01-01T00:00:00Z",
    "OldClassification": "PropertyPlantEquipment",
    "NewClassification": "SemiExpendable",
    "Reason": "Updated PPE threshold per COA Circular 2025-015",
    "EffectiveDate": "2026-01-01"
  }
]
```

### AssetClassificationRule Table
```sql
-- Old rule (expired)
RuleName: "Property, Plant and Equipment"
MinimumCost: 50001
MaximumCost: 999999999
EffectiveDate: 2022-01-01
ExpiryDate: 2025-12-31
COAReference: "COA Circular 2022-004"

-- New rule (active)
RuleName: "Property, Plant and Equipment (Updated)"
MinimumCost: 100001
MaximumCost: 999999999
EffectiveDate: 2026-01-01
ExpiryDate: NULL
COAReference: "COA Circular 2025-015"
```

## Permissions Required

**Permission**: `Permissions.Inventories.Update`  
**Roles**: Admin, Accounting Officer

This is a sensitive operation that should only be performed by authorized personnel after official COA/DBM circular issuance.

## Best Practices

1. **Timing**: Execute reclassification on the effective date specified in the COA circular
2. **Backup**: Create database backup before running
3. **Testing**: Test on staging environment first with sample data
4. **Notification**: Notify all stakeholders (Supply Officers, Custodians, Accounting) of the change
5. **Reports**: Generate reclassification report for audit purposes
6. **Document Reprinting**: May need to reissue ICS/PAR documents for affected assets

## Rollback Procedure

If reclassification needs to be reversed:

```json
POST /api/v1/inventories/asset-management/reclassify
{
  "newPPEThreshold": 50000,
  "effectiveDate": "2026-01-02",
  "coaReference": "Reversal - Administrative correction",
  "reason": "Rollback of threshold change due to [reason]"
}
```

## Monitoring and Reporting

After reclassification, verify:
- Asset counts by classification
- SEMEX registry totals
- PPE registry totals
- Depreciation schedule impacts
- Document compliance (ICS vs PAR)

Generate reports:
```sql
-- Assets reclassified summary
SELECT 
    CurrentClassification,
    COUNT(*) as AssetCount,
    SUM(AcquisitionCost) as TotalValue
FROM PhysicalAssets
WHERE ReclassificationHistory IS NOT NULL
    AND ReclassificationHistory LIKE '%2026-01-01%'
GROUP BY CurrentClassification;
```

## Support Contact

For questions or issues with asset reclassification:
- **IT Support**: Check system logs in `Inventories.Application.Services.ReclassifyAssetsHandler`
- **Accounting**: Review audit trail in PhysicalAsset.ReclassificationHistory
- **COA Compliance**: Reference official circular documentation
