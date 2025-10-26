using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Issuances;

public partial class Issuances
{
    private MudDataGrid<IssuanceResponse> _table = default!;
    private HashSet<IssuanceResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    private readonly List<EmployeeResponse> _employees = new();
    private readonly List<ProductResponse> _products = new();
    private IEnumerable<IssuanceResponse> _entityList = Enumerable.Empty<IssuanceResponse>();
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

        await LoadEmployeesAsync();
        await LoadProductsAsync();
    }

    private async Task LoadEmployeesAsync()
    {
        if (_employees.Count > 0)
        {
            return;
        }

        try
        {
            var response = await ApiClient.SearchEmployeesEndpointAsync("1", new SearchEmployeesCommand
            {
                PageNumber = 1,
                PageSize = 250,
                OrderBy = new[] { "name" }
            });

            if (response?.Items != null)
            {
                _employees.Clear();
                _employees.AddRange(response.Items);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load employees: {ex.Message}", Severity.Error);
        }
    }

    private async Task LoadProductsAsync()
    {
        if (_products.Count > 0)
        {
            return;
        }

        try
        {
            var response = await ApiClient.SearchProductsEndpointAsync("1", new SearchProductsCommand
            {
                PageNumber = 1,
                PageSize = 250,
                OrderBy = new[] { "name" }
            });

            if (response?.Items != null)
            {
                _products.Clear();
                _products.AddRange(response.Items);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load products: {ex.Message}", Severity.Error);
        }
    }

    private async Task<GridData<IssuanceResponse>> ServerReload(GridState<IssuanceResponse> state)
    {
        _loading = true;

        var command = new SearchIssuancesCommand
        {
            PageNumber = state.Page + 1,
            PageSize = state.PageSize,
            Keyword = string.IsNullOrWhiteSpace(_searchString) ? null : _searchString,
            AdvancedSearch = string.IsNullOrWhiteSpace(_searchString)
                ? null
                : new()
                {
                    Fields = new List<string> { "employee.name" },
                    Keyword = _searchString
                }
        };

        try
        {
            var result = await ApiClient.SearchIssuancesEndpointAsync("1", command);
            _totalItems = result?.TotalCount ?? 0;
            _entityList = result?.Items ?? Enumerable.Empty<IssuanceResponse>();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load issuances: {ex.Message}", Severity.Error);
            _totalItems = 0;
            _entityList = Enumerable.Empty<IssuanceResponse>();
        }
        finally
        {
            _loading = false;
        }

        return new GridData<IssuanceResponse>
        {
            Items = _entityList,
            TotalItems = _totalItems
        };
    }

    private async Task OnSearchAsync(string keyword)
    {
        _searchString = keyword;
        await _table.ReloadServerData();
    }

    private async Task OnCreate()
    {
        if (_employees.Count == 0)
        {
            Snackbar?.Add("Please add an employee before creating an issuance.", Severity.Warning);
            return;
        }

        var defaultEmployeeId = _employees
            .Select(employee => employee.Id)
            .FirstOrDefault(id => id.HasValue) ?? Guid.Empty;

        var model = new IssuanceEditModel
        {
            Id = Guid.Empty,
            IssuanceDate = DateTime.Today,
            TotalAmount = 0,
            IsClosed = false,
            EmployeeId = defaultEmployeeId,
            Items = new()
        };

        await ShowDialogAsync("Create issuance", model, isCreate: true);
    }

    private async Task OnEdit(IssuanceResponse dto)
    {
        var model = new IssuanceEditModel
        {
            Id = dto.Id ?? Guid.Empty,
            EmployeeId = dto.EmployeeId,
            IssuanceDate = dto.IssuanceDate,
            TotalAmount = Convert.ToDecimal(dto.TotalAmount),
            IsClosed = dto.IsClosed
        };

        try
        {
            if (dto.Id.HasValue)
            {
                var itemsResp = await ApiClient.SearchIssuanceItemsEndpointAsync("1", new SearchIssuanceItemsCommand
                {
                    PageNumber = 1,
                    PageSize = 250,
                    IssuanceId = dto.Id.Value
                });

                model.Items = itemsResp?.Items?
                    .Select(x => new IssuanceItemDto
                    {
                        Id = x.Id ?? Guid.Empty,
                        IssuanceId = x.IssuanceId,
                        ProductId = x.ProductId,
                        Qty = x.Qty,
                        UnitPrice = x.UnitPrice,
                        Status = x.Status
                    })
                    .ToList() ?? new();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Failed to load issuance items: {ex.Message}", Severity.Error);
            model.Items = new();
        }

        await ShowDialogAsync("Edit issuance", model, isCreate: false);
    }

    private async Task ShowDialogAsync(string title, IssuanceEditModel model, bool isCreate)
    {
        var parameters = new DialogParameters
        {
            { nameof(IssuanceDialog.Model), model },
            { nameof(IssuanceDialog.IsCreate), isCreate },
            { nameof(IssuanceDialog.Employees), _employees },
            { nameof(IssuanceDialog.Products), _products }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<IssuanceDialog>(title, parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadServerData();
        }
    }

    private async Task OnDelete(IssuanceResponse dto)
    {
        if (!dto.Id.HasValue)
        {
            return;
        }

        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), $"You're sure you want to delete issuance '{dto.Id}'?" }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await ApiHelper.ExecuteCallGuardedAsync(() => ApiClient.DeleteIssuanceEndpointAsync("1", dto.Id.Value), Snackbar!);
            await _table.ReloadServerData();
        }
    }

    private async Task OnDeleteSelected()
    {
        if (_selectedItems.Count == 0)
        {
            Snackbar?.Add("No issuances selected.", Severity.Info);
            return;
        }

        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), "Delete selected issuances?" }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            var ids = _selectedItems
                .Where(item => item.Id.HasValue)
                .Select(item => item.Id!.Value)
                .ToList();

            foreach (var id in ids)
            {
                await ApiHelper.ExecuteCallGuardedAsync(() => ApiClient.DeleteIssuanceEndpointAsync("1", id), Snackbar!);
            }

            _selectedItems.Clear();
            await _table.ReloadServerData();
        }
    }

    private async Task OnToggleClosedAsync(IssuanceResponse dto)
    {
        if (!dto.Id.HasValue)
        {
            return;
        }

        if (!_canUpdate)
        {
            Snackbar?.Add("You do not have permission to update issuances.", Severity.Warning);
            return;
        }

        var command = new UpdateIssuanceCommand
        {
            Id = dto.Id.Value,
            EmployeeId = dto.EmployeeId,
            IssuanceDate = dto.IssuanceDate,
            TotalAmount = dto.TotalAmount,
            IsClosed = !dto.IsClosed
        };

        await ApiHelper.ExecuteCallGuardedAsync(() => ApiClient.UpdateIssuanceEndpointAsync("1", dto.Id.Value, command), Snackbar!);
        await _table.ReloadServerData();
    }
}

public class IssuanceEditModel
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime IssuanceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsClosed { get; set; }
    public List<IssuanceItemDto> Items { get; set; } = new();
}
