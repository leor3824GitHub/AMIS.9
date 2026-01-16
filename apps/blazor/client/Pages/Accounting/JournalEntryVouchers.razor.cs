using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Accounting;

/// <summary>
/// Code-behind for JournalEntryVouchers.razor
/// Handles journal entry voucher CRUD operations, approval workflow, and financial posting
/// </summary>
public partial class JournalEntryVouchers
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Properties would be bound from the Razor component
}
