using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Mapster;

namespace AMIS.Blazor.Client.Pages.Catalog.Product2;
public partial class Product3
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject]
    protected IApiClient productclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private List<ProductResponse> _entityList = new();
    private HashSet<ProductResponse> _selectedItems = new();
    private string searchString = "";
    private bool _loading = false;

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

        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        _loading = true;
        var productFilter = new SearchProductsCommand
        {
            Keyword = searchString,
            PageSize = 10,
            PageNumber = 1
        };

        var result = await productclient.SearchProductsEndpointAsync("1", productFilter);
        _entityList = (List<ProductResponse>)(result?.Items ?? new List<ProductResponse>());
        _loading = false;
    }

    private Task OnSearch(string text)
    {
        searchString = text;
        return LoadProductsAsync();
    }

    private async Task OnCreate()
    {
        var command = new UpdateProductCommand();
        await ShowEditFormDialog("Create New Product", command, true);
    }

    private async Task OnEdit(ProductResponse product)
    {
        var command = product.Adapt<UpdateProductCommand>();
        await ShowEditFormDialog("Edit Product", command, false);
    }

    private async Task ShowEditFormDialog(string title, UpdateProductCommand command, bool isCreate)
    {
        var parameters = new DialogParameters
    {
        { nameof(ProductDialog.Model), command },
        { nameof(ProductDialog.IsCreate), isCreate }
    };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ProductDialog>(title, parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await LoadProductsAsync();
            _selectedItems.Clear();
        }
    }

    private async Task OnDelete(ProductResponse product)
    {
        //var confirm = await ConfirmDelete(product);
        //if (confirm)
        //{
        //    await productclient.DeleteProductEndpointAsync("1", product.Id);
        //    await LoadProductsAsync();
        //}
    }

    //private async Task<bool> ConfirmDelete(ProductResponse product)
    //{
    //    var parameters = new DialogParameters
    //{
    //    { "ContentText", $"Are you sure you want to delete product {product.Name}?" }
    //};
    //    var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
    //    var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete Confirmation", parameters, options);
    //    var result = await dialog.Result;
    //    return !result.Canceled;
    //}

    private async Task OnDeleteChecked()
    {
        foreach (var product in _selectedItems)
        {
            await OnDelete(product);
        }
    }

    private async Task OnRefresh()
    {
        await LoadProductsAsync();
    }

    private void OnAddToCart(ProductResponse product)
    {
        Snackbar?.Add($"{product.Name} added to cart!", Severity.Success);
    }
}
