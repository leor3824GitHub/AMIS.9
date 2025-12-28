using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.AnnualProcurementPlans;

public partial class AnnualProcurementPlanDialog
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public bool IsCreate { get; set; }
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public Guid PreparedByUserId { get; set; }
    [Parameter] public GetAnnualProcurementPlanResponse? ViewModel { get; set; }

    [Inject] protected IApiClient Api { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected IDialogService Dialog { get; set; } = default!;

    // Create form fields
    private string _controlNumber = string.Empty;
    private int _fiscalYear = DateTime.Now.Year;
    private BudgetType _budgetType = BudgetType._0;

    // Edit form fields
    private int _editFiscalYear;
    private BudgetType _editBudgetType;

    protected override void OnInitialized()
    {
        // Initialize edit fields if editing
        if (!IsCreate && ViewModel != null)
        {
            _editFiscalYear = ViewModel.FiscalYear;
            _editBudgetType = ViewModel.BudgetType;
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

    private static string GetProjectTypeLabel(ProjectType type) => type switch
    {
        ProjectType._0 => "Goods",
        ProjectType._1 => "Services",
        ProjectType._2 => "Infrastructure",
        _ => "Unknown"
    };
}
