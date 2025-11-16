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

namespace AMIS.Blazor.Client.Pages.Catalog.PurchaseRequests;
public partial class PurchaseRequests
{
    private MudDataGrid<PurchaseRequestResponse> _table = default!;
    private HashSet<PurchaseRequestResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient Api { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private PurchaseRequestResponse _currentDto = new();
    private string searchString = string.Empty;
    private bool _loading;

    private IEnumerable<PurchaseRequestResponse>? _entityList;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.PurchaseRequests);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.PurchaseRequests);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.PurchaseRequests);
    }

    private async Task<GridData<PurchaseRequestResponse>> ServerReload(GridState<PurchaseRequestResponse> state)
    {
        _loading = true;
        var filter = new SearchPurchaseRequestsCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            Keyword = searchString
        };
        try
        {
            var result = await Api.SearchPurchaseRequestsEndpointAsync("1", filter);
            if (result != null)
            {
                _totalItems = result.TotalCount;
                _entityList = result.Items;
            }
            else
            {
                _totalItems = 0;
                _entityList = Array.Empty<PurchaseRequestResponse>();
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

        return new GridData<PurchaseRequestResponse> { TotalItems = _totalItems, Items = _entityList ?? Enumerable.Empty<PurchaseRequestResponse>() };
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

    private async Task OnCreate()
    {
        var model = new CreatePurchaseRequestCommand
        {
            RequestDate = DateTime.UtcNow,
            RequestedBy = GetCurrentUserId(),
            Purpose = string.Empty,
            Items = new List<PurchaseRequestItemDto>()
        };
        var parameters = new DialogParameters
        {
            { nameof(PurchaseRequestDialog.CreateModel), model },
            { nameof(PurchaseRequestDialog.IsCreate), true }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<PurchaseRequestDialog>("Create Purchase Request", parameters, options);
        var state = await dialog.Result;
        if (state != null && !state.Canceled)
        {
            await _table.ReloadServerData();
        }
    }

    private async Task OnView(PurchaseRequestResponse item)
    {
        // fetch latest details
        var dto = await Api.GetPurchaseRequestEndpointAsync("1", item.Id);
        var parameters = new DialogParameters
        {
            { nameof(PurchaseRequestDialog.ViewModel), dto },
            { nameof(PurchaseRequestDialog.IsCreate), false },
            { nameof(PurchaseRequestDialog.ReadOnly), true }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<PurchaseRequestDialog>("View Purchase Request", parameters, options);
        _ = await dialog.Result;
    }

    private async Task OnSubmit(PurchaseRequestResponse item)
    {
        try
        {
            await Api.SubmitPurchaseRequestEndpointAsync("1", item.Id);
            Snackbar?.Add("Purchase request submitted.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Submit failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnApprove(PurchaseRequestResponse item)
    {
        var remarks = await PromptAsync("Approval remarks (optional):");
        try
        {
            await Api.ApprovePurchaseRequestEndpointAsync("1", item.Id, new ApprovePurchaseRequestCommand { Remarks = remarks ?? string.Empty });
            Snackbar?.Add("Purchase request approved.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Approve failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnReject(PurchaseRequestResponse item)
    {
        var reason = await PromptAsync("Rejection reason:");
        if (string.IsNullOrWhiteSpace(reason)) return;
        try
        {
            await Api.RejectPurchaseRequestEndpointAsync("1", item.Id, new RejectPurchaseRequestCommand { Reason = reason });
            Snackbar?.Add("Purchase request rejected.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Reject failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnCancel(PurchaseRequestResponse item)
    {
        try
        {
            await Api.CancelPurchaseRequestEndpointAsync("1", item.Id);
            Snackbar?.Add("Purchase request canceled.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Cancel failed: {ex.Message}", Severity.Error);
        }
    }

    private static Color GetStatusColor(int status) => status switch
    {
        0 => Color.Default,      // Draft
        1 => Color.Info,         // Submitted
        2 => Color.Success,      // Approved
        3 => Color.Error,        // Rejected
        4 => Color.Secondary,    // Cancelled
        _ => Color.Default
    };

    private async Task<string?> PromptAsync(string title)
    {
        var parameters = new DialogParameters { { nameof(TextPrompt.ContentText), title } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<TextPrompt>("Input", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false } && result.Data is string s)
            return s;
        return null;
    }

    private Guid GetCurrentUserId()
    {
        try
        {
            var user = AuthState.GetAwaiter().GetResult().User;
            var idClaim = user?.Claims?.FirstOrDefault(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier", StringComparison.OrdinalIgnoreCase));
            if (idClaim != null && Guid.TryParse(idClaim.Value, out var id)) return id;
        }
        catch 
        {
            // ignored: unable to parse user id claim
        }
        return Guid.Empty;
    }
}
