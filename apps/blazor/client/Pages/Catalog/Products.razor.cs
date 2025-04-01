using AMIS.Blazor.Client.Components.EntityTable;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog;

public partial class Products
{
    [Inject]
    protected IApiClient _client { get; set; } = default!;
    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    protected EntityServerTableContext<ProductResponse, Guid, ProductViewModel> Context { get; set; } = default!;

    private EntityTable<ProductResponse, Guid, ProductViewModel> _table = default!;

    private List<CategoryResponse> _categories = new();

    private bool _isUploading = false;
    private string _uploadErrorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Context = new(
            entityName: "Product",
            entityNamePlural: "Products",
            entityResource: FshResources.Products,
            fields: new()
            {
                new(prod => prod.Id,"Id", "Id"),
                new(prod => prod.Name,"Name", "Name"),
                new(prod => prod.Description, "Description", "Description"),
                new(prod => prod.Unit, "Unit", "Unit"),
                new(prod => prod.Sku, "Sku", "Sku"),
                new(prod => prod.ImagePath, "ImagePath", "ImagePath"),
                new(prod => prod.Category?.Name, "Category", "Category")
            },
            enableAdvancedSearch: true,
            idFunc: prod => prod.Id!.Value,
            searchFunc: async filter =>
            {
                var productFilter = filter.Adapt<SearchProductsCommand>();
                //productFilter.MinimumRate = Convert.ToDouble(SearchMinimumRate);
                //productFilter.MaximumRate = Convert.ToDouble(SearchMaximumRate);
                //productFilter.BrandId = SearchBrandId;
                var result = await _client.SearchProductsEndpointAsync("1", productFilter);
                return result.Adapt<PaginationResponse<ProductResponse>>();
            },
            createFunc: async prod =>
            {
                await _client.CreateProductEndpointAsync("1", prod.Adapt<CreateProductCommand>());
            },
            updateFunc: async (id, prod) =>
            {
                await _client.UpdateProductEndpointAsync("1", id, prod.Adapt<UpdateProductCommand>());
            },
            deleteFunc: async id => await _client.DeleteProductEndpointAsync("1", id));

        await LoadCategoriesAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        if (_categories.Count == 0)
        {
            var response = await _client.SearchCategorysEndpointAsync("1", new SearchCategorysCommand());
            if (response?.Items != null)
            {
                _categories = response.Items.ToList();
            }
        }
    }

    //        if (!AppConstants.SupportedImageFormats.Contains(extension.ToLower(System.Globalization.CultureInfo.CurrentCulture)))
    //        {
    //            Snackbar.Add("Image Format Not Supported.", Severity.Error);
    //            return;
    //        }

    //        Context.AddEditModal.RequestModel.ImageExtension = extension;
    //        var imageFile = await e.File.RequestImageFileAsync(AppConstants.StandardImageFormat, AppConstants.MaxImageWidth, AppConstants.MaxImageHeight);
    //        byte[]? buffer = new byte[imageFile.Size];
    //        await imageFile.OpenReadStream(AppConstants.MaxAllowedSize).ReadExactlyAsync(buffer);
    //        Context.AddEditModal.RequestModel.ImageInBytes = $"data:{AppConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";
    //        Context.AddEditModal.ForceRender();
    //    }
    //}
    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        _uploadErrorMessage = string.Empty;
       
        if (e.File == null)
        {
            Snackbar.Add("No e.File selected.", Severity.Error);
            return;
        }

        string? extension = Path.GetExtension(e.File.Name);

        // Check if the e.File has a supported image format
        if (!AppConstants.SupportedImageFormats.Contains(extension.ToLower()))
        {
            Snackbar.Add("Image format not supported.", Severity.Error);
            return;
        }

        // e.File size validation (5MB max)
        if (e.File.Size > AppConstants.MaxAllowedSize)
        {
            Snackbar.Add("e.File size exceeds the maximum allowed size.", Severity.Error);
            return;
        }

        Context.AddEditModal.RequestModel.ImageExtension = extension;

        try
        {
            // Show progress indicator while uploading
            _isUploading = true;
            string? fileName = $"{Context.AddEditModal.RequestModel.Name}-{Guid.NewGuid():N}";
            fileName = fileName[..Math.Min(fileName.Length, 90)];
            // Request the image e.File to be resized (if necessary) to fit within the specified max width and height
            var imageFile = await e.File.RequestImageFileAsync(AppConstants.StandardImageFormat, AppConstants.MaxImageWidth, AppConstants.MaxImageHeight);

            // Read the e.File's bytes into a buffer
            byte[] buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream(AppConstants.MaxAllowedSize).ReadAsync(buffer);
            string? base64String = $"data:{AppConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";
            // Convert the image bytes to a Base64 string
            Context.AddEditModal.RequestModel.Image = new FileUploadCommand() { Name = fileName, Data = base64String, Extension = extension };

            // Trigger a UI update
            Context.AddEditModal.ForceRender();
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the e.File upload process
            Snackbar.Add($"An error occurred while uploading the image: {ex.Message}", Severity.Error);
        }
        finally
        {
            // Hide the progress indicator once the upload is complete or fails
            _isUploading = false;
        }
    }

    public void ClearImageInBytes()
    {
        Context.AddEditModal.RequestModel.ImageInBytes = string.Empty;
        Context.AddEditModal.ForceRender();
    }

    public void SetDeleteCurrentImageFlag()
    {
        Context.AddEditModal.RequestModel.ImageInBytes = string.Empty;
        Context.AddEditModal.RequestModel.ImagePath = string.Empty;
        Context.AddEditModal.RequestModel.DeleteCurrentImage = true;
        Context.AddEditModal.ForceRender();
    }
}

public class ProductViewModel : UpdateProductCommand
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }
}
