using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Client.Pages.Catalog.Products;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using AMIS.Blazor.Client.Pages.Catalog.Inspections; // for InspectionDialog
using System.Linq; // ensure LINQ

namespace AMIS.Blazor.Client.Pages.Catalog.InspectionRequests;

public partial class InspectionRequests
{
    private MudDataGrid<InspectionRequestResponse> _table = default!;
    private HashSet<InspectionRequestResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient inspectionrequestclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    private InspectionRequestResponse _currentDto = new();
    private List<EmployeeResponse> _employees = new List<EmployeeResponse>();
    private List<PurchaseResponse> _purchases = new List<PurchaseResponse>();

    private string searchString = "";
    private bool _loading;
    private string successMessage = "";

    private IEnumerable<InspectionRequestResponse>? _entityList;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;


    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.InspectionRequests);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.InspectionRequests);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.InspectionRequests);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.InspectionRequests);

        await LoadPurchasesAsync();
        await LoadEmployeesAsync();

    }

    private async Task LoadEmployeesAsync()
    {
        if (_employees.Count == 0)
        {
            var response = await inspectionrequestclient.SearchEmployeesEndpointAsync("1", new SearchEmployeesCommand());
            if (response?.Items != null)
            {
                _employees = response.Items.ToList();
            }
        }
    }
    private async Task LoadPurchasesAsync()
    {
        if (_purchases.Count == 0)
        {
            var response = await inspectionrequestclient.SearchPurchasesEndpointAsync("1", new SearchPurchasesCommand { OnlyWithoutInspectionRequest = true, PageNumber = 1, PageSize = 200 });
            if (response?.Items != null)
            {
                _purchases = response.Items.ToList();
            }
        }
    }
    private async Task<GridData<InspectionRequestResponse>> ServerReload(GridState<InspectionRequestResponse> state)
    {
        _loading = true;
        var inspectionrequestFilter = new SearchInspectionRequestsCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            AdvancedSearch = new()
            {
                Fields = new[] { "purchaseId", "status", "assignedInspectorName" },
                Keyword = searchString
            }
        };

        try
        {
            var result = await inspectionrequestclient.SearchInspectionRequestsEndpointAsync("1", inspectionrequestFilter);

            if (result != null)
            {
                _totalItems = result.TotalCount;
                _entityList = result.Items;
            }
            else
            {
                _totalItems = 0;
                _entityList = new List<InspectionRequestResponse>();
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

        return new GridData<InspectionRequestResponse> { TotalItems = _totalItems, Items = _entityList };
    }

    private async Task ShowEditFormDialog(string title, UpdateInspectionRequestCommand command, bool IsCreate, List<EmployeeResponse> employees, List<PurchaseResponse> purchases, List<InspectionRequestResponse> inspectionrequests)
    {
        // Purchases already filtered server-side to exclude those with existing inspection requests
        var selectablePurchases = purchases;

        var parameters = new DialogParameters
        {
            { nameof(InspectionRequestDialog.Model), command },
            { nameof(InspectionRequestDialog.IsCreate), IsCreate },
            { nameof(InspectionRequestDialog._employees), employees },
            { nameof(InspectionRequestDialog._purchases), selectablePurchases }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InspectionRequestDialog>(title, parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }
    private async Task OnSearch(string value)
    {
        searchString = value;
        await _table.ReloadServerData();
    }

    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
    }

    private async Task ReAssign(InspectionRequestResponse item)
    {
        var model = item.Adapt<UpdateInspectionRequestCommand>();
        var currentRequests = (_entityList ?? Enumerable.Empty<InspectionRequestResponse>()).ToList();
        await ShowEditFormDialog("Re-assign to other inspector", model, false, _employees, _purchases, currentRequests);
    }
    private async Task OnAssign(InspectionRequestResponse item)
    {
        var model = item.Adapt<UpdateInspectionRequestCommand>();
        var currentRequests = (_entityList ?? Enumerable.Empty<InspectionRequestResponse>()).ToList();
        await ShowEditFormDialog("Assign a inspector", model, false, _employees, _purchases, currentRequests);
    }

    private async Task OnView(InspectionRequestResponse item)
    {
        // open dialog in read-only mode
        var model = item.Adapt<UpdateInspectionRequestCommand>();
        var parameters = new DialogParameters
        {
            { nameof(InspectionRequestDialog.Model), model },
            { nameof(InspectionRequestDialog.IsCreate), false },
            { nameof(InspectionRequestDialog._employees), _employees },
            { nameof(InspectionRequestDialog._purchases), _purchases },
            { nameof(InspectionRequestDialog.ReadOnly), true }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InspectionRequestDialog>("View inspection request", parameters, options);
        await dialog.Result; // no action on close
    }

    private async Task OnInspect(InspectionRequestResponse item)
    {
        // opens InspectionDialog to create inspection for this request
        var model = new UpdateInspectionCommand
        {
            InspectionDate = DateTime.Now,
            InspectorId = item.InspectorId,
            InspectionRequestId = item.Id,
            Remarks = string.Empty
        };

        var requestList = new List<InspectionRequestResponse> { item };
        var parameters = new DialogParameters
        {
            { nameof(AMIS.Blazor.Client.Pages.Catalog.Inspections.InspectionDialog.Model), model },
            { nameof(AMIS.Blazor.Client.Pages.Catalog.Inspections.InspectionDialog.IsCreate), true },
            { nameof(AMIS.Blazor.Client.Pages.Catalog.Inspections.InspectionDialog.InspectionRequests), requestList },
            { nameof(AMIS.Blazor.Client.Pages.Catalog.Inspections.InspectionDialog.Employees), _employees }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InspectionDialog>("Create Inspection", parameters, options);
        var state = await dialog.Result;
        if (!state.Canceled)
        {
            await _table.ReloadServerData();
        }
    }

    private async Task OnMarkCompleted(InspectionRequestResponse? item)
    {
        if (item is null) return;

        if (item.Status == InspectionRequestStatus.Completed)
        {
            Snackbar?.Add("Request is already completed.", Severity.Info);
            return;
        }

        try
        {
            await inspectionrequestclient.MarkCompletedInspectionRequestEndpointAsync("1", item.Id!.Value);
            Snackbar?.Add("Inspection request marked as completed.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Mark completed failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnMarkAccepted(InspectionRequestResponse? item)
    {
        if (item is null) return;

        if (item.Status == InspectionRequestStatus.Accepted)
        {
            Snackbar?.Add("Request is already accepted.", Severity.Info);
            return;
        }

        try
        {
            await inspectionrequestclient.MarkAcceptedInspectionRequestEndpointAsync("1", item.Id!.Value);
            Snackbar?.Add("Inspection request marked as accepted.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Mark accepted failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnUpdateStatus(InspectionRequestResponse? item)
    {
        if (item is null) return;

        // Show dialog to select new status
        var parameters = new DialogParameters
        {
            { "CurrentStatus", item.Status }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<UpdateStatusDialog>("Update Status", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: InspectionRequestStatus newStatus })
        {
            try
            {
                await inspectionrequestclient.UpdateStatusInspectionRequestEndpointAsync("1", item.Id!.Value, newStatus);
                Snackbar?.Add($"Status updated to {newStatus}.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Status update failed: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnDeleteChecked()
    {
        var inspectionrequestid = _selectedItems
        .Select(item => item.Id)
        .Where(id => id.HasValue)
        .Select(id => id.Value)
        .ToList();

        if (inspectionrequestid.Count == 0)
        {
            Snackbar?.Add("No items selected for deletion.", Severity.Warning);
            return;
        }

        string deleteContent = "Are you sure you want to delete the selected items?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), deleteContent }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            try
            {
                await ApiHelper.ExecuteCallGuardedAsync(
                    () => inspectionrequestclient.DeleteRangeInspectionRequestsEndpointAsync("1", inspectionrequestid),
                    Snackbar);

                await _table.ReloadServerData();
                _selectedItems.Clear();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error deleting inspection requests: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnCreate()
    {
        var model = _currentDto.Adapt<UpdateInspectionRequestCommand>();
        var currentRequests = (_entityList ?? Enumerable.Empty<InspectionRequestResponse>()).ToList();
        await ShowEditFormDialog("Create new inspection request", model, true, _employees, _purchases, currentRequests);
    }

    private static bool IsAlreadyInspected(InspectionRequestResponse request) =>
        request.Status is InspectionRequestStatus.Completed
            or InspectionRequestStatus.Failed
            or InspectionRequestStatus.Accepted;

    // Inspect is only enabled when the request is currently Assigned to an inspector
    private static bool ShouldDisableInspect(InspectionRequestResponse request) =>
        request.Status != InspectionRequestStatus.Assigned;

    // Assign/Re-Assign should be disabled once the request has already been inspected (completed/failed) or accepted
    private static bool ShouldDisableAssignActions(InspectionRequestResponse request) =>
        IsAlreadyInspected(request);

    private string RowStyle(InspectionRequestResponse request, int index)
    {
        return request.Status switch
        {
            InspectionRequestStatus.Completed => "background-color: rgba(76, 175, 80, 0.08);", // Green for completed
            InspectionRequestStatus.Failed => "background-color: rgba(244, 67, 54, 0.08);", // Red for failed
            InspectionRequestStatus.Accepted => "background-color: rgba(33, 150, 243, 0.08);", // Blue for accepted
            InspectionRequestStatus.Assigned => "background-color: rgba(255, 152, 0, 0.05);", // Light orange for assigned
            _ => string.Empty
        };
    }

    private string GetRelativeDateText(DateTime date)
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

    private string GetEmployeeInitials(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "?";

        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
            return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpperInvariant();

        return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
    }

    private Color GetStatusColor(InspectionRequestStatus status)
    {
        return status switch
        {
            InspectionRequestStatus.Pending => Color.Default,
            InspectionRequestStatus.Assigned => Color.Info,
            InspectionRequestStatus.Completed => Color.Success,
            InspectionRequestStatus.Failed => Color.Error,
            InspectionRequestStatus.Accepted => Color.Primary,
            _ => Color.Default
        };
    }

    private string GetStatusIcon(InspectionRequestStatus status)
    {
        return status switch
        {
            InspectionRequestStatus.Pending => Icons.Material.Filled.HourglassEmpty,
            InspectionRequestStatus.Assigned => Icons.Material.Filled.AssignmentTurnedIn,
            InspectionRequestStatus.Completed => Icons.Material.Filled.CheckCircle,
            InspectionRequestStatus.Failed => Icons.Material.Filled.Cancel,
            InspectionRequestStatus.Accepted => Icons.Material.Filled.TaskAlt,
            _ => Icons.Material.Filled.Help
        };
    }
}
