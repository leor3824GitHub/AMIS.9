using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Catalog.ProcurementPlans;

public partial class ProcurementPlanItemDialog
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Parameter] public Guid PlanHeaderId { get; set; }
    [Parameter] public bool IsCreate { get; set; }
    [Parameter] public ProcurementPlanItemResponse? Item { get; set; }

    [Inject] protected IApiClient Api { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected IAuthorizationService AuthService { get; set; } = default!;

    private string _papCode = string.Empty;
    private string _description = string.Empty;
    private ProjectType _projectType = ProjectType._0;
    private int _quantity = 1;
    private string _unitOfMeasure = string.Empty;
    private double _unitCost = 0;
    private string _mode = string.Empty;
    private bool _isEarlyProcurement = false;
    private string _scheduleMonth = "January";
    private string _fundingSource = string.Empty;
    private string _remarks = string.Empty;

    private bool _hasSavePermission;

    protected override void OnInitialized()
    {
        if (!IsCreate && Item != null)
        {
            _papCode = Item.PapCode ?? string.Empty;
            _description = Item.Description;
            _projectType = Item.ProjectType;
            _quantity = Item.Quantity;
            _unitOfMeasure = Item.UnitOfMeasure;
            _unitCost = Item.UnitCost;
            _mode = Item.Mode;
            _isEarlyProcurement = Item.IsEarlyProcurement;
            _scheduleMonth = Item.ScheduleMonth;
            _fundingSource = Item.FundingSource;
            _remarks = Item.Remarks ?? string.Empty;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _hasSavePermission = IsCreate
            ? await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.ProcurementPlanItems)
            : await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.ProcurementPlanItems);
    }

    private double CalculateEstimatedBudget()
    {
        return _quantity * _unitCost;
    }

    private bool CanSave()
    {
         return _hasSavePermission &&
             !string.IsNullOrWhiteSpace(_description) &&
               _quantity > 0 &&
               !string.IsNullOrWhiteSpace(_unitOfMeasure) &&
               _unitCost >= 0 &&
               !string.IsNullOrWhiteSpace(_mode) &&
               !string.IsNullOrWhiteSpace(_scheduleMonth) &&
               !string.IsNullOrWhiteSpace(_fundingSource);
    }

    private async Task Save()
    {
        if (!_hasSavePermission) return;

        try
        {
            if (IsCreate)
            {
                var command = new AddProcurementPlanItemCommand
                {
                    PlanId = PlanHeaderId,
                    PapCode = _papCode,
                    Description = _description,
                    ProjectType = _projectType,
                    Quantity = _quantity,
                    UnitOfMeasure = _unitOfMeasure,
                    UnitCost = _unitCost,
                    Mode = _mode,
                    IsEarlyProcurement = _isEarlyProcurement,
                    ScheduleMonth = _scheduleMonth,
                    FundingSource = _fundingSource,
                    Remarks = _remarks
                };

                await Api.AddProcurementPlanItemAsync("1", PlanHeaderId, command);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else if (Item != null)
            {
                var command = new UpdateProcurementPlanItemCommand
                {
                    PlanId = PlanHeaderId,
                    ItemId = Item.Id,
                    PapCode = _papCode,
                    Description = _description,
                    ProjectType = _projectType,
                    Quantity = _quantity,
                    UnitOfMeasure = _unitOfMeasure,
                    UnitCost = _unitCost,
                    Mode = _mode,
                    IsEarlyProcurement = _isEarlyProcurement,
                    ScheduleMonth = _scheduleMonth,
                    FundingSource = _fundingSource,
                    Remarks = _remarks
                };

                await Api.UpdateProcurementPlanItemAsync("1", PlanHeaderId, Item.Id, command);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Failed to save item: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();
}
