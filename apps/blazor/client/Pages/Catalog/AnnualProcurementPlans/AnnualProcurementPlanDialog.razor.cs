using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Catalog.AnnualProcurementPlans;

public partial class AnnualProcurementPlanDialog
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Parameter] public bool IsCreate { get; set; }
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public Guid PreparedByUserId { get; set; }
    [Parameter] public GetAnnualProcurementPlanResponse? ViewModel { get; set; }
    [Parameter] public bool OpenAddItemOnLoad { get; set; }

    [Inject] protected IApiClient Api { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected IDialogService Dialog { get; set; } = default!;

    [Inject] protected IAuthorizationService AuthService { get; set; } = default!;

    // Create form fields
    private string _controlNumber = string.Empty;
    private int _fiscalYear = DateTime.Now.Year;
    private BudgetType _budgetType = BudgetType._0;

    // Edit form fields
    private int _editFiscalYear;
    private BudgetType _editBudgetType;

    private List<AnnualProcurementPlanItemResponse>? _editItems;
    private bool _openedAddItem;

    private bool _canUpdatePlan;
    private bool _canCreateItems;
    private bool _canUpdateItems;
    private bool _canDeleteItems;

    protected override void OnInitialized()
    {
        // Initialize edit fields if editing
        if (!IsCreate && ViewModel != null)
        {
            _editFiscalYear = ViewModel.FiscalYear;
            _editBudgetType = ViewModel.BudgetType;
            _editItems = ViewModel.Items?.ToList() ?? new List<AnnualProcurementPlanItemResponse>();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canUpdatePlan = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.AnnualProcurementPlans);
        _canCreateItems = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.AnnualProcurementPlanItems);
        _canUpdateItems = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.AnnualProcurementPlanItems);
        _canDeleteItems = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.AnnualProcurementPlanItems);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        if (_openedAddItem) return;
        if (IsCreate) return;
        if (ReadOnly) return;
        if (!OpenAddItemOnLoad) return;
        if (ViewModel == null) return;
        if (!_canCreateItems) return;

        _openedAddItem = true;
        await OnAddItem();
    }

    private async Task OnAddItem()
    {
        if (ViewModel == null) return;
        if (!_canCreateItems) return;

        var parameters = new DialogParameters
        {
            { nameof(AnnualProcurementPlanItemDialog.PlanHeaderId), ViewModel.Id },
            { nameof(AnnualProcurementPlanItemDialog.IsCreate), true }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await Dialog.ShowAsync<AnnualProcurementPlanItemDialog>("Add Line Item", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Snackbar.Add("Item added successfully.", Severity.Success);
            await RefreshPlan();
        }
    }

    private async Task OnEditItem(AnnualProcurementPlanItemResponse item)
    {
        if (ViewModel == null) return;
        if (!_canUpdateItems) return;

        var parameters = new DialogParameters
        {
            { nameof(AnnualProcurementPlanItemDialog.PlanHeaderId), ViewModel.Id },
            { nameof(AnnualProcurementPlanItemDialog.IsCreate), false },
            { nameof(AnnualProcurementPlanItemDialog.Item), item }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await Dialog.ShowAsync<AnnualProcurementPlanItemDialog>("Edit Line Item", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Snackbar.Add("Item updated successfully.", Severity.Success);
            await RefreshPlan();
        }
    }

    private async Task OnDeleteItem(AnnualProcurementPlanItemResponse item)
    {
        if (ViewModel == null) return;
        if (!_canDeleteItems) return;

        var confirm = await Dialog.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete this item: '{item.Description}'?",
            yesText: "Delete", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Api.DeleteAnnualProcurementPlanItemAsync("1", ViewModel.Id, item.Id);
                Snackbar.Add("Item deleted successfully.", Severity.Success);
                await RefreshPlan();
            }
            catch (ApiException ex)
            {
                Snackbar.Add($"Failed to delete item: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task RefreshPlan()
    {
        if (ViewModel == null) return;

        try
        {
            var updated = await Api.GetAnnualProcurementPlanEndpointAsync("1", ViewModel.Id);
            ViewModel = updated;
            _editItems = updated.Items?.ToList() ?? new List<AnnualProcurementPlanItemResponse>();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to refresh plan: {ex.Message}", Severity.Error);
        }
    }

    private string GetDialogTitle()
    {
        if (IsCreate) return "Create New APP";
        if (ReadOnly) return $"APP: {ViewModel?.ControlNumber}";
        return $"Edit APP: {ViewModel?.ControlNumber}";
    }

    private bool CanCreate()
    {
        return _fiscalYear > 0;
    }

    private async Task Create()
    {
        try
        {
            var command = new CreateAnnualProcurementPlanCommand
            {
                ControlNumber = _controlNumber,
                FiscalYear = _fiscalYear,
                BudgetType = _budgetType,
                PreparedByUserId = PreparedByUserId
            };

            var result = await Api.CreateAnnualProcurementPlanEndpointAsync("1", command);
            MudDialog.Close(DialogResult.Ok(result));
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Failed to create APP: {ex.Message}", Severity.Error);
        }
    }

    private async Task Update()
    {
        if (ViewModel == null) return;
        if (!_canUpdatePlan) return;

        try
        {
            var command = new UpdateAnnualProcurementPlanCommand
            {
                Id = ViewModel.Id,
                FiscalYear = _editFiscalYear,
                BudgetType = _editBudgetType
            };

            var result = await Api.UpdateAnnualProcurementPlanEndpointAsync("1", ViewModel.Id, command);
            MudDialog.Close(DialogResult.Ok(result));
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Failed to update APP: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();

    // Status helpers
    private static string GetStatusLabel(AnnualProcurementPlanStatus status) => status switch
    {
        AnnualProcurementPlanStatus._0 => "None",
        AnnualProcurementPlanStatus._1 => "Draft",
        AnnualProcurementPlanStatus._2 => "Submitted",
        AnnualProcurementPlanStatus._3 => "Approved",
        AnnualProcurementPlanStatus._4 => "Rejected",
        AnnualProcurementPlanStatus._5 => "Cancelled",
        _ => "Unknown"
    };

    private static Color GetStatusColor(AnnualProcurementPlanStatus status) => status switch
    {
        AnnualProcurementPlanStatus._0 => Color.Default,
        AnnualProcurementPlanStatus._1 => Color.Default,
        AnnualProcurementPlanStatus._2 => Color.Info,
        AnnualProcurementPlanStatus._3 => Color.Success,
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

    private static string GetProjectTypeLabel(ProjectType type) => type switch
    {
        ProjectType._0 => "Goods",
        ProjectType._1 => "Services",
        ProjectType._2 => "Infrastructure",
        _ => "Unknown"
    };
}
