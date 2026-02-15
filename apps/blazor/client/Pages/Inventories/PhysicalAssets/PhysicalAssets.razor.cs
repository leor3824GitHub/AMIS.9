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
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private bool _loading;
    private bool _isReloading;
    private string _searchString = string.Empty;
    private IEnumerable<PhysicalAssetResponse> _entityList = Array.Empty<PhysicalAssetResponse>();
    private int _totalItems;
    private List<EmployeeResponse> _employees = new();
    private Guid _currentUserId;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canView = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PhysicalAssets);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.PhysicalAssets);
        _canEdit = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.PhysicalAssets);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.PhysicalAssets);
        
        // Get current user ID
        var userId = user?.FindFirst("sub")?.Value ?? "";
        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userGuid))
        {
            _currentUserId = userGuid;
        }
        
        await LoadEmployees();
    }

    private async Task LoadEmployees()
    {
        try
        {
            var searchFilter = new SearchEmployeesCommand
            {
                PageSize = 10000, // Load all employees
                PageNumber = 1
            };
            var result = await ApiClient.SearchEmployeesEndpointAsync("1", searchFilter);
            if (result?.Items != null)
            {
                _employees = result.Items.ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading employees: {ex.Message}", Severity.Warning);
        }
    }

    private string GetClassificationLabel(PropertyClassification classification)
    {
        return classification switch
        {
            PropertyClassification._2 => "Semi-Expendable",
            PropertyClassification._3 => "Property, Plant & Equipment",
            _ => classification.ToString()
        };
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

    private async Task OpenCreateDialog()
    {
        var dialog = await DialogService.ShowAsync<CreatePhysicalAssetDialog>("Create Physical Asset",
            new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await Reload();
        }
    }

    private async Task OpenEditDialog(Guid id)
    {
        var parameters = new DialogParameters<UpdatePhysicalAssetDialog>
        {
            { x => x.AssetId, id }
        };

        var dialog = await DialogService.ShowAsync<UpdatePhysicalAssetDialog>("Update Physical Asset", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await Reload();
        }
    }

    private async Task OpenDetailsDialog(Guid id)
    {
        var parameters = new DialogParameters<UpdatePhysicalAssetDialog>
        {
            { x => x.AssetId, id }
        };

        var dialog = await DialogService.ShowAsync<UpdatePhysicalAssetDialog>("Asset Details", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });

        await dialog.Result;
    }

    private async Task OpenIssueDialog(Guid id)
    {
        try
        {
            var asset = await ApiClient.GetPhysicalAssetEndpointAsync("1", id);
            if (asset == null)
            {
                Snackbar?.Add("Asset not found.", Severity.Error);
                return;
            }

        // Route based on classification
            if ((int)asset.Classification == 3)  // PropertyPlantEquipment
            {
                await OpenIssuePARDialog(id);
            }
            else
            {
                await OpenIssueICSDialog(id);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error opening issue dialog: {ex.Message}", Severity.Error);
        }
    }

    private async Task OpenIssueICSDialog(Guid id)
    {
        try
        {
            var asset = await ApiClient.GetPhysicalAssetEndpointAsync("1", id);
            if (asset == null)
            {
                Snackbar?.Add("Asset not found.", Severity.Error);
                return;
            }

            var dialog = await DialogService.ShowAsync<IssuePhysicalAssetDialog>(
                $"Issue Asset (ICS): {asset.PropertyCode}",
                new DialogParameters<IssuePhysicalAssetDialog> 
                { 
                    { x => x.AssetId, id },
                    { x => x.Employees, _employees }
                },
                new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;
            if (!result.Canceled)
            {
                _isReloading = true;
                try
                {
                    await Reload();
                }
                finally
                {
                    _isReloading = false;
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error opening ICS dialog: {ex.Message}", Severity.Error);
        }
    }

    private async Task OpenIssuePARDialog(Guid id)
    {
        try
        {
            var asset = await ApiClient.GetPhysicalAssetEndpointAsync("1", id);
            if (asset == null)
            {
                Snackbar?.Add("Asset not found.", Severity.Error);
                return;
            }

            var dialog = await DialogService.ShowAsync<IssuePARDialog>(
                $"Issue Asset (PAR): {asset.PropertyCode}",
                new DialogParameters<IssuePARDialog> 
                { 
                    { x => x.AssetId, id },
                    { x => x.Employees, _employees }
                },
                new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });

            var result = await dialog.Result;
            if (!result.Canceled)
            {
                _isReloading = true;
                try
                {
                    await Reload();
                }
                finally
                {
                    _isReloading = false;
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error opening PAR dialog: {ex.Message}", Severity.Error);
        }
    }

    private async Task OpenReturnDialog(Guid id)
    {
        try
        {
            var asset = await ApiClient.GetPhysicalAssetEndpointAsync("1", id);
            if (asset == null)
            {
                Snackbar?.Add("Asset not found.", Severity.Error);
                return;
            }

            var dialog = await DialogService.ShowAsync<ReturnPhysicalAssetDialog>(
                $"Return Asset: {asset.PropertyCode}",
                new DialogParameters<ReturnPhysicalAssetDialog> 
                { 
                    { x => x.AssetId, id },
                    { x => x.Employees, _employees }
                },
                new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Reload();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error opening return dialog: {ex.Message}", Severity.Error);
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        var confirmed = await DialogService.ShowMessageBox("Delete Physical Asset",
            "Are you sure you want to delete this physical asset? This action cannot be undone.",
            yesText: "Delete", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                await ApiClient.DeletePhysicalAssetEndpointAsync("1", id);
                Snackbar?.Add("Physical asset deleted successfully.", Severity.Success);
                await Reload();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error deleting physical asset: {ex.Message}", Severity.Error);
            }
        }
    }

    private bool CanReturnAsset(PhysicalAssetResponse asset)
    {
        // TODO: Implement separate query to check if current user is custodian
        return false; // Disabled until assignment tracking is implemented
    }
}
