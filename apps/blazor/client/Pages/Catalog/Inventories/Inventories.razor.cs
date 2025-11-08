using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Blazor.Infrastructure.Notifications;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MediatR.Courier;
using AMIS.Blazor.Shared.Notifications;

namespace AMIS.Blazor.Client.Pages.Catalog.Inventories;

public partial class Inventories
{
    private MudDataGrid<InventoryResponse> _table = default!;
    private HashSet<InventoryResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    [Inject]
    protected ICourier Courier { get; set; } = default!;

    private string searchString = string.Empty;
    private bool _loading;
    private IEnumerable<InventoryResponse>? _entityList;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    private List<ProductResponse> _products = new();

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Inventories);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Inventories);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Inventories);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Inventories);
        await LoadProductsAsync();

        // Subscribe to inventory change notifications to refresh the grid if this page is open
        Courier.SubscribeWeak<NotificationWrapper<InventoryChangedNotification>>(wrapper =>
        {
            // Marshal back to UI thread and reload without disrupting selection
            _ = InvokeAsync(async () =>
            {
                if (_table is not null)
                {
                    await _table.ReloadServerData();
                    Snackbar?.Add("Inventory updated", Severity.Info);
                }
            });
        });
    }

    private async Task LoadProductsAsync()
    {
        if (_products.Count == 0)
        {
            var response = await ApiClient.SearchProductsEndpointAsync("1", new SearchProductsCommand());
            if (response?.Items != null)
            {
                _products = response.Items.ToList();
            }
        }
    }

    private async Task<GridData<InventoryResponse>> ServerReload(GridState<InventoryResponse> state)
    {
        _loading = true;

        var filter = new SearchInventoriesCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            AdvancedSearch = new()
            {
                Fields = new[] { "product.name" },
                Keyword = searchString
            }
        };

        try
        {
            var result = await ApiClient.SearchInventoriesEndpointAsync("1", filter);
            if (result != null)
            {
                _totalItems = result.TotalCount;
                _entityList = result.Items;
            }
            else
            {
                _totalItems = 0;
                _entityList = Array.Empty<InventoryResponse>();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading data: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }

        return new GridData<InventoryResponse> { TotalItems = _totalItems, Items = _entityList };
    }

    private async Task ShowEditFormDialog(string title, InventoryEditModel model, bool isCreate)
    {
        var parameters = new DialogParameters
        {
            { nameof(InventoryDialog.Model), model },
            { nameof(InventoryDialog.IsCreate), isCreate },
            { nameof(InventoryDialog.Products), _products }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InventoryDialog>(title, parameters, options);
        var state = await dialog.Result;
        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnCreate()
    {
        var model = new InventoryEditModel
        {
            Id = Guid.Empty,
            ProductId = _products.FirstOrDefault()?.Id ?? Guid.Empty,
            Qty = 1,
            AvePrice = 1m
        };
        await ShowEditFormDialog("Create Inventory", model, true);
    }

    private async Task OnEdit(InventoryResponse dto)
    {
        var model = dto.Adapt<InventoryEditModel>();
        await ShowEditFormDialog("Edit Inventory", model, false);
    }

    private async Task OnDelete(InventoryResponse dto)
    {
        if (!dto.Id.HasValue) return;

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, "Inventory", dto.Id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.DeleteInventoryEndpointAsync("1", dto.Id.Value),
                Snackbar);

            await _table.ReloadServerData();
        }
    }

    private async Task OnDeleteChecked()
    {
        if (_selectedItems.Count == 0)
        {
            Snackbar?.Add("No items selected for deletion.", Severity.Warning);
            return;
        }

        string deleteContent = "Are you sure you want to delete the selected inventories?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), deleteContent }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            foreach (var item in _selectedItems)
            {
                if (item.Id.HasValue)
                {
                    await ApiHelper.ExecuteCallGuardedAsync(
                        () => ApiClient.DeleteInventoryEndpointAsync("1", item.Id.Value),
                        Snackbar);
                }
            }
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnSearch(string text)
    {
        searchString = text;
        await _table.ReloadServerData();
    }
 
    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
    }

    private string RowStyle(InventoryResponse inventory, int index)
    {
        if (inventory.Qty <= 0)
            return "background-color: rgba(244, 67, 54, 0.1);"; // Red tint for out of stock
        if (inventory.Qty <= 10)
            return "background-color: rgba(255, 152, 0, 0.08);"; // Orange tint for low stock
        return string.Empty;
    }

    private Color GetStockLevelColor(int qty)
    {
        if (qty <= 0) return Color.Error;
        if (qty <= 10) return Color.Warning;
        if (qty <= 50) return Color.Info;
        return Color.Success;
    }

    private string GetStockLevelIcon(int qty)
    {
        if (qty <= 0) return Icons.Material.Filled.ErrorOutline;
        if (qty <= 10) return Icons.Material.Filled.Warning;
        return Icons.Material.Filled.CheckCircle;
    }

    private string GetStockLevelText(int qty)
    {
        if (qty <= 0) return "Out of Stock";
        if (qty <= 10) return "Low Stock";
        if (qty <= 50) return "In Stock";
        return "Well Stocked";
    }
}

public class InventoryEditModel
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Qty { get; set; }
    public decimal AvePrice { get; set; }
}
