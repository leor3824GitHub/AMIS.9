using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Inventories.PhysicalAssets;

public partial class PhysicalAssets
{
    private MudDataGrid<PhysicalAssetResponse> _table = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private bool _canView;
    private bool _loading;
    private string _searchString = string.Empty;
    private IEnumerable<PhysicalAssetResponse> _entityList = Array.Empty<PhysicalAssetResponse>();
    private int _totalItems;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canView = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PhysicalAssets);
    }

    private async Task<GridData<PhysicalAssetResponse>> ServerReload(GridState<PhysicalAssetResponse> state)
    {
        _loading = true;

        var filter = new SearchPhysicalAssetsCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1
        };

        try
        {
            var result = await ApiClient.SearchPhysicalAssetsEndpointAsync("1", filter);
            if (result?.Items != null)
            {
                _entityList = ApplySearchFilter(result.Items);
                _totalItems = string.IsNullOrWhiteSpace(_searchString) ? result.TotalCount : _entityList.Count();
            }
            else
            {
                _entityList = Array.Empty<PhysicalAssetResponse>();
                _totalItems = 0;
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading physical assets: {ex.Message}", Severity.Error);
            _entityList = Array.Empty<PhysicalAssetResponse>();
            _totalItems = 0;
        }
        finally
        {
            _loading = false;
        }

        return new GridData<PhysicalAssetResponse> { TotalItems = _totalItems, Items = _entityList };
    }

    private IEnumerable<PhysicalAssetResponse> ApplySearchFilter(IEnumerable<PhysicalAssetResponse> items)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
        {
            return items;
        }

        return items.Where(asset =>
            (!string.IsNullOrWhiteSpace(asset.PropertyCode) && asset.PropertyCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(asset.Description) && asset.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(asset.UnitOfMeasure) && asset.UnitOfMeasure.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(asset.Location) && asset.Location.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(asset.SerialNumber) && asset.SerialNumber.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(asset.ModelNumber) && asset.ModelNumber.Contains(_searchString, StringComparison.OrdinalIgnoreCase)));
    }

    private async Task OnSearch(string text)
    {
        _searchString = text;
        await _table.ReloadServerData();
    }

    private async Task Reload()
    {
        await _table.ReloadServerData();
    }
}
