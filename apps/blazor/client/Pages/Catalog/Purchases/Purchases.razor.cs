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
using System.Reflection;

namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;
public partial class Purchases
{
    private MudDataGrid<PurchaseResponse> _table = default!;
    private HashSet<PurchaseResponse> _selectedItems = new();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient purchaseclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    private PurchaseResponse _currentDto = new();
    private List<SupplierResponse> _supplier = new List<SupplierResponse>();
    private List<ProductResponse> _product = new List<ProductResponse>();

    private string searchString = "";
    private bool _loading;
    private string successMessage = "";

    private IEnumerable<PurchaseResponse>? _entityList;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.Purchases);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Purchases);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Purchases);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Purchases);

        await LoadSupplierAsync();
        await LoadProductAsync();

    }
    private async Task LoadProductAsync()
    {
        if (_product.Count == 0)
        {
            var response = await purchaseclient.SearchProductsEndpointAsync("1", new SearchProductsCommand());
            if (response?.Items != null)
            {
                _product = response.Items.ToList();
            }
        }
    }

    private async Task LoadSupplierAsync()
    {
        if (_supplier.Count == 0)
        {
            var response = await purchaseclient.SearchSuppliersEndpointAsync("1", new SearchSuppliersCommand());
            if (response?.Items != null)
            {
                _supplier = response.Items.ToList();
            }
        }
    }

    private async Task<GridData<PurchaseResponse>> ServerReload(GridState<PurchaseResponse> state)
    {
        _loading = true;
        var purchaseFilter = new SearchPurchasesCommand
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
            var result = await purchaseclient.SearchPurchasesEndpointAsync("1", purchaseFilter);

            if (result != null)
            {
                _totalItems = result.TotalCount;
                _entityList = result.Items;
            }
            else
            {
                _totalItems = 0;
                _entityList = new List<PurchaseResponse>();
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

        return new GridData<PurchaseResponse> { TotalItems = _totalItems, Items = _entityList };
    }

    private async Task ShowEditFormDialog(string title, UpdatePurchaseCommand command, bool IsCreate, List<SupplierResponse> suppliers)
    {
        var parameters = new DialogParameters
        {
            { nameof(PurchaseDialog.Model), command },
            { nameof(PurchaseDialog.IsCreate), IsCreate },
            { nameof(PurchaseDialog._suppliers), suppliers }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<PurchaseDialog>(title, parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnCreate()
    {
        _currentDto.PurchaseDate = DateTime.Today;
        _currentDto.SupplierId = null;
        var model = _currentDto.Adapt<UpdatePurchaseCommand>();
        await ShowEditFormDialog("Create new Item", model, true, _supplier);
    }

    private async Task OnClone()
    {
        var copy = _selectedItems.First();
        if (copy != null)
        {
            var command = new Mapper().Map<PurchaseResponse, UpdatePurchaseCommand>(copy);
            //var command = copy.Adapt<PurchaseViewModel>();
            command.Id = Guid.NewGuid(); // Assign a new Id for the cloned item
            await ShowEditFormDialog("Clone an Item", command, true, _supplier);
        }
    }
    private async Task OnDetails(PurchaseResponse dto)
    {

    }
    private async Task OnEdit(PurchaseResponse dto)
    {
        
        var command = dto.Adapt<UpdatePurchaseCommand>();
        await ShowEditFormDialog("Edit the Item", command, false, _supplier);
    }

    private async Task OnDelete(PurchaseResponse dto)
    {
        var purchaseId = dto.Id;
        _ = purchaseId ?? throw new InvalidOperationException("IdFunc can't be null!");

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, "Purchase", dto.Id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled && purchaseId.HasValue)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => purchaseclient.DeletePurchaseEndpointAsync("1", purchaseId.Value),
                Toast);

            await _table.ReloadServerData();
        }
    }
   
    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
        _selectedItems = new HashSet<PurchaseResponse>();
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
