using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Inventories;

public partial class PropertyCodeSequencesList
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    [Inject]
    protected ISnackbar? Snackbar { get; set; }

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    private List<PropertyCodeSequenceDto> Sequences = new();
    private List<PropertyCodeSequenceDto> FilteredSequences = new();
    private string SearchTerm = string.Empty;

    private bool ShowDialog = false;
    private bool ShowDeleteDialog = false;
    private bool IsLoading = false;
    private bool IsSaving = false;
    private bool IsDeleting = false;
    private bool IsCreateMode = true;
    private bool FormIsValid = false;

    private MudForm? EditForm;
    private PropertyCodeSequenceViewModel EditingItem = new();
    private PropertyCodeSequenceDto? DeleteItem = null;

    private bool CanCreate = false;
    private bool CanEdit = false;
    private bool CanDelete = false;
    private bool IsAdminRole = false;

    protected override async Task OnInitializedAsync()
    {
        await CheckPermissionsAsync();
        await LoadSequences();
    }

    private async Task CheckPermissionsAsync()
    {
        var state = await AuthState;
        var user = state.User;

        // Check if user is authenticated
        if (!user.Identity?.IsAuthenticated ?? false)
        {
            CanCreate = false;
            CanEdit = false;
            CanDelete = false;
            return;
        }

        IsAdminRole = user.IsInRole(FshRoles.Admin);

        if (IsAdminRole)
        {
            CanCreate = true;
            CanEdit = true;
            CanDelete = true;
            return;
        }

        CanCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.PropertyCodeSequences);
        CanEdit = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.PropertyCodeSequences);
        CanDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.PropertyCodeSequences);
    }

    private async Task LoadSequences()
    {
        IsLoading = true;
        try
        {
            var result = await ApiClient.PropertyCodeSequenceEndpointsListAsync("1", pageNumber: 1, pageSize: 1000, searchTerm: null);
            Sequences = result.Items?.ToList() ?? new();
            FilterSequences();
            Snackbar?.Add("Property code sequences loaded successfully.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading sequences: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void FilterSequences()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            FilteredSequences = Sequences;
        }
        else
        {
            var term = SearchTerm.ToLower();
            FilteredSequences = Sequences.Where(s =>
                s.Classification.ToLower().Contains(term) ||
                s.CategoryCode.ToLower().Contains(term) ||
                s.ClassCode.ToLower().Contains(term) ||
                s.ItemCode.ToLower().Contains(term) ||
                s.ItemDescription.ToLower().Contains(term) ||
                (s.GlAccount?.ToLower().Contains(term) ?? false)
            ).ToList();
        }
    }

    private async Task OnSearchTermChanged(string value)
    {
        SearchTerm = value;
        FilterSequences();
        await Task.CompletedTask;
    }

    private void ShowCreateDialog()
    {
        if (!CanCreate)
        {
            Snackbar?.Add("You don't have permission to create property code sequences.", Severity.Warning);
            return;
        }

        IsCreateMode = true;
        EditingItem = new PropertyCodeSequenceViewModel();
        FormIsValid = false;
        ShowDialog = true;
        StateHasChanged();
    }

    private void ShowEditDialog(PropertyCodeSequenceDto item)
    {
        if (!CanEdit)
        {
            Snackbar?.Add("You don't have permission to edit property code sequences.", Severity.Warning);
            return;
        }

        IsCreateMode = false;
        EditingItem = new PropertyCodeSequenceViewModel
        {
            Id = item.Id,
            Classification = item.Classification,
            Category = item.CategoryCode,
            ClassCode = item.ClassCode,
            ItemCode = item.ItemCode,
            ItemDescription = item.ItemDescription,
            GLAccount = item.GlAccount,
            LastSequenceValue = item.LastSequenceValue
        };
        FormIsValid = false;
        ShowDialog = true;
        StateHasChanged();
    }

    private void CloseDialog()
    {
        ShowDialog = false;
        EditingItem = new();
        StateHasChanged();
    }

    private async Task SaveItem()
    {
        if (IsSaving)
            return;

        if (EditForm != null)
        {
            await EditForm.Validate();
            if (!FormIsValid)
            {
                Snackbar?.Add("Please fix the errors in the form.", Severity.Warning);
                return;
            }
        }

        IsSaving = true;
        try
        {
            if (IsCreateMode)
            {
                var request = new CreatePropertyCodeSequenceRequest
                {
                    Classification = EditingItem.Classification,
                    Category = EditingItem.Category
                };
                await ApiClient.PropertyCodeSequenceEndpointsCreateAsync("1", request);
                Snackbar?.Add("Property code sequence created successfully.", Severity.Success);
            }
            else
            {
                var request = new UpdatePropertyCodeSequenceRequest
                {
                    Classification = EditingItem.Classification,
                    Category = EditingItem.Category,
                    LastSequenceValue = EditingItem.LastSequenceValue
                };
                await ApiClient.PropertyCodeSequenceEndpointsUpdateAsync("1", EditingItem.Id, request);
                Snackbar?.Add("Property code sequence updated successfully.", Severity.Success);
            }

            CloseDialog();
            await LoadSequences();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error saving sequence: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void ShowDeleteConfirm(PropertyCodeSequenceDto item)
    {
        if (!CanDelete)
        {
            Snackbar?.Add("You don't have permission to delete property code sequences.", Severity.Warning);
            return;
        }

        DeleteItem = item;
        ShowDeleteDialog = true;
        StateHasChanged();
    }

    private void CloseDeleteDialog()
    {
        ShowDeleteDialog = false;
        DeleteItem = null;
        StateHasChanged();
    }

    private async Task ConfirmDelete()
    {
        if (DeleteItem == null || IsDeleting)
            return;

        IsDeleting = true;
        try
        {
            await ApiClient.PropertyCodeSequenceEndpointsDeleteAsync("1", DeleteItem.Id);
            Snackbar?.Add("Property code sequence deleted successfully.", Severity.Success);
            CloseDeleteDialog();
            await LoadSequences();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error deleting sequence: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsDeleting = false;
        }
    }

    private IEnumerable<string> ValidateClassification(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            yield return "Classification is required";
    }

    public class PropertyCodeSequenceViewModel
    {
        public int Id { get; set; }
        public string Classification { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string? GLAccount { get; set; }
        public int LastSequenceValue { get; set; }
    }
}
