using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Issuances;

/// <summary>
/// Code-behind for IssuanceWorkflow.razor
/// Handles the issuance workflow including requisition selection, item allocation,
/// and generation of ICS/PAR/RSMI documents
/// </summary>
public partial class IssuanceWorkflow
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Properties would be bound from the Razor component
}

