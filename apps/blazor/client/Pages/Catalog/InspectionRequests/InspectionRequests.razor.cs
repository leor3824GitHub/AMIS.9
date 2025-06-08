using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

private MudDataGrid<InspectionRequestResponse> _table = default!;
private HashSet<InspectionRequestResponse> _selectedItems = new();
private InspectionRequestResponse _currentDto = new();

private IEnumerable<InspectionRequestResponse>? _entityList;
private int _totalItems;
private bool _loading;
private string searchString = "";

[Inject] private IAuthorizationService AuthService { get; set; } = default!;
[Inject] private ISnackbar Snackbar { get; set; } = default!;
[Inject] private IApiClient ApiClient { get; set; } = default!;
[CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;

private bool _canSearch, _canCreate, _canDelete, _canUpdate;

protected override async Task OnInitializedAsync()
{
    var user = (await AuthState).User;
    _canSearch = await AuthService.HasPermissionAsync(user, FshActions.Search, FshResources.InspectionRequests);
    _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.InspectionRequests);
    _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.InspectionRequests);
    _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.InspectionRequests);
}


