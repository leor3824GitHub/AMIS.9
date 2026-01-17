# Asset Issuance UI - Supply Officer PAR/ICS Management

## Overview

The Asset Issuance module provides a comprehensive UI for supply officers to issue assets to employees/custodians through two government-compliant document types:

- **PAR (Property Acknowledgment Receipt)** - For PPE (Property, Plant & Equipment) items costing > ₱50,000
- **ICS (Inventory Custodian Slip)** - For Semi-Expendable Property items costing ≤ ₱50,000

## Features

### 1. **Main Asset Issuance Page** (`/catalog/asset-issuance`)
Location: `apps/blazor/client/Pages/Catalog/AssetIssuance/AssetIssuance.razor`

#### Tabbed Interface:
- **Pending Issuances Tab** - Shows all pending issuances awaiting custodian acceptance
- **Completed Issuances Tab** - Shows accepted issuances with acceptance dates
- **All Issuances Tab** - Complete history of all issuances

#### Key Features:
- Multi-column data grid with employee avatars
- Document type indicators (PAR/ICS) with color coding
- Item count and total value display
- Real-time search functionality
- Status indicators with color-coded chips
- Quick actions: Preview, Edit, Print

### 2. **Asset Issuance Dialog** (`AssetIssuanceDialog.razor`)
Multi-step wizard for creating or editing issuances:

#### Step 1: Select Custodian
- Autocomplete employee search
- Auto-complete with employee name and designation
- Display selected custodian information
- Real-time validation

#### Step 2: Select Assets
- Inventory grid showing available items
- Classification indicator (PPE vs Semi-Expendable)
- Multiple item selection
- Quantity input with validation
- Running total calculation
- Summary table with edit capability

#### Step 3: Document Preview & Confirmation
- Automatic document type determination based on asset cost
- Full document preview before confirmation
- Review of custodian and items
- One-click confirmation with audit trail

### 3. **Document Preview Components**

#### DocumentPreviewDialog (`DocumentPreviewDialog.razor`)
- Modal dialog with tabbed interface
- Document preview tab showing full PAR/ICS format
- Details tab with comprehensive issuance information
- Print functionality
- Acceptance status display

#### DocumentPreviewInline (`DocumentPreviewInline.razor`)
- Renders complete PAR or ICS document
- Proper government-compliant formatting
- Professional layout with borders and sections
- Item tables with calculations
- Signature areas for custodian and supply officer
- Full details capture as per COA standards

## Document Types

### PAR (Property Acknowledgment Receipt)
**Used for**: PPE assets costing > ₱50,000

**Contains**:
- PAR number and issuance date
- Accountable officer signature line
- Custodian information (Name, Designation, Office)
- Detailed item table with:
  - Article/Description
  - Unit of Measure
  - Quantity
  - Unit Cost
  - Total Cost
- Certification section
- Custodian and date received signatures

### ICS (Inventory Custodian Slip)
**Used for**: Semi-Expendable Property ≤ ₱50,000

**Contains**:
- ICS number and issuance date
- Issued by (Supply Officer) line
- Custodian information (Name, Designation, Department)
- Detailed item table with:
  - Article/Description
  - Property Code (to be filled)
  - Quantity
  - Unit
  - Unit Cost
  - Total Value
- Remarks section
- Three-party signature line (Custodian, Supply Officer, Date Received)

## Component Structure

```
AssetIssuance/
├── AssetIssuance.razor                          # Main page
├── AssetIssuance.razor.cs                       # Code-behind with grid logic
└── Components/
    ├── AssetIssuanceDialog.razor                # Create/Edit wizard
    ├── AssetIssuanceDialog.razor.cs             # Dialog logic
    ├── DocumentPreviewDialog.razor              # Preview dialog
    ├── DocumentPreviewDialog.razor.cs           # Preview logic
    └── DocumentPreviewInline.razor              # Document rendering
```

## Usage Flow

### For Supply Officers Creating New Issuance:

1. Navigate to `/catalog/asset-issuance`
2. Click **"New Issuance"** button
3. **Step 1**: Search and select custodian
4. **Step 2**: Select assets from inventory
   - Choose items from available stock
   - Adjust quantities as needed
   - Review total amount
5. **Step 3**: Review document preview
   - Verify custodian and items
   - Check document type (PAR/ICS)
   - Confirm issuance
6. Document generated automatically
7. Custodian notified for acceptance

### For Managing Existing Issuances:

- **Preview**: View complete document in modal dialog
- **Edit**: Modify items before custodian acceptance
- **Print**: Generate printable PAR/ICS
- **Track**: Monitor acceptance status

## Data Models

### InventoryItemForIssuance
```csharp
public class InventoryItemForIssuance
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }
    public string PropertyCode { get; set; }
    public string Classification { get; set; }  // "PPE" or "Semi-Expendable"
    public int AvailableQty { get; set; }
    public decimal UnitPrice { get; set; }
    public int QuantityToIssue { get; set; }
}
```

