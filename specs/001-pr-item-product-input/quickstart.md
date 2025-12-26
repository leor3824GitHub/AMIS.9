# Quickstart: Purchase Request Item Product Input

## Goal
Verify that users can add Purchase Request items by either:
1) selecting a Product from the library, or
2) manually typing a product name (no ProductId).

## Prerequisites
- .NET 9 SDK
- Local database configured per repo defaults (PostgreSQL recommended)

## Run (local)

- API: `dotnet run --project api/server`
- Blazor client: `dotnet run --project apps/blazor/client`

## Manual verification steps

1. Open the Purchase Request create dialog.
2. In the item grid footer row:
   - Choose “Select from product list” and add an item; confirm Product label shows the library product.
3. Add a second item:
   - Choose “Manual entry”, type a product name, add item; confirm Product label shows the manual name.
4. Edit each item and switch modes:
   - Switching to Manual clears the selected ProductId.
   - Switching to Select clears the manual name.
5. Attempt invalid saves:
   - Manual entry with blank product name should block save.
   - Both ProductId and manual name set should be rejected (client-side and server-side).

## API contract regeneration (client)
After implementing server-side DTO changes, regenerate the Blazor API client (NSwag) and rebuild the Blazor projects so the new `manualProductName` field appears in the generated `PurchaseRequestItemCreateDto` / `PurchaseRequestItemResponse`.
