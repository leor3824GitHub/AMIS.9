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

namespace AMIS.Blazor.Client.Pages.Catalog.Inspections;

public partial class Inspections
{
    private MudDataGrid<InspectionResponse> _table = default!;
    private HashSet<InspectionResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient inspectionclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    private InspectionResponse _currentDto = new();
    private List<InspectionRequestResponse> _inspectionrequests = new List<InspectionRequestResponse>();
    private List<InspectionItemResponse> _inspectionitems = new List<InspectionItemResponse>();
    private List<EmployeeResponse> _employees = new List<EmployeeResponse>();
    private List<PurchaseResponse> _purchases = new List<PurchaseResponse>();

    private string searchString = "";
    private bool _loading;
    // Always keep a non-null enumerable for the grid
    private IEnumerable<InspectionResponse> _entityList = Enumerable.Empty<InspectionResponse>();
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    // simple cache for approve enablement per inspection id
    private readonly Dictionary<Guid, bool> _approveEnabledCache = new();
    // cache for whether an inspection already has an acceptance
    private readonly Dictionary<Guid, bool> _hasAcceptanceCache = new();

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Inspections);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Inspections);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Inspections);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Inspections);

        await LoadInspectionItemsAsync();
        await LoadInspectionRequestsAsync();
        await LoadPurchasesAsync();
        await LoadEmployeesAsync();

    }

    private async Task LoadInspectionRequestsAsync()
    {
        if (_inspectionrequests.Count == 0)
        {
            var response = await inspectionclient.SearchInspectionRequestsEndpointAsync("1", new SearchInspectionRequestsCommand());
            if (response?.Items != null)
            {
                _inspectionrequests = response.Items.ToList();
            }
        }
    }
    private async Task LoadInspectionItemsAsync()
    {
        if (_inspectionitems.Count == 0)
        {
            var response = await inspectionclient.SearchInspectionItemsEndpointAsync("1", new SearchInspectionItemsCommand());
            if (response?.Items != null)
            {
                _inspectionitems = response.Items.ToList();
            }
        }
    }

    private async Task LoadEmployeesAsync()
    {
        if (_employees.Count == 0)
        {
            var response = await inspectionclient.SearchEmployeesEndpointAsync("1", new SearchEmployeesCommand());
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
            var response = await inspectionclient.SearchPurchasesEndpointAsync("1", new SearchPurchasesCommand());
            if (response?.Items != null)
            {
                _purchases = response.Items.ToList();
            }
        }
    }
    private async Task<GridData<InspectionResponse>> ServerReload(GridState<InspectionResponse> state)
    {
        _loading = true;
        // default to empty so the grid never gets a null Items
        _entityList = Enumerable.Empty<InspectionResponse>();
        _totalItems = 0;

        var inspectionFilter = new SearchInspectionsCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            AdvancedSearch = new()
            {
                Fields = new[] { "purchaseId", "inspectorId" },
                Keyword = searchString
            }
        };

        try
        {
            var result = await inspectionclient.SearchInspectionsEndpointAsync("1", inspectionFilter);

            _approveEnabledCache.Clear();
            _hasAcceptanceCache.Clear();

            _totalItems = result?.TotalCount ?? 0;
            _entityList = result?.Items ?? Enumerable.Empty<InspectionResponse>();

            // Pre-compute acceptance existence per inspection for delete gating
            // Optimize to only query inspections that are InProgress (others can't be deleted anyway), and parallelize calls per page
            if (result?.Items != null)
            {
                var candidates = result.Items.Where(i => i.Status == InspectionStatus.InProgress).ToList();
                var tasks = candidates.Select(async insp =>
                {
                    try
                    {
                        // Acceptances are associated with Purchases, not Inspections.
                        // Use the inspection's PurchaseId to determine if any acceptances exist.
                        if (insp.PurchaseId.HasValue)
                        {
                            var acc = await inspectionclient.SearchAcceptancesEndpointAsync("1", new SearchAcceptancesCommand
                            {
                                PurchaseId = insp.PurchaseId.Value,
                                PageNumber = 1,
                                PageSize = 1
                            });
                            _hasAcceptanceCache[insp.Id] = (acc?.TotalCount ?? 0) > 0;
                        }
                        else
                        {
                            _hasAcceptanceCache[insp.Id] = false;
                        }
                    }
                    catch
                    {
                        _hasAcceptanceCache[insp.Id] = false;
                    }
                });
                await Task.WhenAll(tasks);
            }

        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading data: {ex.Message}", Severity.Error);
            _totalItems = 0;
            _entityList = Enumerable.Empty<InspectionResponse>();
        }
        finally
        {
            _loading = false;
        }

        return new GridData<InspectionResponse> { TotalItems = _totalItems, Items = _entityList };
    }

    private async Task ShowEditFormDialog(string title, UpdateInspectionCommand command, bool IsCreate, List<InspectionRequestResponse> inspectionrequests)
    {
        var parameters = new DialogParameters
        {
            { nameof(InspectionDialog.Model), command },
            { nameof(InspectionDialog.IsCreate), IsCreate },
            { nameof(InspectionDialog.InspectionRequests), inspectionrequests },
            { nameof(InspectionDialog.Employees), _employees }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InspectionDialog>(title, parameters, options);
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

    private async Task OnEdit(InspectionResponse item)
    {
        if (!CanEdit(item))
        {
            Snackbar?.Add("Only in-progress inspections can be edited.", Severity.Info);
            return;
        }

        var model = item.Adapt<UpdateInspectionCommand>();
        await ShowEditFormDialog("Edit inspection", model, false, _inspectionrequests);
    }

    private async Task OnDelete(InspectionResponse item)
    {
        if (!CanDelete(item))
        {
            Snackbar?.Add("Only in-progress inspections can be deleted.", Severity.Info);
            return;
        }

        string deleteContent = $"You're sure you want to delete inspection '{item.Id}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), deleteContent }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => inspectionclient.DeleteInspectionEndpointAsync("1", item.Id),
                Snackbar);

            await _table.ReloadServerData();
        }
    }

    private async Task OnDeleteChecked()
    {
        if (_selectedItems.Count == 0)
        {
            Snackbar?.Add("No inspections selected.", Severity.Info);
            return;
        }

        string deleteContent = "Delete selected inspections?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), deleteContent }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            foreach (var item in _selectedItems.ToList())
            {
                if (!CanDelete(item))
                {
                    continue;
                }

                await ApiHelper.ExecuteCallGuardedAsync(
                    () => inspectionclient.DeleteInspectionEndpointAsync("1", item.Id),
                    Snackbar);
            }

            _selectedItems.Clear();
            await _table.ReloadServerData();
        }
    }

    private async Task OnCreate()
    {
        var model = new UpdateInspectionCommand
        {
            InspectionDate = DateTime.Now,
            InspectorId = null,
            InspectionRequestId = null,
            Remarks = null
        };
        await ShowEditFormDialog("Create new inspection", model, true, _inspectionrequests);
    }

    private async Task OnApprove(InspectionResponse item)
    {
        try
        {
            await inspectionclient.ApproveInspectionEndpointAsync("1", item.Id);
            Snackbar?.Add("Inspection approved", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Approve failed: {ex.Message}", Severity.Error);
        }
    }

    private bool IsApproveEnabled(InspectionResponse item)
    {
        if (item == null) return false;
        if (_approveEnabledCache.TryGetValue(item.Id, out var enabled)) return enabled;

        if (item.Status != InspectionStatus.InProgress)
        {
            _approveEnabledCache[item.Id] = false;
            return false;
        }

        try
        {
            // enable if there exists at least one inspection item with status Passed or AcceptedWithDeviation
            var itemsResp = inspectionclient.SearchInspectionItemsEndpointAsync("1", new SearchInspectionItemsCommand { InspectionId = item.Id, PageNumber = 1, PageSize = 1 });
            itemsResp.Wait();
            var any = itemsResp.Result?.Items?.Any(i => i.InspectionItemStatus == InspectionItemStatus.Passed || i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation) == true;
            _approveEnabledCache[item.Id] = any;
            return any;
        }
        catch
        {
            _approveEnabledCache[item.Id] = false;
            return false;
        }
    }

    private static Color GetStatusColor(InspectionResponse? inspection)
    {
        if (inspection is null)
        {
            return Color.Default;
        }

        return inspection.Status switch
        {
            InspectionStatus.InProgress => Color.Warning,
            InspectionStatus.Completed => Color.Info,
            InspectionStatus.Approved => Color.Success,
            InspectionStatus.Rejected => Color.Error,
            InspectionStatus.Cancelled => Color.Dark,
            _ => Color.Default
        };
    }

    private bool CanEdit(InspectionResponse? inspection) =>
        _canUpdate && inspection is { Status: InspectionStatus.InProgress };

    private bool CanDelete(InspectionResponse? inspection)
    {
        if (!_canDelete || inspection is null || inspection.Status != InspectionStatus.InProgress)
            return false;

        return !_hasAcceptanceCache.TryGetValue(inspection.Id, out var has) || !has;
    }
}