### IssuanceItemDto
```csharp
public class IssuanceItemDto
{
    public Guid ProductId { get; set; }
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Status { get; set; }
}
```

## Backend Integration

The component integrates with existing API endpoints:

- `GetInventoriesAsync()` - Fetch available inventory
- `SearchEmployeesEndpointAsync()` - Search employees
- `CreateIssuanceAsync()` - Create new issuance
- `GetIssuancesAsync()` - Fetch issuances with filtering
- `GetIssuanceByIdAsync()` - Get specific issuance
- `UpdateIssuanceAsync()` - Update issuance items

## Authorization & Permissions

Uses existing Catalog module permissions:
- `Permissions.Issuances.Search` - Search/view issuances
- `Permissions.Issuances.Create` - Create new issuances
- `Permissions.Issuances.Update` - Edit issuances
- `Permissions.Issuances.Delete` - Delete issuances

## Navigation

Added to main navigation under **Catalog** section:
- **Menu Item**: Asset Issuance (PAR/ICS)
- **Icon**: QR Code 2
- **Route**: `/catalog/asset-issuance`

## Status Workflow

### Issuance Status Flow:
1. **Pending** - Awaiting custodian acceptance
2. **Accepted** - Custodian accepted (with signature)
3. **Rejected** - Custodian rejected (with reason)
4. **Returned** - Asset returned to inventory
5. **Cancelled** - Issuance cancelled

## UI/UX Features

### Color Coding:
- **PAR**: Warning color (Orange) - High-value PPE
- **ICS**: Info color (Blue) - Semi-Expendable
- **Pending**: Warning color (Yellow) - Awaiting action
- **Accepted**: Success color (Green) - Completed
- **Rejected/Cancelled**: Error/Default colors

### Responsive Design:
- Mobile-friendly data grids
- Responsive dialogs for all screen sizes
- Touch-friendly buttons and controls

### Professional Styling:
- MudBlazor Material Design components
- Proper spacing and typography
- Consistent color scheme
- Accessible color contrasts

## Printing & Export

### Print Functionality:
- Browser print-to-PDF support
- PAR/ICS formatted for standard paper
- Government-compliant layout
- Ready for filing and archival

### Future Enhancements:
- Email distribution
- Digital signature integration
- Barcode generation
- Batch issuance processing

## Testing Scenarios

1. **Create PAR** - Issue PPE item (> ₱50,000) to custodian
2. **Create ICS** - Issue semi-expendable item (≤ ₱50,000)
3. **Mixed Issuance** - Both PAR and ICS items in one issuance (generates appropriate docs)
4. **Search & Filter** - Find issuances by custodian or date
5. **Edit Items** - Modify quantities before acceptance
6. **Print Document** - Generate printable document
7. **View History** - Track all completed issuances

## API Requirements

The component requires the following API endpoints to be available:

```csharp
// Inventories
Task<PaginatedListDto<InventoryResponse>> GetInventoriesAsync(int pageNumber, int pageSize);

// Employees
Task<PaginatedListDto<EmployeeResponse>> SearchEmployeesEndpointAsync(string tenantId, SearchEmployeesCommand command);

// Issuances
Task<CreateIssuanceResponse> CreateIssuanceAsync(CreateIssuanceCommand command);
Task<GridData<IssuanceResponse>> GetIssuancesAsync(int pageNumber, int pageSize, string? status, string? search);
Task<IssuanceResponse> GetIssuanceByIdAsync(Guid id);
Task UpdateIssuanceAsync(Guid id, UpdateIssuanceCommand command);
```

## Future Enhancements

1. **Digital Signatures** - E-signature support for PAR/ICS
2. **QR Codes** - Generate QR codes for asset tracking
3. **Batch Operations** - Bulk issue assets to multiple custodians
4. **Email Notifications** - Automatic custodian notifications
5. **Custodian Portal** - Self-service acceptance interface
6. **Turnover Report** - Generate asset turnover summaries
7. **Return Management** - Track asset returns and reconciliation

## Troubleshooting

### Common Issues:

**Issue**: Inventory not loading
- **Solution**: Verify API endpoint `GetInventoriesAsync` is available and returning data

**Issue**: Employee search not working
- **Solution**: Check `SearchEmployeesEndpointAsync` endpoint and permissions

**Issue**: Document not showing correct type
- **Solution**: Verify product cost in inventory is correctly set (> or ≤ ₱50,000)

**Issue**: Print not working
- **Solution**: Ensure browser allows print-to-PDF functionality

## Configuration

No additional configuration required. Component inherits settings from:
- AMIS.9 main configuration
- API client settings
- Authorization policies
- Blazor options

## Dependencies

- MudBlazor 6.x+ (UI components)
- AMIS.Blazor.Infrastructure (API client)
- AMIS.Blazor.Client.Components (Shared components)
- System.ComponentModel.DataAnnotations

## Support & Maintenance

For issues or enhancements:
1. Check existing functionality in `AssetIssuance` and component files
2. Verify API endpoint compatibility
3. Review permission configurations
4. Check browser console for errors
5. Consult project architecture documentation
