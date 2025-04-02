using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using AMIS.Blazor.Infrastructure.Auth;
using MapsterMapper;
using Mapster;

namespace AMIS.Blazor.Client.Pages.Catalog.Product2;
public partial class Productstable
{
    private List<BrandResponse> _brands = new();
    private MudDataGrid<ProductResponse> _table = default!;
    private HashSet<ProductResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject]
    protected IApiClient productclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    private ProductResponse _currentDto = new();
    private List<CategoryResponse> _categories = new List<CategoryResponse>();

    private string searchString = "";
    private bool _loading;
    private string successMessage = "";

    private IEnumerable<ProductResponse>? _entityList;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    //private bool _canExport;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Products);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Products);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Products);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Products);
        //_canExport = await AuthService.HasPermissionAsync(user, FshActions.Export, FshResources.Products);

        await LoadCategoriesAsync();
    }
    private async Task LoadCategoriesAsync()
    {
        if (_categories.Count == 0)
        {
            var response = await productclient.SearchCategorysEndpointAsync("1", new SearchCategorysCommand());
            if (response?.Items != null)
            {
                _categories = response.Items.ToList();
            }
        }
    }

    private async Task<GridData<ProductResponse>> ServerReload(GridState<ProductResponse> state)
    {
        _loading = true;
        var productFilter = new SearchProductsCommand
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            AdvancedSearch = new()
            {
                Fields = new[] { "name" },
                Keyword = searchString
            }
        };

        var result = await productclient.SearchProductsEndpointAsync("1", productFilter);

        if (result != null)
        {
            _totalItems = result.TotalCount;
            _entityList = result.Items;
        }
        else
        {
            _totalItems = 0;
            _entityList = new List<ProductResponse>();
        }

        _loading = false;
        return new GridData<ProductResponse> { TotalItems = _totalItems, Items = _entityList };
    }
    private async Task ShowEditFormDialog(string title, UpdateProductCommand command, bool IsCreate)
    {
        var parameters = new DialogParameters
        {
            { nameof(ProductDialog.Model), command },
            { nameof(ProductDialog.IsCreate), IsCreate }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ProductDialog>(title, parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnCreate()
    {
        var model = new UpdateProductCommand();

        await ShowEditFormDialog("Create new Item", model, true);
    }
    private async Task OnClone()
    {
        var copy = _selectedItems.First();
        if (copy != null)
        {
            var command = new Mapper().Map<ProductResponse, UpdateProductCommand>(copy);
            command.Id = Guid.NewGuid(); // Assign a new Id for the cloned item
            await ShowEditFormDialog("Clone an Item", command, true);
        }
    }
    private async Task OnEdit(ProductResponse dto)
    {
        var command = dto.Adapt<UpdateProductCommand>();
        await ShowEditFormDialog("Edit the Item", command, false);
    }
    private async Task OnDelete(ProductResponse dto)
    {
        var productId = dto.Id;
        _ = productId ?? throw new InvalidOperationException("IdFunc can't be null!");

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, "Product", dto.Id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled && productId.HasValue)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => productclient.DeleteProductEndpointAsync("1", productId.Value),
                Toast);

            await _table.ReloadServerData();
        }
    }
    private async Task OnDeleteChecked()
    {
        var productId = _selectedItems.First().Id;
        _ = productId ?? throw new InvalidOperationException("IdFunc can't be null!");

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, "Product", _selectedItems.First().Id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled && productId.HasValue)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => productclient.DeleteProductEndpointAsync("1", productId.Value),
                Toast);

            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }
    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
        _selectedItems = new HashSet<ProductResponse>();
    }

    private Task OnSearch(string text)
    {
        searchString = text;
        return _table.ReloadServerData();
    }

    private async Task LoadBrandsAsync()
    {
        if (_brands.Count == 0)
        {
            var response = await productclient.SearchBrandsEndpointAsync("1", new SearchBrandsCommand());
            if (response?.Items != null)
            {
                _brands = response.Items.ToList();
            }
        }
    }

    // Advanced Search

    private Guid? _searchBrandId;
    private Guid? SearchBrandId
    {
        get => _searchBrandId;
        set
        {
            _searchBrandId = value;
            _ = _table.ReloadServerData();
        }
    }

    private decimal _searchMinimumRate;
    private decimal SearchMinimumRate
    {
        get => _searchMinimumRate;
        set
        {
            _searchMinimumRate = value;
            _ = _table.ReloadServerData();
        }
    }

    private decimal _searchMaximumRate = 9999;
    private decimal SearchMaximumRate
    {
        get => _searchMaximumRate;
        set
        {
            _searchMaximumRate = value;
            _ = _table.ReloadServerData();
        }
    }
}
