using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.User;

/// <summary>
/// Code-behind for MyAssets.razor
/// Displays assets assigned to the current user with condition tracking and details view
/// </summary>
public partial class MyAssets
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Properties would be bound from the Razor component
}
