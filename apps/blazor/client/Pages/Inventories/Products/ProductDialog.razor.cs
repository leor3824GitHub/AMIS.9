using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Shared.Authorization;
using System.Linq;

namespace AMIS.Blazor.Client.Pages.Inventories.Products;
public partial class ProductDialog
{
    [Inject]
    private IApiClient ProductClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public ProductViewModel Model { get; set; } = new ProductViewModel();
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<CategoryResponse> _categories { get; set; } = new List<CategoryResponse>();
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;

    protected override async Task OnInitializedAsync()
    {
        // Ensure Model is never null
        Model ??= new ProductViewModel();
        await base.OnInitializedAsync();
    }

    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        Snackbar.Add(IsCreate.Value ? "Creating product..." : "Updating product...", Severity.Info);

        if (IsCreate.Value) // Create product
        {
            var model = Model.Adapt<CreateProductCommand>();
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ProductClient.CreateProductEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Product created successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
        else // Update product
        {
            var model = Model.Adapt<UpdateProductCommand>();
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ProductClient.UpdateProductEndpointAsync("1", model.Id, model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Product updated successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
    }
    //private void OnCategoryChanged(List<CategoryResponse> Category)
    //{
    //    _categories = Category;
    //}
    protected override async Task OnParametersSetAsync()
    {
        if (_categories.Count == 0)
        {
            await LoadCategoriesAsync();
        }
        
        if (Model != null && Model.CategoryId == null && _categories.Count != 0)
        {
            Model.CategoryId = null;
        }
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            var searchCommand = new SearchCategorysCommand
            {
                PageNumber = 1,
                PageSize = 100
            };
            var response = await ProductClient.SearchCategoriesEndpointAsync("1", searchCommand);
            if (response?.Items != null)
            {
                _categories = response.Items.ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading categories: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

