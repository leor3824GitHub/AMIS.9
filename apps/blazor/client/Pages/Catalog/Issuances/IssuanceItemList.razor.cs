using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Issuances;

public partial class IssuanceItemList
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }

    [Parameter]
    public ICollection<IssuanceItemUpsert> Items { get; set; } = new List<IssuanceItemUpsert>();
    [Parameter]
    public IReadOnlyList<ProductResponse> Products { get; set; } = Array.Empty<ProductResponse>();
    [Parameter]
    public Guid? IssuanceId { get; set; }
    [Parameter]
    public Action<double> OnTotalAmountChanged { get; set; } = _ => { };
    [Parameter]
    public bool? IsCreate { get; set; }

    private Guid? _productId;
    private Guid? ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            if (_productId.HasValue)
            {
                // default qty to 1 on selection and load inventory
                if (Qty < 1) Qty = 1;
                _ = EnsureInventoryLoadedAsync(_productId.Value);
            }
        }
    }
    private int Qty { get; set; }
    private double UnitPrice { get; set; }
    private string Status { get; set; } = "Pending";
    private IssuanceItemUpsert? EditingItem { get; set; }

    private sealed class InventorySnapshot
    {
        public int Qty { get; set; }
        public double AvePrice { get; set; }
    }

    private readonly Dictionary<Guid, InventorySnapshot> _inventoryCache = new();

    protected override void OnParametersSet()
    {
        Items ??= new List<IssuanceItemUpsert>();
        // optional: prefetch inventory for existing items (fire-and-forget)
        foreach (var pid in Items.Select(i => i.ProductId).Distinct())
        {
            _ = EnsureInventoryLoadedAsync(pid);
        }
    }

    private async System.Threading.Tasks.Task EnsureInventoryLoadedAsync(Guid productId)
    {
        if (_inventoryCache.ContainsKey(productId)) return;
        try
        {
            var resp = await ApiClient.SearchInventoriesEndpointAsync("1", new SearchInventoriesCommand
            {
                PageNumber = 1,
                PageSize = 1,
                ProductId = productId
            });

            var inv = resp?.Items?.FirstOrDefault();
            _inventoryCache[productId] = new InventorySnapshot
            {
                Qty = inv?.Qty ?? 0,
                AvePrice = inv?.AvePrice ?? 0d
            };
            _ = InvokeAsync(StateHasChanged);
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Failed to load inventory: {ex.Message}", Severity.Error);
            _inventoryCache[productId] = new InventorySnapshot { Qty = 0, AvePrice = 0d };
        }
    }

    private int GetBalanceFor(Guid? productId)
    {
        if (productId is null) return 0;
        var id = productId.Value;
        if (_inventoryCache.TryGetValue(id, out var snap))
        {
            return snap.Qty;
        }
        _ = EnsureInventoryLoadedAsync(id);
        return 0;
    }

    private double GetAvePriceFor(Guid? productId)
    {
        if (productId is null) return 0d;
        var id = productId.Value;
        if (_inventoryCache.TryGetValue(id, out var snap))
        {
            return snap.AvePrice;
        }
        _ = EnsureInventoryLoadedAsync(id);
        return 0d;
    }

    private void EditItem(IssuanceItemUpsert item)
    {
        EditingItem = item;
    }

    private async System.Threading.Tasks.Task SaveEdit()
    {
        if (EditingItem == null)
            return;

        try
        {
            // Enforce inventory constraints and pricing
            var balance = GetBalanceFor(EditingItem.ProductId);
            if (EditingItem.Qty < 1) EditingItem.Qty = 1;
            if (EditingItem.Qty > balance)
            {
                Snackbar?.Add($"Quantity exceeds balance ({balance}). Clamped to available.", Severity.Warning);
                EditingItem.Qty = balance;
            }
            EditingItem.UnitPrice = GetAvePriceFor(EditingItem.ProductId);

            // No individual API call needed - aggregate operation will handle this
            EditingItem = null;
            UpdateTotalAmount();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
        }
    }

    private void CancelEdit()
    {
        EditingItem = null;
    }

    private async System.Threading.Tasks.Task AddNewItem()
    {
        if (ProductId == null)
            return;

        // Ensure inventory snapshot exists
        var pid = ProductId.Value;
        if (!_inventoryCache.ContainsKey(pid))
        {
            await EnsureInventoryLoadedAsync(pid);
        }

        // Default and clamp
        if (Qty < 1) Qty = 1;
        var balance = GetBalanceFor(ProductId);
        if (Qty > balance)
        {
            Snackbar?.Add($"Quantity exceeds balance ({balance}). Clamped to available.", Severity.Warning);
            Qty = balance;
        }
        UnitPrice = GetAvePriceFor(ProductId);

        var newItem = new IssuanceItemUpsert
        {
            Id = Guid.NewGuid(),
            ProductId = ProductId.Value,
            Qty = Qty,
            UnitPrice = UnitPrice,
            Status = Status
        };

        Items.Add(newItem);

        // No individual API call needed - aggregate operation will handle this

        // Reset fields after adding
        ProductId = null;
        Qty = 1;
        UnitPrice = 0;
        Status = "Pending";

        UpdateTotalAmount();
    }

    private async System.Threading.Tasks.Task RemoveItem(IssuanceItemUpsert item)
    {
        try
        {
            // No individual API call needed - aggregate operation will handle deletion by comparing with existing items
            Items.Remove(item);
            UpdateTotalAmount();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
        }
    }

    private void UpdateTotalAmount()
    {
        double total = Items.Sum(i => i.Qty * GetAvePriceFor(i.ProductId));
        OnTotalAmountChanged?.Invoke(total);
        StateHasChanged();
    }
}
