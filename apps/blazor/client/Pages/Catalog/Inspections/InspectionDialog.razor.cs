using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Inspections;
public partial class InspectionDialog
{
    [Inject]
    private IApiClient InspectionClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public UpdateInspectionCommand Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<InspectionRequestResponse> _inspectionrequests { get; set; }
    //[Parameter] public List<PurchaseResponse> _purchases { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;

    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        Snackbar.Add(IsCreate.Value ? "Creating inspection..." : "Updating inspection...", Severity.Info);

        if (IsCreate.Value) // Create inspection request
        {
            var model = Model.Adapt<CreateInspectionCommand>();

            // Fix for CS8629: Ensure AssignedInspectorId is not null before casting  

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.CreateInspectionEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Inspection created successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
        else // Update product
        {
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.UpdateInspectionEndpointAsync("1", Model.Id, Model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Inspection updated successfully!";
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
        if (Model != null && Model.InspectorId == null && _inspectionrequests.Count != 0)
        {
            Model.InspectorId = null;
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
