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

    private async Task ShowEditFormDialog(string title, UpdatePurchaseCommand command, bool IsCreate)
    {
        var parameters = new DialogParameters
        {
            { nameof(PurchaseDialog.Model), command },
            { nameof(PurchaseDialog.IsCreate), IsCreate },
            { nameof(PurchaseDialog.Refresh), EventCallback.Factory.Create(this, OnRefresh) }

        };
        var options = new DialogOptions { 
            CloseButton = true, 
            MaxWidth = MaxWidth.Large, 
            FullWidth = true, 
            BackdropClick = false, 
            Position = DialogPosition.Center 
        };
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
        var model = CreateDefaultPurchaseCommand();

        await ShowEditFormDialog("Create new purchase", model, true);
    }

    private static UpdatePurchaseCommand CreateDefaultPurchaseCommand()
    {
        return new UpdatePurchaseCommand
        {
            PurchaseDate = DateTime.Today,
            SupplierId = null,
            Status = PurchaseStatus.Pending,
            Items = new List<PurchaseItemUpdateDto>()
        };
    }

    private async Task OnClone()
    {
        var copy = _selectedItems.First();
        if (copy != null)
        {
            var command = new Mapper().Map<PurchaseResponse, UpdatePurchaseCommand>(copy);
            //var command = copy.Adapt<PurchaseViewModel>();
            command.Id = Guid.NewGuid(); // Assign a new Id for the cloned item
            await ShowEditFormDialog("Clone a purchase", command, true);
        }
    }
    private async Task OnEdit(PurchaseResponse dto)
    {        
        var command = dto.Adapt<UpdatePurchaseCommand>();
        await ShowEditFormDialog("Edit the purchase", command, false);
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
    private async Task OnDeleteChecked()
    {
        var purchaseIds = _selectedItems
       .Select(item => item.Id)
       .Where(id => id.HasValue)
       .Select(id => id.Value)
       .ToList();

        if (purchaseIds.Count == 0)
        {
            Snackbar?.Add("No items selected for deletion.", Severity.Warning);
            return;
        }

        string deleteContent = "Are you sure you want to delete the selected purchases?";
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
                    () => purchaseclient.DeletePurchasesEndpointAsync("1", purchaseIds),
                    Snackbar);

                await _table.ReloadServerData();
                _selectedItems.Clear();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error deleting purchases: {ex.Message}", Severity.Error);
            }
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

}
