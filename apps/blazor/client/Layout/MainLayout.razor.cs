using AMIS.Blazor.Infrastructure.Preferences;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Layout;

public partial class MainLayout
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
    [Parameter]
    public EventCallback<bool> OnDarkModeToggle { get; set; }
    [Parameter]
    public EventCallback<bool> OnRightToLeftToggle { get; set; }

    private bool _drawerOpen;
    private bool _isDarkMode;
    private bool _showDashboard;

    // Dashboard metrics
    private int _purchaseCount;
    private int _inspectionRequestCount;
    private int _inspectionsCount;
    private int _acceptancesCount;
    private int _suppliersCount;
    private string _lastUpdatedLocal = "—";

    [Inject]
    private IApiClient Api { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preferences)
        {
            _drawerOpen = preferences.IsDrawerOpen;
            _isDarkMode = preferences.IsDarkMode;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        UpdateShowDashboardFlag();
        if (_showDashboard)
        {
            await LoadDashboardAsync();
        }
    }

    public async Task ToggleDarkMode()
    {
        _isDarkMode = !_isDarkMode;
        await OnDarkModeToggle.InvokeAsync(_isDarkMode);
    }

    private async Task DrawerToggle()
    {
        _drawerOpen = await ClientPreferences.ToggleDrawerAsync();
    }
    private void Logout()
    {
        var parameters = new DialogParameters
        {
                { nameof(Components.Dialogs.Logout.ContentText), "Do you want to logout from the system?"},
                { nameof(Components.Dialogs.Logout.ButtonText), "Logout"},
                { nameof(Components.Dialogs.Logout.Color), Color.Error}
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        DialogService.Show<Components.Dialogs.Logout>("Logout", parameters, options);
    }

    private void Profile()
    {
        Navigation.NavigateTo("/identity/account");
    }

    private void UpdateShowDashboardFlag()
    {
        try
        {
            var uri = new Uri(Navigation.Uri);
            var path = uri.AbsolutePath.TrimEnd('/').ToLowerInvariant();
            _showDashboard = string.IsNullOrWhiteSpace(path) || path == string.Empty || path == "/".TrimEnd('/') || path == "/dashboard".TrimEnd('/');
        }
        catch
        {
            _showDashboard = false;
        }
    }

    private async Task LoadDashboardAsync()
    {
        try
        {
            // Use minimal page size to retrieve total counts
            var purchases = await Api.SearchPurchasesEndpointAsync("1", new SearchPurchasesCommand { PageNumber = 1, PageSize = 1 });
            _purchaseCount = purchases?.TotalCount ?? 0;

            var requests = await Api.SearchInspectionRequestsEndpointAsync("1", new SearchInspectionRequestsCommand { PageNumber = 1, PageSize = 1 });
            _inspectionRequestCount = requests?.TotalCount ?? 0;

            var inspections = await Api.SearchInspectionsEndpointAsync("1", new SearchInspectionsCommand { PageNumber = 1, PageSize = 1 });
            _inspectionsCount = inspections?.TotalCount ?? 0;

            var acceptances = await Api.SearchAcceptancesEndpointAsync("1", new SearchAcceptancesCommand { PageNumber = 1, PageSize = 1 });
            _acceptancesCount = acceptances?.TotalCount ?? 0;

            var suppliers = await Api.SearchSuppliersEndpointAsync("1", new SearchSuppliersCommand { PageNumber = 1, PageSize = 1 });
            _suppliersCount = suppliers?.TotalCount ?? 0;

            _lastUpdatedLocal = DateTime.Now.ToString("g");
        }
        catch (Exception ex)
        {
            // Show a non-blocking toast; avoid breaking the layout
            Toast.Add($"Failed to load dashboard metrics: {ex.Message}", Severity.Warning);
        }
        StateHasChanged();
    }
}
