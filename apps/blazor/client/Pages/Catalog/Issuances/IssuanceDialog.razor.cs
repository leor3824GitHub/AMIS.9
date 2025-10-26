using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Issuances;

public partial class IssuanceDialog
{
    [Inject]
    private IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public IssuanceEditModel Model { get; set; } = default!;

    [Parameter]
    public bool? IsCreate { get; set; }

    [Parameter]
    public IReadOnlyList<EmployeeResponse> Employees { get; set; } = Array.Empty<EmployeeResponse>();

    private FshValidation? _customValidation;
    private Guid? SelectedEmployeeId;
    private DateTime _issuanceDate = DateTime.Today;

    protected override void OnParametersSet()
    {
        if (Model != null && Model.EmployeeId != Guid.Empty)
        {
            SelectedEmployeeId = Model.EmployeeId;
        }
        else if (Employees.Count > 0)
        {
            SelectedEmployeeId = Employees[0].Id;
        }
        else
        {
            SelectedEmployeeId = null;
        }

        if (Model != null)
        {
            _issuanceDate = Model.IssuanceDate;
        }
    }

    private async Task OnValidSubmit()
    {
        if (IsCreate == null)
        {
            return;
        }

        if (SelectedEmployeeId is null)
        {
            Snackbar.Add("Please select an employee.", Severity.Warning);
            return;
        }

        Model.EmployeeId = SelectedEmployeeId.Value;

        if (IsCreate.Value)
        {
            var command = new CreateIssuanceCommand
            {
                EmployeeId = Model.EmployeeId,
                IssuanceDate = Model.IssuanceDate,
                TotalAmount = (double)Model.TotalAmount
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.CreateIssuanceEndpointAsync("1", command),
                Snackbar,
                Navigation,
                _customValidation);

            if (response != null)
            {
                Snackbar.Add("Issuance created successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var command = new UpdateIssuanceCommand
            {
                Id = Model.Id,
                EmployeeId = Model.EmployeeId,
                IssuanceDate = Model.IssuanceDate,
                TotalAmount = (double)Model.TotalAmount,
                IsClosed = Model.IsClosed
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.UpdateIssuanceEndpointAsync("1", command.Id, command),
                Snackbar,
                Navigation,
                _customValidation);

            if (response != null)
            {
                Snackbar.Add("Issuance updated successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private void OnDateChanged(DateTime? value)
    {
        if (value.HasValue)
        {
            _issuanceDate = value.Value;
            Model.IssuanceDate = value.Value;
        }
    }
}
