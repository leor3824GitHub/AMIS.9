﻿using AMIS.Blazor.Infrastructure.Auth;
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

    private bool _canViewHangfire;
    private bool _canViewDashboard;
    private bool _canViewRoles;
    private bool _canViewUsers;
    private bool _canViewProducts;
    private bool _canViewPurchases;
    private bool _canViewCategories;
    private bool _canViewSuppliers;
    private bool _canViewEmployees;
    private bool _canViewTodos;
    private bool _canViewTenants;
    private bool _canViewAuditTrails;
    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;
        _canViewHangfire = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Hangfire);
        _canViewDashboard = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Dashboard);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Roles);
        _canViewUsers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Users);
        _canViewProducts = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Products);
        _canViewPurchases = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Purchases);
        _canViewCategories = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Categories);
        _canViewSuppliers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Suppliers);
        _canViewEmployees = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Employees);
        _canViewTodos = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Todos);
        _canViewTenants = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Tenants);
        _canViewAuditTrails = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AuditTrails);
    }
}
