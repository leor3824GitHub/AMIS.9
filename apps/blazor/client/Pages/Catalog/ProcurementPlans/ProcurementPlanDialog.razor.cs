using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Catalog.ProcurementPlans;

public partial class ProcurementPlanDialog
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Parameter] public bool IsCreate { get; set; }
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public Guid PreparedByUserId { get; set; }
    [Parameter] public GetProcurementPlanResponse? ViewModel { get; set; }
    [Parameter] public bool OpenAddItemOnLoad { get; set; }

    [Inject] protected IApiClient Api { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected IDialogService Dialog { get; set; } = default!;

    [Inject] protected IAuthorizationService AuthService { get; set; } = default!;

    // Create form fields
    private string _controlNumber = string.Empty;
    private int _fiscalYear = DateTime.Now.Year;
    private string _departmentName = string.Empty;
    private BudgetType _budgetType = BudgetType._0;
    private bool _isSupplemental = false;

    // Edit form fields
    private string _editDepartmentName = string.Empty;
    private BudgetType _editBudgetType;
    private bool _editIsSupplemental;
    private List<ProcurementPlanItemResponse>? _editItems;

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
            _editDepartmentName = ViewModel.DepartmentName;
            _editBudgetType = ViewModel.BudgetType;
            _editIsSupplemental = ViewModel.IsSupplemental;
            _editItems = ViewModel.Items?.ToList() ?? new List<ProcurementPlanItemResponse>();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canUpdatePlan = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.ProcurementPlans);
        _canCreateItems = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.ProcurementPlanItems);
        _canUpdateItems = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.ProcurementPlanItems);
        _canDeleteItems = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.ProcurementPlanItems);
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

    private string GetDialogTitle()
    {
        if (IsCreate) return "Create New PPMP";
        if (ReadOnly) return $"PPMP: {ViewModel?.ControlNumber}";
        return $"Edit PPMP: {ViewModel?.ControlNumber}";
    }

    private bool CanCreate()
    {
        return _fiscalYear > 0 && !string.IsNullOrWhiteSpace(_departmentName);
    }

    private async Task Create()
    {
        try
        {
            var command = new CreateProcurementPlanCommand
            {
                ControlNumber = _controlNumber,
                FiscalYear = _fiscalYear,
                DepartmentName = _departmentName,
                BudgetType = _budgetType,
                IsSupplemental = _isSupplemental,
                PreparedByUserId = PreparedByUserId,
                DepartmentId = Guid.NewGuid()
            };

            var result = await Api.CreateProcurementPlanEndpointAsync("1", command);
            MudDialog.Close(DialogResult.Ok(result));
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Failed to create PPMP: {ex.Message}", Severity.Error);
        }
    }

    private async Task Update()
    {
        if (ViewModel == null) return;
        if (!_canUpdatePlan) return;

        try
        {
            var command = new UpdateProcurementPlanCommand
            {
                Id = ViewModel.Id,
                DepartmentName = _editDepartmentName,
                BudgetType = _editBudgetType,
                IsSupplemental = _editIsSupplemental,
                DepartmentId = ViewModel.DepartmentId
            };

            var result = await Api.UpdateProcurementPlanEndpointAsync("1", ViewModel.Id, command);
            MudDialog.Close(DialogResult.Ok(result));
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Failed to update PPMP: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnAddItem()
    {
        if (ViewModel == null) return;
        if (!_canCreateItems) return;

        var parameters = new DialogParameters
        {
            { nameof(ProcurementPlanItemDialog.PlanHeaderId), ViewModel.Id },
            { nameof(ProcurementPlanItemDialog.IsCreate), true }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await Dialog.ShowAsync<ProcurementPlanItemDialog>("Add Line Item", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Snackbar.Add("Item added successfully.", Severity.Success);

            // Reload the plan to get updated totals
            await RefreshPlan();
        }
    }

    private async Task OnEditItem(ProcurementPlanItemResponse item)
    {
        if (ViewModel == null) return;
        if (!_canUpdateItems) return;

        var parameters = new DialogParameters
        {
            { nameof(ProcurementPlanItemDialog.PlanHeaderId), ViewModel.Id },
            { nameof(ProcurementPlanItemDialog.IsCreate), false },
            { nameof(ProcurementPlanItemDialog.Item), item }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await Dialog.ShowAsync<ProcurementPlanItemDialog>("Edit Line Item", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            Snackbar.Add("Item updated successfully.", Severity.Success);
            await RefreshPlan();
        }
    }

    private async Task OnDeleteItem(ProcurementPlanItemResponse item)
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
                await Api.DeleteProcurementPlanItemAsync("1", ViewModel.Id, item.Id);
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
            var updated = await Api.GetProcurementPlanEndpointAsync("1", ViewModel.Id);
            ViewModel = updated;
            _editItems = updated.Items?.ToList() ?? new List<ProcurementPlanItemResponse>();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to refresh plan: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();

    // Status helpers
    private static string GetStatusLabel(ProcurementPlanStatus status) => status switch
    {
        ProcurementPlanStatus._0 => "None",
        ProcurementPlanStatus._1 => "Draft",
        ProcurementPlanStatus._2 => "Submitted",
        ProcurementPlanStatus._3 => "Approved",
        ProcurementPlanStatus._4 => "Rejected",
        ProcurementPlanStatus._5 => "Cancelled",
        _ => "Unknown"
    };

    private static Color GetStatusColor(ProcurementPlanStatus status) => status switch
    {
        ProcurementPlanStatus._0 => Color.Default,
        ProcurementPlanStatus._1 => Color.Default,
        ProcurementPlanStatus._2 => Color.Info,
        ProcurementPlanStatus._3 => Color.Success,
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

    private static string GetProjectTypeLabel(ProjectType type) => type switch
    {
        ProjectType._0 => "Goods",
        ProjectType._1 => "Services",
        ProjectType._2 => "Infrastructure",
        _ => "Unknown"
    };
}
