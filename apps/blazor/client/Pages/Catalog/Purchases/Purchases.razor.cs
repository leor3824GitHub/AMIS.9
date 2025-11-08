using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;

public partial class Purchases
{
    private MudDataGrid<PurchaseResponse>? _table;
    private HashSet<PurchaseResponse> _selectedItems = new();
    // Removed status filter state with simplified UI.

    private PurchaseResponse _currentDto = new();
    private string _searchTerm = string.Empty;
    private bool _loading;
    private List<PurchaseResponse> _entityList = new();
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    // Removed filter & summary UI; old state retained only if future reinstatement is desired.

    // Removed advanced filter operator constants.

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject]
    protected IApiClient PurchaseClient { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Purchases);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Purchases);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Purchases);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Purchases);
    }

    private async Task<GridData<PurchaseResponse>> ServerReload(GridState<PurchaseResponse> state)
    {
        _loading = true;

        var request = new SearchPurchasesCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            Keyword = string.IsNullOrWhiteSpace(_searchTerm) ? null : _searchTerm,
            OrderBy = BuildOrderBy(state)
        };

        try
        {
            var result = await PurchaseClient.SearchPurchasesEndpointAsync("1", request);
            _totalItems = result?.TotalCount ?? 0;
            _entityList = result?.Items?.ToList() ?? new List<PurchaseResponse>();
            // Summary removed.
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }

        return new GridData<PurchaseResponse>
        {
            TotalItems = _totalItems,
            Items = _entityList
        };
    }

    private async Task ShowEditFormDialog(string title, CreatePurchaseCommand command, bool isCreate)
    {
        var parameters = new DialogParameters
        {
            { nameof(PurchaseDialog.Model), command },
            { nameof(PurchaseDialog.IsCreate), isCreate },
            { nameof(PurchaseDialog.Refresh), EventCallback.Factory.Create(this, OnRefresh) }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            BackdropClick = false,
            Position = DialogPosition.Center
        };

        var dialog = await DialogService.ShowAsync<PurchaseDialog>(title, parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false } && _table is not null)
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnCreate()
    {
        var model = CreateDefaultPurchaseCommand();
        await ShowEditFormDialog("Create new purchase", model, true);
    }

    private static CreatePurchaseCommand CreateDefaultPurchaseCommand() =>
        new()
        {
            PurchaseDate = DateTime.Today,
            SupplierId = null,
            Status = PurchaseStatus.Pending,
            Items = new List<PurchaseItemDto>()
        };

    // Clone removed in simplified toolbar version.
    private async Task OnClone()
    {
        var copy = _selectedItems.FirstOrDefault();
        if (copy is null)
        {
            Snackbar.Add("Select a purchase to clone.", Severity.Info);
            return;
        }

        var command = copy.Adapt<CreatePurchaseCommand>();
        command.Id = Guid.NewGuid();
        await ShowEditFormDialog("Clone purchase", command, true);
    }

    private async Task OnEdit(PurchaseResponse dto)
    {
        if (!_canUpdate)
        {
            Snackbar.Add("You do not have permission to edit purchases.", Severity.Warning);
            return;
        }

        var command = dto.Adapt<CreatePurchaseCommand>();
        await ShowEditFormDialog("Edit the purchase", command, false);
    }

    private async Task OnDelete(PurchaseResponse dto)
    {
        if (!dto.Id.HasValue)
        {
            Snackbar.Add("Purchase id missing.", Severity.Error);
            return;
        }

        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), $"You're sure you want to delete Purchase with id '{dto.Id}'?" }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackdropClick = false
        };

        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => PurchaseClient.DeletePurchaseEndpointAsync("1", dto.Id!.Value),
                Snackbar);

            if (_table is not null)
            {
                await _table.ReloadServerData();
            }
        }
    }

    private async Task OnDeleteChecked()
    {
        var purchaseIds = _selectedItems
            .Select(item => item.Id)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .ToList();

        if (purchaseIds.Count == 0)
        {
            Snackbar.Add("No items selected for deletion.", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), "Are you sure you want to delete the selected purchases?" }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackdropClick = false
        };

        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => PurchaseClient.DeletePurchasesEndpointAsync("1", purchaseIds),
                Snackbar);

            if (_table is not null)
            {
                await _table.ReloadServerData();
            }

            _selectedItems.Clear();
        }
    }

    private async Task OnRefresh()
    {
        if (_table is not null)
        {
            await _table.ReloadServerData();
        }

        _selectedItems.Clear();
    }

    private Task OnSearch(string text)
    {
        _searchTerm = text;
        return _table?.ReloadServerData() ?? Task.CompletedTask;
    }

    // Advanced filter logic removed with UI simplification.

    private static List<string>? BuildOrderBy(GridState<PurchaseResponse> state)
    {
        if (state.SortDefinitions is not { Count: > 0 })
        {
            return null;
        }

        return state.SortDefinitions
            .Where(definition => !string.IsNullOrWhiteSpace(definition.SortBy))
            .Select(definition => $"{definition.SortBy} {(definition.Descending ? "Desc" : "Asc")}")
            .ToList();
    }

    // Summary computation removed.

    // Supplier search removed with filter UI.

    // All filter-related handlers removed.

    private static Color GetStatusChipColor(PurchaseStatus status) => status switch
    {
        PurchaseStatus.PendingApproval or PurchaseStatus.Pending => Color.Warning,
        PurchaseStatus.OnHold or PurchaseStatus.Rejected or PurchaseStatus.Cancelled => Color.Error,
        PurchaseStatus.Approved or PurchaseStatus.Acknowledged or PurchaseStatus.FullyReceived or PurchaseStatus.Delivered or PurchaseStatus.Closed => Color.Success,
        PurchaseStatus.InProgress or PurchaseStatus.Shipped or PurchaseStatus.PartiallyReceived or PurchaseStatus.PartiallyDelivered or PurchaseStatus.PendingInvoice or PurchaseStatus.PendingPayment => Color.Info,
        _ => Color.Default
    };

    private static string GetStatusIcon(PurchaseStatus status) => status switch
    {
        PurchaseStatus.PendingApproval or PurchaseStatus.Pending => Icons.Material.Filled.HourglassEmpty,
        PurchaseStatus.OnHold => Icons.Material.Filled.PauseCircle,
        PurchaseStatus.Rejected => Icons.Material.Filled.HighlightOff,
        PurchaseStatus.Cancelled => Icons.Material.Filled.Cancel,
        PurchaseStatus.Approved => Icons.Material.Filled.ThumbUp,
        PurchaseStatus.Acknowledged => Icons.Material.Filled.MarkEmailRead,
        PurchaseStatus.InProgress => Icons.Material.Filled.DirectionsRun,
        PurchaseStatus.Shipped => Icons.Material.Filled.LocalShipping,
        PurchaseStatus.PartiallyReceived or PurchaseStatus.PartiallyDelivered => Icons.Material.Filled.Inventory,
        PurchaseStatus.FullyReceived or PurchaseStatus.Delivered => Icons.Material.Filled.Inventory2,
        PurchaseStatus.PendingInvoice or PurchaseStatus.PendingPayment => Icons.Material.Filled.RequestQuote,
        PurchaseStatus.Closed => Icons.Material.Filled.TaskAlt,
        _ => Icons.Material.Filled.Info
    };

    private static string GetStatusDisplay(PurchaseStatus status)
    {
        var member = typeof(PurchaseStatus).GetMember(status.ToString()).FirstOrDefault();
        var descriptionAttribute = member?.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? status.ToString();
    }

    // Old row class logic removed in favor of inline RowStyle.

    // Relative date helper (copied style from InspectionRequests page)
    private static string GetRelativeDateText(DateTime date)
    {
        var today = DateTime.Today;
        var days = (today - date.Date).Days;
        return days switch
        {
            0 => "Today",
            1 => "Yesterday",
            < 7 => $"{days} days ago",
            < 30 => $"{days / 7} week{(days / 7 > 1 ? "s" : "")} ago",
            < 365 => $"{days / 30} month{(days / 30 > 1 ? "s" : "")} ago",
            _ => $"{days / 365} year{(days / 365 > 1 ? "s" : "")} ago"
        };
    }

    // PurchaseSummary type removed; if needed later, reintroduce.

    // Inline style adaptation similar to InspectionRequests RowStyle
    private static string RowStyle(PurchaseResponse purchase, int _) => purchase.Status switch
    {
        PurchaseStatus.PendingApproval or PurchaseStatus.Pending => "background-color: rgba(255, 193, 7, 0.08);",
        PurchaseStatus.OnHold => "background-color: rgba(255, 152, 0, 0.05);",
        PurchaseStatus.Rejected or PurchaseStatus.Cancelled => "background-color: rgba(244, 67, 54, 0.08);",
        PurchaseStatus.Approved or PurchaseStatus.Acknowledged => "background-color: rgba(33, 150, 243, 0.05);",
        PurchaseStatus.FullyReceived or PurchaseStatus.Delivered or PurchaseStatus.Closed => "background-color: rgba(76, 175, 80, 0.06);",
        _ => string.Empty
    };
}

