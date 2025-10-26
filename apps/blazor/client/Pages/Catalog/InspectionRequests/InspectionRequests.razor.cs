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
            var response = await inspectionrequestclient.SearchPurchasesEndpointAsync("1", new SearchPurchasesCommand());
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

    private async Task ShowEditFormDialog(string title, UpdateInspectionRequestCommand command, bool IsCreate, List<EmployeeResponse> employees, List<PurchaseResponse> purchases)
    {
        var parameters = new DialogParameters
        {
            { nameof(InspectionRequestDialog.Model), command },
            { nameof(InspectionRequestDialog.IsCreate), IsCreate },
            { nameof(InspectionRequestDialog._employees), employees },
            { nameof(InspectionRequestDialog._purchases), purchases }
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
        // Open reassign inspector dialog or redirect
        var model = item.Adapt<UpdateInspectionRequestCommand>(); // Fix: Change the type to match the expected argument
        await ShowEditFormDialog("Re-assign to other inspector", model, false, _employees, _purchases);
    }
    private async Task OnAssign(InspectionRequestResponse item)
    {
        var model = item.Adapt<UpdateInspectionRequestCommand>(); // Fix: Change the type to match the expected argument
        await ShowEditFormDialog("Assign a inspector", model, false, _employees, _purchases);
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
        // Confirm and delete selected requests InspectionRequestDialog
        var model = _currentDto.Adapt<UpdateInspectionRequestCommand>(); // Fix: Change the type to match the expected argument
        await ShowEditFormDialog("Create new inspection request", model, true, _employees, _purchases);
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
}  
