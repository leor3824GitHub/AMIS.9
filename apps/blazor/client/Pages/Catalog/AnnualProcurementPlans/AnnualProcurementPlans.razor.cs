using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Catalog.AnnualProcurementPlans;

public partial class AnnualProcurementPlans
{
    private MudDataGrid<AnnualProcurementPlanListItemResponse> _table = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient Api { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;
    [Inject]
    private IDialogService Dialog { get; set; } = default!;

    private string _searchString = string.Empty;
    private bool _loading;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canSubmit;
    private bool _canApprove;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.AnnualProcurementPlans);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.AnnualProcurementPlans);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.AnnualProcurementPlans);
        _canSubmit = await AuthService.HasPermissionAsync(user, FshActions.Submit, FshResources.AnnualProcurementPlans);
        _canApprove = await AuthService.HasPermissionAsync(user, FshActions.Approve, FshResources.AnnualProcurementPlans);
    }

    private async Task<GridData<AnnualProcurementPlanListItemResponse>> ServerReload(GridState<AnnualProcurementPlanListItemResponse> state)
    {
        _loading = true;
        try
        {
            var filter = new SearchAnnualProcurementPlansCommand
            {
                PageSize = state.PageSize,
                PageNumber = state.Page + 1,
                Keyword = _searchString
            };

            var result = await Api.SearchAnnualProcurementPlansEndpointAsync("1", filter);
            _totalItems = result.TotalCount;
            return new GridData<AnnualProcurementPlanListItemResponse>
            {
                TotalItems = result.TotalCount,
                Items = result.Items ?? Enumerable.Empty<AnnualProcurementPlanListItemResponse>()
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }

        return new GridData<AnnualProcurementPlanListItemResponse> { TotalItems = 0, Items = Enumerable.Empty<AnnualProcurementPlanListItemResponse>() };
    }

    private async Task OnSearch(string value)
    {
        _searchString = value;
        await _table.ReloadServerData();
    }

    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
    }

    private async Task OnCreate()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            Snackbar.Add("Unable to determine current user.", Severity.Error);
            return;
        }

        var parameters = new DialogParameters
        {
            { nameof(AnnualProcurementPlanDialog.IsCreate), true },
            { nameof(AnnualProcurementPlanDialog.PreparedByUserId), userId }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await Dialog.ShowAsync<AnnualProcurementPlanDialog>("Create APP", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Snackbar.Add("Annual Procurement Plan created successfully.", Severity.Success);
            await _table.ReloadServerData();
        }
    }

    private async Task OnView(AnnualProcurementPlanListItemResponse item)
    {
        try
        {
            var detail = await Api.GetAnnualProcurementPlanEndpointAsync("1", item.Id);
            var parameters = new DialogParameters
            {
                { nameof(AnnualProcurementPlanDialog.ViewModel), detail },
                { nameof(AnnualProcurementPlanDialog.IsCreate), false },
                { nameof(AnnualProcurementPlanDialog.ReadOnly), true }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            await Dialog.ShowAsync<AnnualProcurementPlanDialog>("View APP", parameters, options);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading details: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnEdit(AnnualProcurementPlanListItemResponse item)
    {
        try
        {
            var detail = await Api.GetAnnualProcurementPlanEndpointAsync("1", item.Id);
            var parameters = new DialogParameters
            {
                { nameof(AnnualProcurementPlanDialog.ViewModel), detail },
                { nameof(AnnualProcurementPlanDialog.IsCreate), false },
                { nameof(AnnualProcurementPlanDialog.ReadOnly), false }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            var dialog = await Dialog.ShowAsync<AnnualProcurementPlanDialog>("Edit APP", parameters, options);
            var result = await dialog.Result;

            if (result is { Canceled: false })
            {
                Snackbar.Add("Annual Procurement Plan updated successfully.", Severity.Success);
                await _table.ReloadServerData();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading details: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnQuickAddItem(AnnualProcurementPlanListItemResponse item)
    {
        if (!_canUpdate) return;

        try
        {
            var detail = await Api.GetAnnualProcurementPlanEndpointAsync("1", item.Id);
            var parameters = new DialogParameters
            {
                { nameof(AnnualProcurementPlanDialog.ViewModel), detail },
                { nameof(AnnualProcurementPlanDialog.IsCreate), false },
                { nameof(AnnualProcurementPlanDialog.ReadOnly), false },
                { nameof(AnnualProcurementPlanDialog.OpenAddItemOnLoad), true }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };
            var dialog = await Dialog.ShowAsync<AnnualProcurementPlanDialog>($"Add Item: {item.ControlNumber}", parameters, options);
            var result = await dialog.Result;

            if (result is { Canceled: false })
            {
                await _table.ReloadServerData();
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error opening APP: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnSubmit(AnnualProcurementPlanListItemResponse item)
    {
        var confirm = await ConfirmAsync("Submit APP", "Are you sure you want to submit this APP for approval?");
        if (!confirm) return;

        try
        {
            await Api.SubmitAnnualProcurementPlanEndpointAsync("1", item.Id);
            Snackbar.Add("APP submitted for approval.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Submit failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnApprove(AnnualProcurementPlanListItemResponse item)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            Snackbar.Add("Unable to determine current user.", Severity.Error);
            return;
        }

        var confirm = await ConfirmAsync("Approve APP", "Are you sure you want to approve this APP?");
        if (!confirm) return;

        try
        {
            await Api.ApproveAnnualProcurementPlanEndpointAsync("1", item.Id, new ApproveAnnualProcurementPlanBody
            {
                ApprovedByUserId = userId
            });
            Snackbar.Add("APP approved successfully.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Approve failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnReject(AnnualProcurementPlanListItemResponse item)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            Snackbar.Add("Unable to determine current user.", Severity.Error);
            return;
        }

        var reason = await PromptAsync("Rejection reason (required):");
        if (string.IsNullOrWhiteSpace(reason))
        {
            Snackbar.Add("Rejection reason is required.", Severity.Warning);
            return;
        }

        try
        {
            await Api.RejectAnnualProcurementPlanEndpointAsync("1", item.Id, new RejectAnnualProcurementPlanBody
            {
                RejectedByUserId = userId,
                Reason = reason
            });
            Snackbar.Add("APP rejected.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Reject failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnCancel(AnnualProcurementPlanListItemResponse item)
    {
        var confirm = await ConfirmAsync("Cancel APP", "Are you sure you want to cancel this APP?");
        if (!confirm) return;

        try
        {
            await Api.CancelAnnualProcurementPlanEndpointAsync("1", item.Id);
            Snackbar.Add("APP cancelled.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Cancel failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnRevertToDraft(AnnualProcurementPlanListItemResponse item)
    {
        var confirm = await ConfirmAsync("Revert to Draft", "Are you sure you want to revert this rejected APP to draft?");
        if (!confirm) return;

        try
        {
            await Api.RevertAnnualProcurementPlanToDraftEndpointAsync("1", item.Id);
            Snackbar.Add("APP reverted to draft.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Revert failed: {ex.Message}", Severity.Error);
        }
    }

    // Status helpers
    private static bool IsDraft(AnnualProcurementPlanStatus status) => status == AnnualProcurementPlanStatus._0;
    private static bool IsSubmitted(AnnualProcurementPlanStatus status) => status == AnnualProcurementPlanStatus._1;
    private static bool IsApproved(AnnualProcurementPlanStatus status) => status == AnnualProcurementPlanStatus._2;
    private static bool IsRejected(AnnualProcurementPlanStatus status) => status == AnnualProcurementPlanStatus._4;
    private static bool IsCancelled(AnnualProcurementPlanStatus status) => status == AnnualProcurementPlanStatus._5;

    private static string GetStatusLabel(AnnualProcurementPlanStatus status) => status switch
    {
        AnnualProcurementPlanStatus._0 => "Draft",
        AnnualProcurementPlanStatus._1 => "Submitted",
        AnnualProcurementPlanStatus._2 => "Approved",
        AnnualProcurementPlanStatus._3 => "Published",
        AnnualProcurementPlanStatus._4 => "Rejected",
        AnnualProcurementPlanStatus._5 => "Cancelled",
        _ => "Unknown"
    };

    private static Color GetStatusColor(AnnualProcurementPlanStatus status) => status switch
    {
        AnnualProcurementPlanStatus._0 => Color.Default,
        AnnualProcurementPlanStatus._1 => Color.Info,
        AnnualProcurementPlanStatus._2 => Color.Success,
        AnnualProcurementPlanStatus._3 => Color.Primary,
        AnnualProcurementPlanStatus._4 => Color.Error,
        AnnualProcurementPlanStatus._5 => Color.Secondary,
        _ => Color.Default
    };

    private static string GetBudgetTypeLabel(BudgetType type) => type switch
    {
        BudgetType._0 => "MOOE",
        BudgetType._1 => "CO",
        BudgetType._2 => "Mixed",
        _ => "Unknown"
    };

    private static Color GetBudgetTypeColor(BudgetType type) => type switch
    {
        BudgetType._0 => Color.Info,
        BudgetType._1 => Color.Warning,
        BudgetType._2 => Color.Primary,
        _ => Color.Default
    };

    private Guid GetCurrentUserId()
    {
        try
        {
            var user = AuthState.GetAwaiter().GetResult().User;
            var idClaim = user?.Claims?.FirstOrDefault(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier", StringComparison.OrdinalIgnoreCase));
            if (idClaim != null && Guid.TryParse(idClaim.Value, out var id)) return id;
        }
        catch
        {
            // ignored
        }
        return Guid.Empty;
    }

    private async Task<string?> PromptAsync(string title)
    {
        var parameters = new DialogParameters { { nameof(TextPrompt.ContentText), title } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await Dialog.ShowAsync<TextPrompt>("Input", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false } && result.Data is string s)
            return s;
        return null;
    }

    private async Task<bool> ConfirmAsync(string title, string message)
    {
        var parameters = new DialogParameters
        {
            { nameof(ConfirmationDialog.ContentText), message }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
        var dialog = await Dialog.ShowAsync<ConfirmationDialog>(title, parameters, options);
        var result = await dialog.Result;
        return result is { Canceled: false };
    }
}
