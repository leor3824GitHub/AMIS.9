using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.InspectionRequests;
public partial class InspectionRequestDialog
{
    [Inject]
    private IApiClient InspectionRequestClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public UpdateInspectionRequestCommand Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<EmployeeResponse> _employees { get; set; }
    [Parameter] public List<PurchaseResponse> _purchases { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;

    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        Snackbar.Add(IsCreate.Value ? "Creating inspection request..." : "Updating inspection request...", Severity.Info);

        if (IsCreate.Value) // Create inspection request
        {
            var model = Model.Adapt<CreateInspectionRequestCommand>();

            // Fix for CS8629: Ensure AssignedInspectorId is not null before casting
            if (model.AssignedInspectorId.HasValue)
            {
                model.RequestedById = model.AssignedInspectorId.Value;
            }

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionRequestClient.CreateInspectionRequestEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Inspection request created successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
        else // Update product
        {
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionRequestClient.UpdateInspectionRequestEndpointAsync("1", Model.Id, Model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Inspection request updated successfully!";
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
        if (Model != null && Model.AssignedInspectorId == null && _employees.Count != 0)
        {
            Model.AssignedInspectorId = null;
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
