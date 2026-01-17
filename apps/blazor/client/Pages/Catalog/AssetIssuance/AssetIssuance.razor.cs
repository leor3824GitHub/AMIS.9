using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Client.Pages.Catalog.AssetIssuance.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Catalog.AssetIssuance;

public partial class AssetIssuance
{
    private MudDataGrid<IssuanceResponse> _pendingTable = default!;
    private MudDataGrid<IssuanceResponse> _completedTable = default!;
    private MudDataGrid<IssuanceResponse> _allTable = default!;

    private AssetIssuanceDialog? _issuanceDialog;
    private DocumentPreviewDialog? _previewDialog;
    private IssuanceResponse? _selectedIssuanceForPreview;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private IEnumerable<IssuanceResponse> _pendingIssuances = Enumerable.Empty<IssuanceResponse>();
    private IEnumerable<IssuanceResponse> _completedIssuances = Enumerable.Empty<IssuanceResponse>();
    private IEnumerable<IssuanceResponse> _allIssuances = Enumerable.Empty<IssuanceResponse>();

    private int _totalItems;
    private bool _loading;
    private string _searchString = string.Empty;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Issuances);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Issuances);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Issuances);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Issuances);
    }

    // Server-side pagination methods
    private async Task<GridData<IssuanceResponse>> ServerReloadPending(GridState<IssuanceResponse> state)
    {
        _loading = true;
        try
        {
            var response = await ApiClient.SearchIssuancesEndpointAsync("1", new SearchIssuancesCommand
            {
                PageNumber = state.Page + 1,
                PageSize = state.PageSize,
                Keyword = _searchString
            });

            _pendingIssuances = (response?.Items ?? Enumerable.Empty<IssuanceResponse>()).Where(x => !x.IsClosed).ToList();
            _totalItems = _pendingIssuances.Count();

            return new GridData<IssuanceResponse>
            {
                Items = _pendingIssuances,
                TotalItems = _totalItems
            };
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading issuances: {ex.Message}", Severity.Error);
            return new GridData<IssuanceResponse> { Items = Enumerable.Empty<IssuanceResponse>(), TotalItems = 0 };
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task<GridData<IssuanceResponse>> ServerReloadCompleted(GridState<IssuanceResponse> state)
    {
        _loading = true;
        try
        {
            var response = await ApiClient.SearchIssuancesEndpointAsync("1", new SearchIssuancesCommand
            {
                PageNumber = state.Page + 1,
                PageSize = state.PageSize,
                Keyword = _searchString
            });

            _completedIssuances = (response?.Items ?? Enumerable.Empty<IssuanceResponse>()).Where(x => x.IsClosed).ToList();

            return new GridData<IssuanceResponse>
            {
                Items = _completedIssuances,
                TotalItems = _completedIssuances.Count()
            };
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading completed issuances: {ex.Message}", Severity.Error);
            return new GridData<IssuanceResponse> { Items = Enumerable.Empty<IssuanceResponse>(), TotalItems = 0 };
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task<GridData<IssuanceResponse>> ServerReloadAll(GridState<IssuanceResponse> state)
    {
        _loading = true;
        try
        {
            var response = await ApiClient.SearchIssuancesEndpointAsync("1", new SearchIssuancesCommand
            {
                PageNumber = state.Page + 1,
                PageSize = state.PageSize,
                Keyword = _searchString
            });

            _allIssuances = response?.Items ?? Enumerable.Empty<IssuanceResponse>();
            return new GridData<IssuanceResponse>
            {
                Items = _allIssuances,
                TotalItems = response?.TotalCount ?? 0
            };
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading issuances: {ex.Message}", Severity.Error);
            return new GridData<IssuanceResponse> { Items = Enumerable.Empty<IssuanceResponse>(), TotalItems = 0 };
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnSearchAsync(string search)
    {
        _searchString = search;
        await _pendingTable.ReloadServerData();
        await _completedTable.ReloadServerData();
        await _allTable.ReloadServerData();
    }

    private async Task OnCreateNewIssuance()
    {
        if (_issuanceDialog != null)
        {
            await _issuanceDialog.OpenCreateDialog();
        }
    }

    private async Task OnEditIssuance(IssuanceResponse issuance)
    {
        if (_issuanceDialog != null)
        {
            await _issuanceDialog.OpenEditDialog(issuance.Id ?? Guid.Empty);
        }
    }

    private async Task OnPreviewDocument(IssuanceResponse issuance)
    {
        _selectedIssuanceForPreview = issuance;
        if (_previewDialog != null)
        {
            await _previewDialog.OpenAsync();
        }
    }

    private async Task OnPrint(IssuanceResponse issuance)
    {
        Snackbar?.Add("Print functionality will be implemented", Severity.Info);
        // Implementation: Use JavaScript to trigger browser print
    }

    private async Task OnIssuanceCreated()
    {
        Snackbar?.Add("Issuance created successfully!", Severity.Success);
        await _pendingTable.ReloadServerData();
    }

    private async Task OnIssuanceUpdated()
    {
        Snackbar?.Add("Issuance updated successfully!", Severity.Success);
        await _pendingTable.ReloadServerData();
    }

    // Helper methods
    private string GetInitials(string? name) => 
        string.IsNullOrEmpty(name) ? "?" : new string(name.Split(' ').Select(x => x.FirstOrDefault()).ToArray());

    private string GetDocumentTypeLabel(string? type) => type switch
    {
        "PAR" => "PAR (Property Acknowledgment Receipt)",
        "ICS" => "ICS (Inventory Custodian Slip)",
        _ => "Unknown"
    };

    private Color GetDocumentTypeColor(string? type) => type switch
    {
        "PAR" => Color.Warning,
        "ICS" => Color.Info,
        _ => Color.Default
    };

    private Color GetStatusColor(string? status) => status switch
    {
        "Pending" => Color.Warning,
        "Accepted" => Color.Success,
        "Rejected" => Color.Error,
        "Returned" => Color.Secondary,
        "Cancelled" => Color.Default,
        _ => Color.Default
    };

    private string RowStylePending(IssuanceResponse response, int rowNumber) =>
        !response.IsClosed ? "background-color: rgba(255, 193, 7, 0.05);" : string.Empty;
}
