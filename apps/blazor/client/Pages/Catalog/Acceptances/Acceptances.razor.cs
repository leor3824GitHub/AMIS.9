using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Shared.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Acceptances;

public partial class Acceptances
{
    private MudDataGrid<AcceptanceResponse?> _table = default!;
    private HashSet<AcceptanceResponse?> _selectedItems = new();
    private AcceptanceResponse? _current;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private bool _loading;
    private string _search = string.Empty;
    private IEnumerable<AcceptanceResponse?> _entityList = Enumerable.Empty<AcceptanceResponse?>();
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    private readonly List<EmployeeResponse> _supplyOfficers = new();
    private readonly List<PurchaseResponse> _purchaseLookup = new();

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Acceptances);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Acceptances);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Acceptances);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Acceptances);

        await LoadSupplyOfficersAsync();
        await LoadPurchaseLookupAsync();
    }

    private async Task LoadSupplyOfficersAsync()
    {
        try
        {
            var result = await ApiClient.SearchEmployeesEndpointAsync("1", new SearchEmployeesCommand
            {
                PageNumber = 1,
                PageSize = 200
            });

            _supplyOfficers.Clear();
            if (result?.Items != null)
            {
                _supplyOfficers.AddRange(result.Items);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load employees: {ex.Message}", Severity.Error);
        }
    }

    private async Task LoadPurchaseLookupAsync()
    {
        try
        {
            var result = await ApiClient.SearchPurchasesEndpointAsync("1", new SearchPurchasesCommand
            {
                PageNumber = 1,
                PageSize = 200
            });

            _purchaseLookup.Clear();
            if (result?.Items != null)
            {
                _purchaseLookup.AddRange(result.Items);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load purchases: {ex.Message}", Severity.Error);
        }
    }

    private async Task<GridData<AcceptanceResponse?>> ServerReload(GridState<AcceptanceResponse?> state)
    {
        _loading = true;
        var filter = new SearchAcceptancesCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            AdvancedSearch = new Search
            {
                Fields = new[] { "remarks" },
                Keyword = _search
            }
        };

        try
        {
            var result = await ApiClient.SearchAcceptancesEndpointAsync("1", filter);
            _entityList = result?.Items ?? Enumerable.Empty<AcceptanceResponse?>();
            _totalItems = result?.TotalCount ?? 0;
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading acceptances: {ex.Message}", Severity.Error);
            _entityList = Enumerable.Empty<AcceptanceResponse>();
            _totalItems = 0;
        }
        finally
        {
            _loading = false;
        }

        return new GridData<AcceptanceResponse?> { TotalItems = _totalItems, Items = _entityList };
    }

    private Task OnSearch(string value)
    {
        _search = value;
        return _table.ReloadServerData();
    }

    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
        _selectedItems.Clear();
    }

    private async Task OnCreate()
    {
        var model = AcceptanceFormModel.CreateDefault();
        await ShowDialog("Create acceptance", model, true, allowItemEditing: true);
    }

    private async Task OnEdit(AcceptanceResponse? dto)
    {
        if (dto is null)
        {
            return;
        }

        if (!CanEdit(dto))
        {
            Snackbar?.Add("Posted acceptances cannot be edited.", Severity.Info);
            return;
        }
        try
        {
            var acceptance = await ApiClient.GetAcceptanceEndpointAsync("1", dto.Id);

            // Get purchase with items to resolve product names
            PurchaseResponse? purchase = null;
            if (acceptance.PurchaseId != Guid.Empty)
            {
                purchase = await ApiClient.GetPurchaseEndpointAsync("1", acceptance.PurchaseId);
            }

            if (!_purchaseLookup.Any(p => p.Id == acceptance.PurchaseId))
            {
                if (purchase != null)
                {
                    _purchaseLookup.Add(purchase);
                }
            }

            var model = new AcceptanceFormModel
            {
                Id = acceptance.Id,
                AcceptanceDate = acceptance.AcceptanceDate,
                PurchaseId = acceptance.PurchaseId,
                SupplyOfficerId = acceptance.SupplyOfficerId,
                Remarks = acceptance.Remarks
            };

            var items = acceptance.Items?.Select(item =>
            {
                // Find matching purchase item to get product info
                var purchaseItem = purchase?.Items?.FirstOrDefault(pi => pi.Id == item.PurchaseItemId);

                return new AcceptanceFormModel.AcceptanceItemInput
                {
                    AcceptanceItemId = item.Id,
                    PurchaseItemId = item.PurchaseItemId,
                    OrderedQty = purchaseItem?.Qty ?? 0,
                    QtyAccepted = item.QtyAccepted,
                    ProductName = purchaseItem?.Product?.Name ?? "Unknown Product",
                    Remarks = item.Remarks
                };
            }) ?? Enumerable.Empty<AcceptanceFormModel.AcceptanceItemInput>();

            model.ReplaceItems(items);

            await ShowDialog("Edit acceptance", model, false, allowItemEditing: false);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load acceptance: {ex.Message}", Severity.Error);
        }
    }

    private async Task ShowDialog(string title, AcceptanceFormModel model, bool isCreate, bool allowItemEditing)
    {
        var parameters = new DialogParameters
        {
            { nameof(AcceptanceDialog.Model), model },
            { nameof(AcceptanceDialog.IsCreate), isCreate },
            { nameof(AcceptanceDialog.AllowItemEditing), allowItemEditing },
            { nameof(AcceptanceDialog.SupplyOfficers), _supplyOfficers },
            { nameof(AcceptanceDialog.Purchases), _purchaseLookup }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            FullWidth = true,
            MaxWidth = MaxWidth.ExtraLarge,
            BackdropClick = false
        };

        var dialog = await DialogService.ShowAsync<AcceptanceDialog>(title, parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnDelete(AcceptanceResponse? dto)
    {
        if (dto is null)
        {
            return;
        }

        if (!CanDelete(dto))
        {
            Snackbar?.Add("Posted acceptances cannot be deleted.", Severity.Info);
            return;
        }
        string content = $"You're sure you want to delete acceptance '{dto.Id}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), content }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.DeleteAcceptanceEndpointAsync("1", dto.Id),
                Snackbar ?? Toast);

            await _table.ReloadServerData();
        }
    }

    private async Task OnDeleteChecked()
    {
        if (_selectedItems.Count == 0)
        {
            Snackbar?.Add("No acceptances selected.", Severity.Info);
            return;
        }

        string content = "Delete selected acceptances?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), content }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            foreach (var item in _selectedItems.ToList())
            {
                if (item is null)
                {
                    continue;
                }

                if (!CanDelete(item))
                {
                    continue;
                }

                await ApiHelper.ExecuteCallGuardedAsync(
                    () => ApiClient.DeleteAcceptanceEndpointAsync("1", item.Id),
                    Snackbar ?? Toast);
            }

            _selectedItems.Clear();
            await _table.ReloadServerData();
        }
    }

    private async Task OnPost(AcceptanceResponse? dto)
    {
        if (dto is null) return;

        if (dto.IsPosted)
        {
            Snackbar?.Add("Acceptance is already posted.", Severity.Info);
            return;
        }

        try
        {
            await ApiClient.PostAcceptanceEndpointAsync("1", dto.Id);
            Snackbar?.Add("Acceptance posted successfully.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Post failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnLinkInspection(AcceptanceResponse? dto)
    {
        if (dto is null) return;

        // Show dialog to select inspection
        var parameters = new DialogParameters
        {
            { "PurchaseId", dto.PurchaseId }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<SelectInspectionDialog>("Link to Inspection", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: Guid inspectionId })
        {
            try
            {
                await ApiClient.LinkAcceptanceInspectionEndpointAsync("1", dto.Id, inspectionId);
                Snackbar?.Add("Acceptance linked to inspection.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Link failed: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnCancelAcceptance(AcceptanceResponse? dto)
    {
        if (dto is null) return;

        var parameters = new DialogParameters
        {
            { "Label", "Cancellation Reason" },
            { "Placeholder", "Enter reason for cancellation..." }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ReasonDialog>("Cancel Acceptance", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: string reason })
        {
            try
            {
                await ApiClient.CancelAcceptanceEndpointAsync("1", dto.Id, reason);
                Snackbar?.Add("Acceptance cancelled.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Cancel failed: {ex.Message}", Severity.Error);
            }
        }
    }

    private string GetSupplyOfficerName(AcceptanceResponse? acceptance)
    {
        if (acceptance?.SupplyOfficer != null)
        {
            return acceptance.SupplyOfficer.Name ?? "—";
        }

        var officer = _supplyOfficers.FirstOrDefault(o => o.Id == acceptance?.SupplyOfficerId);
        return officer?.Name ?? "—";
    }

    private string GetSupplierName(AcceptanceResponse? acceptance)
    {
        var purchase = acceptance is null
            ? null
            : _purchaseLookup.FirstOrDefault(p => p.Id == acceptance.PurchaseId);

        return purchase?.Supplier?.Name ?? "—";
    }

    private static Color GetStatusColor(AcceptanceResponse? acceptance)
    {
        if (acceptance is null)
        {
            return Color.Default;
        }

        return acceptance.Status switch
        {
            AcceptanceStatus.Pending => Color.Warning,
            AcceptanceStatus.Posted => Color.Success,
            AcceptanceStatus.Cancelled => Color.Dark,
            _ => Color.Default
        };
    }

    private bool CanEdit(AcceptanceResponse? acceptance) =>
        _canUpdate && acceptance is { Status: not AcceptanceStatus.Posted };

    private bool CanDelete(AcceptanceResponse? acceptance) =>
        _canDelete && acceptance is { Status: AcceptanceStatus.Pending };
}
