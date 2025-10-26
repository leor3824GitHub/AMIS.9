using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AMIS.Blazor.Client.Layout;

public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private bool _canViewRoles;
    private bool _canViewUsers;
    private bool _canViewProducts;
    private bool _canViewPurchases;
    private bool _canViewInspectionRequests;
    private bool _canViewInspections;
    private bool _canViewAcceptances;
    private bool _canViewIssuances;
    private bool _canViewInventories;
    private bool _canViewCategories;
    private bool _canViewSuppliers;
    private bool _canViewEmployees;
    private bool _canViewTenants;
    private bool _canViewAuditTrails;
    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;
        _canViewRoles = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Roles);
        _canViewUsers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Users);
        _canViewProducts = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Products);
        _canViewPurchases = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Purchases);
    _canViewInspectionRequests = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.InspectionRequests);
    _canViewInspections = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Inspections);
    _canViewAcceptances = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Acceptances);
    _canViewIssuances = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Issuances);
    _canViewInventories = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Inventories);
        _canViewCategories = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Categories);
        _canViewSuppliers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Suppliers);
    _canViewEmployees = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Employees);
        _canViewTenants = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Tenants);
        _canViewAuditTrails = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AuditTrails);
    }
}
