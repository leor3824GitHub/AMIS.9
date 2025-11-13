# Blazor UI Alignment with Aggregated Domain - Summary

## Changes Made

### 1. Removed Standalone Item Search Calls
The Blazor UI was attempting to search for `InspectionItems`, `AcceptanceItems`, and `PurchaseItems` independently, which violates DDD aggregate boundaries.

**Files Modified:**
- `apps/blazor/client/Pages/Catalog/Inspections/Inspections.razor.cs`
- `apps/blazor/client/Pages/Catalog/Acceptances/AcceptanceDialog.razor.cs`
- `apps/blazor/client/Pages/Catalog/Inspections/InspectionDialog.razor.cs`
- `apps/blazor/client/Pages/Catalog/Inspections/InspectionItemsEditor.razor`

### 2. Access Items Through Parent Aggregates
Items are now accessed through their parent aggregate root's response DTOs:
- `PurchaseItems` ? `PurchaseResponse.Items`
- `InspectionItems` ? `InspectionResponse.Items`
- `AcceptanceItems` ? `AcceptanceResponse.Items`

## Remaining Work

### Backend Changes Required

1. **Update Response DTOs to Include Items**

   Currently missing in API responses:
   
   ```csharp
   // PurchaseResponse needs Items collection
   public sealed record PurchaseResponse(
       Guid? Id,
       Guid? SupplierId,
       DateTime? PurchaseDate,
       decimal TotalAmount,
       PurchaseStatus? Status,
       SupplierResponse? Supplier,
       ICollection<PurchaseItemResponse>? Items  // ? ADDED
   );
   
   // InspectionResponse needs Items collection  
   public sealed record InspectionResponse(
       Guid Id,
       Guid? PurchaseId,
       Guid EmployeeId,
       DateTime InspectedOn,
       bool Approved,
       InspectionStatus Status,
       string? Remarks,
       string? IARDocumentPath,
       ICollection<InspectionItemResponse>? Items  // ?? NEEDS TO BE ADDED
   );
   
   // AcceptanceResponse needs Items collection
   public sealed record AcceptanceResponse(
       Guid Id,
       Guid PurchaseId,
       Guid SupplyOfficerId,
       DateTime AcceptanceDate,    
       string Remarks,
       bool IsPosted,
       DateTime? PostedOn,
       AcceptanceStatus Status,
       ICollection<AcceptanceItemResponse>? Items  // ?? NEEDS TO BE ADDED
   );
   ```

2. **Update Specification Queries**

   Specs need to include navigation properties and project items:
   
   ```csharp
   // Example: GetInspectionSpecs
   public class GetInspectionSpecs : Specification<Inspection, InspectionResponse>
   {
       public GetInspectionSpecs(Guid id)
       {
           Query
               .Where(i => i.Id == id)
               .Include(i => i.Employee)
               .Include(i => i.Items)  // Include items
                   .ThenInclude(item => item.PurchaseItem)
                       .ThenInclude(pi => pi.Product)
               .Select(i => new InspectionResponse(
                   i.Id,
                   i.PurchaseId,
                   i.EmployeeId,
                   i.InspectedOn,
                   i.Approved,
                   i.Status,
                   i.Remarks,
                   i.IARDocumentPath,
                   i.Items.Select(item => new InspectionItemResponse(
                       item.Id,
                       item.InspectionId,
                       item.PurchaseItemId,
                       item.QtyInspected,
                       item.QtyPassed,
                       item.QtyFailed,
                       item.Remarks,
                       item.InspectionItemStatus
                   )).ToList()
               ));
       }
   }
   ```

3. **Create Item Response DTOs**

   ```csharp
   // api/modules/Catalog/Catalog.Application/Inspections/Get/v1/InspectionItemResponse.cs
   public sealed record InspectionItemResponse(
       Guid Id,
       Guid InspectionId,
       Guid PurchaseItemId,
       int QtyInspected,
       int QtyPassed,
       int QtyFailed,
       string? Remarks,
       InspectionItemStatus InspectionItemStatus
   );
   
   // Already created: PurchaseItemResponse.cs
   // Already created: AcceptanceItemResponse.cs
   ```

4. **Update Search Specs Similarly**

   All search specifications need the same .Include() and .Select() treatment:
   - `SearchInspectionsSpecs`
   - `SearchPurchasesSpecs` (? already updated)
   - `SearchAcceptancesSpecs`

5. **Remove Standalone Item Endpoints (if any exist)**

   These should NOT exist as top-level routes:
   - ? `/inspectionItems`
   - ? `/purchaseItems` 
   - ? `/acceptanceItems`
   
   Items are managed through nested routes:
   - ? `/inspections/{id}/items`
   - ? `/purchases/{id}/items`
   - ? `/acceptances/{id}/items`

### Frontend Changes Required

1. **Regenerate NSwag API Client**

   After backend DTOs are updated:
   ```powershell
   cd apps/blazor/infrastructure
   dotnet build /t:GenerateClient
   ```

2. **Update Component Parameter Types**

   In `InspectionItemsEditor.razor`:
   ```csharp
   [Parameter] public ICollection<PurchaseItemResponse>? PurchaseItems { get; set; }
   ```

3. **Handle Null Collections**

   Add null-safe navigation throughout:
   ```csharp
   var hasPassedItems = item.Items?.Any(i => 
       i.InspectionItemStatus == InspectionItemStatus.Passed) == true;
   ```

## Benefits of This Architecture

1. **DDD Compliance**: Items are accessed through their aggregate root
2. **Single Source of Truth**: One query loads parent + items atomically  
3. **Performance**: Reduced N+1 queries with eager loading
4. **Consistency**: API enforces aggregate boundaries server-side
5. **Type Safety**: Client gets strongly-typed nested collections

## Testing Steps

1. Build backend after DTO updates
2. Run `dotnet ef database update` if schema changed
3. Regenerate Blazor API client  
4. Build Blazor client
5. Test workflows:
   - Create Purchase ? Add Items
   - Create Inspection ? Inspect Items  
   - Create Acceptance ? Accept Items
   - Verify single-shot constraints work

## Migration Notes

### Breaking Changes
- API responses now include Items collections
- Client code must handle nested collections
- Old direct item search endpoints removed

### Backward Compatibility
- Nested item management endpoints (`/purchases/{id}/items`) remain unchanged
- Command/Query handlers work the same way
- Only response shape changes (adds Items array)

## Next Steps

1. ? Create PurchaseItemResponse DTO
2. ?? Create InspectionItemResponse DTO
3. ?? Update InspectionResponse to include Items
4. ?? Update AcceptanceResponse to include Items
5. ?? Update all Get/Search specifications to include Items
6. ?? Regenerate NSwag client
7. ?? Test all CRUD workflows

Legend:
- ? = Completed
- ?? = Pending/In Progress
- ? = Should not exist
