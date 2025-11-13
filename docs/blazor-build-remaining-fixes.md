# Build Status After NSwag Regeneration

## ? Backend Status
**All backend compiles successfully!**

## ? Blazor Client Remaining Errors

### 1. Fixed: InspectionDialog._poItems
- **Issue**: InspectionDialog.razor referenced `_poItems` which was removed
- **Fix Applied**: Updated to use `GetPurchaseItemsForSelectedRequest()` method that accesses items through InspectionRequest.Purchase.Items aggregate
- **Status**: ? FIXED

### 2. Fixed: InspectionDialog CreateInspectionItemCommand
- **Issue**: Trying to create inspection items individually via non-existent command
- **Fix Applied**: Updated to pass all items in `CreateInspectionCommand.Items` list
- **Status**: ? FIXED

### 3. Remaining: AcceptanceDialog SearchAcceptanceItemsCommand
- **Issue**: `SearchAcceptanceItemsCommand` not found in generated API client
- **Root Cause**: Endpoint exists but NSwag may not have picked it up
- **Location**: `apps/blazor/client/Pages/Catalog/Acceptances/AcceptanceDialog.razor.cs` lines 178-185
- **Options**:
  - Option A: Regenerate NSwag client again (may need API running)
  - Option B: Remove single-shot checking from client (server enforces it anyway)
  - Option C: Use nested endpoint `/acceptances/{id}/items` search

### 4. Remaining: Acceptances.razor.cs AcceptanceItemResponse.PurchaseItem
- **Issue**: `AcceptanceItemResponse.PurchaseItem` navigation property missing
- **Location**: `apps/blazor/client/Pages/Catalog/Acceptances/Acceptances.razor.cs` lines 190-192
- **Root Cause**: AcceptanceItemResponse DTO doesn't include PurchaseItem navigation
- **Fix Needed**: Either:
  - Add PurchaseItem to AcceptanceItemResponse DTO and regenerate
  - Load purchase items separately when displaying acceptance details

### 5. Remaining: PurchaseItemList using old standalone endpoints
- **Issue**: `PurchaseItemList.razor.cs` uses:
  - `UpdatePurchaseItemCommand.Id` (doesn't exist)
  - `CreatePurchaseItemEndpointAsync` (standalone, should use nested)
  - `UpdatePurchaseItemEndpointAsync` (standalone, should use nested)
  - `DeletePurchaseItemEndpointAsync` (standalone, should use nested)
- **Location**: `apps/blazor/client/Pages/Catalog/Purchases/PurchaseItemList.razor.cs`
- **Fix Needed**: Use nested endpoints:
  - POST `/purchases/{purchaseId}/items`
  - PUT `/purchases/{purchaseId}/items/{itemId}`
  - DELETE `/purchases/{purchaseId}/items/{itemId}`

## Recommended Action Plan

### Immediate (to get build working):

1. **Fix AcceptanceDialog** - Remove single-shot client-side check:
   ```csharp
   // Comment out lines 175-191 in AcceptanceDialog.razor.cs
   // The server will enforce single-shot constraint anyway
   foreach (var input in Model.Items)
   {
       input.AlreadyAccepted = false; // Assume not accepted, server validates
   }
   ```

2. **Fix Acceptances.razor.cs** - Load purchase item info differently:
   ```csharp
   // Instead of item.PurchaseItem.Product.Name
   // Get product info from a lookup or acceptance.Items collection
   ```

3. **Fix PurchaseItemList.razor.cs** - Use nested endpoints or disable for now:
   ```csharp
   // Comment out the update/create/delete calls
   // Or convert to use nested endpoints with PurchaseId parameter
   ```

### Long-term (proper fix):

1. **Add PurchaseItem navigation to AcceptanceItemResponse**:
   ```csharp
   public sealed record AcceptanceItemResponse(
       Guid Id,
       Guid AcceptanceId,
       Guid PurchaseItemId,
       int QtyAccepted,
       string? Remarks,
       PurchaseItemResponse? PurchaseItem  // ADD THIS
   );
   ```

2. **Update GetAcceptanceSpecs to include PurchaseItem**:
   ```csharp
   .Include(a => a.Items)
       .ThenInclude(item => item.PurchaseItem)
           .ThenInclude(pi => pi.Product)
   ```

3. **Regenerate NSwag client** after backend DTOs updated

4. **Refactor PurchaseItemList** to use proper nested endpoints

## Quick Build Fix Commands

```powershell
# 1. Comment out problematic code in Blazor client files manually

# 2. Build Blazor client
dotnet build apps/blazor/client

# 3. If successful, full solution build
dotnet build
```

## Files Requiring Manual Edits

1. `apps/blazor/client/Pages/Catalog/Acceptances/AcceptanceDialog.razor.cs` (lines 175-191)
2. `apps/blazor/client/Pages/Catalog/Acceptances/Acceptances.razor.cs` (lines 190-192)
3. `apps/blazor/client/Pages/Catalog/Purchases/PurchaseItemList.razor.cs` (lines 60, 99, etc.)

Would you like me to apply the quick fixes to get the build working?
