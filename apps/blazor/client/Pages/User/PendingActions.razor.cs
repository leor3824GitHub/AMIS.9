using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.User;

/// <summary>
/// Code-behind for PendingActions.razor
/// Displays pending issuances and asset acceptance workflows for end users
/// </summary>
public partial class PendingActions
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Properties would be bound from the Razor component
}
