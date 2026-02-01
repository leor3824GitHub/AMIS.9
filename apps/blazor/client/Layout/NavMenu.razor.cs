using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Authorization;

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
    private bool _canViewPurchaseRequests;
    private bool _canViewCanvasses;
    private bool _canViewProcurementPlans;
    private bool _canViewAnnualProcurementPlans;
    private bool _canViewInspectionRequests;
    private bool _canViewInspections;
    private bool _canViewAcceptances;
    private bool _canViewIssuances;
    private bool _canViewInventories;
    private bool _canViewSmir;
    private bool _canViewSmrr;
    private bool _canViewPpeIssuanceReports;
    private bool _canViewPpeReceivingReports;
    private bool _canViewPropertyAcknowledgementReceipt;
    private bool _canViewCategories;
    private bool _canViewSuppliers;
    private bool _canViewEmployees;
    private bool _canViewTenants;
    private bool _canViewAuditTrails;
    private bool _canViewDepreciationSchedules;
    private bool _canViewJournalEntryVouchers;
    private bool _canViewAssetRequisitions;
    private bool _canViewPhysicalAssets;
    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;
    private bool CanViewAccountingGroup => _canViewDepreciationSchedules || _canViewJournalEntryVouchers;
    private bool CanViewMyAccountabilityGroup => _canViewAssetRequisitions || _canViewPhysicalAssets;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;
        _canViewRoles = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Roles);
        _canViewUsers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Users);
        _canViewProducts = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Products);
        _canViewPurchases = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Purchases);
        _canViewPurchaseRequests = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PurchaseRequests);
        _canViewCanvasses = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Canvasses);
        _canViewProcurementPlans = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.ProcurementPlans);
        _canViewAnnualProcurementPlans = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AnnualProcurementPlans);
        _canViewInspectionRequests = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.InspectionRequests);
        _canViewInspections = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Inspections);
        _canViewAcceptances = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Acceptances);
        _canViewIssuances = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Issuances);
        _canViewInventories = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Inventories);
        _canViewSmir = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.SuppliesAndMaterialsIssuance);
        _canViewSmrr = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.SuppliesAndMaterialsReceiving);
        _canViewPpeIssuanceReports = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PpeIssuance);
        _canViewPpeReceivingReports = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PpeReceiving);
        _canViewPropertyAcknowledgementReceipt = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PropertyAcknowledgementReceipt);
        _canViewCategories = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Categories);
        _canViewSuppliers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Suppliers);
        _canViewEmployees = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Employees);
        _canViewTenants = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Tenants);
        _canViewAuditTrails = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AuditTrails);
        _canViewDepreciationSchedules = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.DepreciationSchedules);
        _canViewJournalEntryVouchers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.JournalEntryVouchers);
        _canViewAssetRequisitions = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AssetRequisitions);
        _canViewPhysicalAssets = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.PhysicalAssets);
    }
}
