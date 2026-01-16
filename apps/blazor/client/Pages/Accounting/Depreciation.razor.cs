using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Accounting;

/// <summary>
/// Code-behind for Depreciation.razor
/// Handles depreciation calculations, asset register management, and financial reporting
/// </summary>
public partial class Depreciation
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Properties would be bound from the Razor component
}
