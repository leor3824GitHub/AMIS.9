using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Products;
public partial class ProductDialog
{
    [Inject]
    private IApiClient ProductClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public ProductViewModel Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<CategoryResponse> _categories { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;
    private bool _uploading;
    private string? _uploadErrorMessage;
    private bool _isUploading;

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
        if (Model != null && Model.CategoryId == null && _categories.Count != 0)
        {
            Model.CategoryId = _categories.FirstOrDefault()?.Id;
        }
    }
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

        Model.ImageExtension = extension;

        try
        {
            // Show progress indicator while uploading
            _isUploading = true;
            string? fileName = $"{Model.Name}-{Guid.NewGuid():N}";
            fileName = fileName[..Math.Min(fileName.Length, 90)];
            // Request the image e.File to be resized (if necessary) to fit within the specified max width and height
            var imageFile = await e.File.RequestImageFileAsync(AppConstants.StandardImageFormat, AppConstants.MaxImageWidth, AppConstants.MaxImageHeight);

            // Read the e.File's bytes into a buffer
            byte[] buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream(AppConstants.MaxAllowedSize).ReadExactlyAsync(buffer);
            string? base64String = $"data:{AppConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";
            // Convert the image bytes to a Base64 string
            Model.Image = new FileUploadCommand() { Name = fileName, Data = base64String, Extension = extension };

            // Trigger a UI update            
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
    public void SetDeleteCurrentImageFlag()
    {
        Model.Image = null;
        Model.ImageInBytes = string.Empty;
        Model.ImagePath = string.Empty;
        Model.DeleteCurrentImage = true;
    }


    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
