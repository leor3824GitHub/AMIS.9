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

namespace AMIS.Blazor.Client.Pages.Catalog.Products;
public partial class Products
{
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

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Products);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Products);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Products);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Products);

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

        try
        {
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
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading data: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }

        return new GridData<ProductResponse> { TotalItems = _totalItems, Items = _entityList };
    }

    private async Task ShowEditFormDialog(string title, ProductViewModel command, bool IsCreate, List<CategoryResponse> categories)
    {
        var parameters = new DialogParameters
        {
            { nameof(ProductDialog.Model), command },
            { nameof(ProductDialog.IsCreate), IsCreate },
            { nameof(ProductDialog._categories), categories }
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
        var model = _currentDto.Adapt<ProductViewModel>();
        await ShowEditFormDialog("Create new Item", model, true, _categories);
    }

    private async Task OnClone()
    {
        var copy = _selectedItems.First();
        if (copy != null)
        {
            var command = new Mapper().Map<ProductResponse, ProductViewModel>(copy);
            //var command = copy.Adapt<ProductViewModel>();
            command.Id = Guid.NewGuid(); // Assign a new Id for the cloned item
            await ShowEditFormDialog("Clone an Item", command, true, _categories);
        }
    }

    private async Task OnEdit(ProductResponse dto)
    {
        
        var command = dto.Adapt<ProductViewModel>();
        await ShowEditFormDialog("Edit the Item", command, false, _categories);
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
        var productIds = _selectedItems
       .Select(item => item.Id)
       .Where(id => id.HasValue)
       .Select(id => id.Value)
       .ToList();

        if (productIds.Count == 0)
        {
            Snackbar?.Add("No items selected for deletion.", Severity.Warning);
            return;
        }

        string deleteContent = "Are you sure you want to delete the selected products?";
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
                    () => productclient.DeleteProductsEndpointAsync("1", productIds),
                    Snackbar);

                await _table.ReloadServerData();
                _selectedItems.Clear();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error deleting products: {ex.Message}", Severity.Error);
            }
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
public class ProductViewModel : UpdateProductCommand
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }
}
