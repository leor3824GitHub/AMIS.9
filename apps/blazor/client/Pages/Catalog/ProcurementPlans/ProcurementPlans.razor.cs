using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;
using System.Security.Claims;

namespace AMIS.Blazor.Client.Pages.Catalog.ProcurementPlans;

public partial class ProcurementPlans
{
    private MudDataGrid<ProcurementPlanListItemResponse> _table = default!;

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
        _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.ProcurementPlans);
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.ProcurementPlans);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.ProcurementPlans);
        _canSubmit = await AuthService.HasPermissionAsync(user, FshActions.Submit, FshResources.ProcurementPlans);
        _canApprove = await AuthService.HasPermissionAsync(user, FshActions.Approve, FshResources.ProcurementPlans);
    }

    private async Task<GridData<ProcurementPlanListItemResponse>> ServerReload(GridState<ProcurementPlanListItemResponse> state)
    {
        _loading = true;
        try
        {
            var filter = new SearchProcurementPlansCommand
            {
                PageSize = state.PageSize,
                PageNumber = state.Page + 1,
                Keyword = _searchString
            };

            var result = await Api.SearchProcurementPlansEndpointAsync("1", filter);
            _totalItems = result.TotalCount;
            return new GridData<ProcurementPlanListItemResponse>
            {
                TotalItems = result.TotalCount,
                Items = result.Items ?? Enumerable.Empty<ProcurementPlanListItemResponse>()
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

        return new GridData<ProcurementPlanListItemResponse> { TotalItems = 0, Items = Enumerable.Empty<ProcurementPlanListItemResponse>() };
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
            { nameof(ProcurementPlanDialog.IsCreate), true },
            { nameof(ProcurementPlanDialog.PreparedByUserId), userId }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };
        var dialog = await Dialog.ShowAsync<ProcurementPlanDialog>("Create PPMP", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Snackbar.Add("Procurement Plan created successfully.", Severity.Success);
            await _table.ReloadServerData();
        }
    }

    private async Task OnView(ProcurementPlanListItemResponse item)
    {
        try
        {
            var detail = await Api.GetProcurementPlanEndpointAsync("1", item.Id);
            var parameters = new DialogParameters
            {
                { nameof(ProcurementPlanDialog.IsCreate), false },
                { nameof(ProcurementPlanDialog.ReadOnly), true },
                { nameof(ProcurementPlanDialog.ViewModel), detail }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };
            await Dialog.ShowAsync<ProcurementPlanDialog>($"View PPMP: {item.ControlNumber}", parameters, options);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading PPMP details: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnEdit(ProcurementPlanListItemResponse item)
    {
        try
        {
            var detail = await Api.GetProcurementPlanEndpointAsync("1", item.Id);
            var parameters = new DialogParameters
            {
                { nameof(ProcurementPlanDialog.IsCreate), false },
                { nameof(ProcurementPlanDialog.ReadOnly), false },
                { nameof(ProcurementPlanDialog.ViewModel), detail }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };
            var dialog = await Dialog.ShowAsync<ProcurementPlanDialog>($"Edit PPMP: {item.ControlNumber}", parameters, options);
            var result = await dialog.Result;

            if (result is { Canceled: false })
            {
                Snackbar.Add("Procurement Plan updated successfully.", Severity.Success);
                await _table.ReloadServerData();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading PPMP details: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnQuickAddItem(ProcurementPlanListItemResponse item)
    {
        if (!_canUpdate) return;

        try
        {
            var detail = await Api.GetProcurementPlanEndpointAsync("1", item.Id);
            var parameters = new DialogParameters
            {
                { nameof(ProcurementPlanDialog.IsCreate), false },
                { nameof(ProcurementPlanDialog.ReadOnly), false },
                { nameof(ProcurementPlanDialog.ViewModel), detail },
                { nameof(ProcurementPlanDialog.OpenAddItemOnLoad), true }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };
            var dialog = await Dialog.ShowAsync<ProcurementPlanDialog>($"Add Item: {item.ControlNumber}", parameters, options);
            var result = await dialog.Result;

            if (result is { Canceled: false })
            {
                await _table.ReloadServerData();
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error opening PPMP: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnSubmit(ProcurementPlanListItemResponse item)
    {
        var confirm = await Dialog.ShowMessageBox(
            "Confirm Submit",
            $"Are you sure you want to submit PPMP '{item.ControlNumber}' for approval?",
            yesText: "Submit", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Api.SubmitProcurementPlanEndpointAsync("1", item.Id);
                Snackbar.Add("PPMP submitted successfully.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error submitting PPMP: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnApprove(ProcurementPlanListItemResponse item)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            Snackbar.Add("Unable to determine current user.", Severity.Error);
            return;
        }

        var confirm = await Dialog.ShowMessageBox(
            "Confirm Approval",
            $"Are you sure you want to approve PPMP '{item.ControlNumber}'?",
            yesText: "Approve", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Api.ApproveProcurementPlanEndpointAsync("1", item.Id, new ApproveProcurementPlanBody
                {
                    ApprovedByUserId = userId
                });
                Snackbar.Add("PPMP approved successfully.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (ApiException ex)
            {
                Snackbar.Add($"Error approving PPMP: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnReject(ProcurementPlanListItemResponse item)
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
            await Api.RejectProcurementPlanEndpointAsync("1", item.Id, new RejectProcurementPlanBody
            {
                RejectedByUserId = userId,
                Reason = reason
            });
            Snackbar.Add("PPMP rejected.", Severity.Success);
            await _table.ReloadServerData();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error rejecting PPMP: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnCancel(ProcurementPlanListItemResponse item)
    {
        var confirm = await Dialog.ShowMessageBox(
            "Confirm Cancel",
            $"Are you sure you want to cancel PPMP '{item.ControlNumber}'?",
            yesText: "Cancel PPMP", cancelText: "Keep");

        if (confirm == true)
        {
            try
            {
                await Api.CancelProcurementPlanEndpointAsync("1", item.Id);
                Snackbar.Add("PPMP cancelled.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (ApiException ex)
            {
                Snackbar.Add($"Error cancelling PPMP: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task<string?> PromptAsync(string prompt)
    {
        var parameters = new DialogParameters { { nameof(TextPrompt.ContentText), prompt } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await Dialog.ShowAsync<TextPrompt>("Input", parameters, options);
        var result = await dialog.Result;
        if (result is { Canceled: false } && result.Data is string s)
            return s;
        return null;
    }

    private async Task OnRevertToDraft(ProcurementPlanListItemResponse item)
    {
        var confirm = await Dialog.ShowMessageBox(
            "Confirm Revert to Draft",
            $"Are you sure you want to revert PPMP '{item.ControlNumber}' back to Draft status?",
            yesText: "Revert", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Api.RevertProcurementPlanToDraftEndpointAsync("1", item.Id);
                Snackbar.Add("PPMP reverted to Draft.", Severity.Success);
                await _table.ReloadServerData();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error reverting PPMP: {ex.Message}", Severity.Error);
            }
        }
    }

    // Status helpers
    private static bool IsDraft(ProcurementPlanStatus status) => status == ProcurementPlanStatus._0;
    private static bool IsSubmitted(ProcurementPlanStatus status) => status == ProcurementPlanStatus._1;
    private static bool IsRejected(ProcurementPlanStatus status) => status == ProcurementPlanStatus._4;

    private static string GetStatusLabel(ProcurementPlanStatus status) => status switch
    {
        ProcurementPlanStatus._0 => "Draft",
        ProcurementPlanStatus._1 => "Submitted",
        ProcurementPlanStatus._2 => "Approved",
        ProcurementPlanStatus._3 => "Published",
        ProcurementPlanStatus._4 => "Rejected",
        ProcurementPlanStatus._5 => "Cancelled",
        _ => "Unknown"
    };

    private static Color GetStatusColor(ProcurementPlanStatus status) => status switch
    {
        ProcurementPlanStatus._0 => Color.Default,
        ProcurementPlanStatus._1 => Color.Info,
        ProcurementPlanStatus._2 => Color.Success,
        ProcurementPlanStatus._3 => Color.Primary,
        ProcurementPlanStatus._4 => Color.Error,
        ProcurementPlanStatus._5 => Color.Secondary,
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
        var user = AuthState.Result.User;
        var userId = user.GetUserId();
        return userId != null && Guid.TryParse(userId, out var id) ? id : Guid.Empty;
    }
}
