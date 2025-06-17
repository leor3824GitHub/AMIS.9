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
    private async Task OnAssign(InspectionRequestResponse item)
    {
        // Open assign inspector dialog or redirect
    }

    private async Task OnView(InspectionRequestResponse item)
    {
        // Show inspection request details
    }

    private async Task OnInspect(InspectionRequestResponse item)
    {
        // Navigate to inspection form
    }

    private async Task OnDeleteChecked()
    {
      
    }

    private async Task OnCreate()
    {
        // Confirm and delete selected requests InspectionRequestDialog
        var model = _currentDto.Adapt<UpdateInspectionRequestCommand>(); // Fix: Change the type to match the expected argument
        await ShowEditFormDialog("Create new inspection request", model, true, _employees, _purchases);
    }
}  
