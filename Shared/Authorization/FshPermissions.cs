﻿using System.Collections.ObjectModel;

namespace AMIS.Shared.Authorization;

public static class FshPermissions
{
    private static readonly FshPermission[] AllPermissions =
    [     
        //tenants
        new("View Tenants", FshActions.View, FshResources.Tenants, IsRoot: true),
        new("Create Tenants", FshActions.Create, FshResources.Tenants, IsRoot: true),
        new("Update Tenants", FshActions.Update, FshResources.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FshActions.UpgradeSubscription, FshResources.Tenants, IsRoot: true),

        //identity
        new("View Users", FshActions.View, FshResources.Users),
        new("Search Users", FshActions.Search, FshResources.Users),
        new("Create Users", FshActions.Create, FshResources.Users),
        new("Update Users", FshActions.Update, FshResources.Users),
        new("Delete Users", FshActions.Delete, FshResources.Users),
        new("Export Users", FshActions.Export, FshResources.Users),
        new("View UserRoles", FshActions.View, FshResources.UserRoles),
        new("Update UserRoles", FshActions.Update, FshResources.UserRoles),
        new("View Roles", FshActions.View, FshResources.Roles),
        new("Create Roles", FshActions.Create, FshResources.Roles),
        new("Update Roles", FshActions.Update, FshResources.Roles),
        new("Delete Roles", FshActions.Delete, FshResources.Roles),
        new("View RoleClaims", FshActions.View, FshResources.RoleClaims),
        new("Update RoleClaims", FshActions.Update, FshResources.RoleClaims),
        
        //products
        new("View Products", FshActions.View, FshResources.Products, IsBasic: true),
        new("Search Products", FshActions.Search, FshResources.Products, IsBasic: true),
        new("Create Products", FshActions.Create, FshResources.Products),
        new("Update Products", FshActions.Update, FshResources.Products),
        new("Delete Products", FshActions.Delete, FshResources.Products),
        new("Export Products", FshActions.Export, FshResources.Products),

        //brands
        new("View Brands", FshActions.View, FshResources.Brands, IsBasic: true),
        new("Search Brands", FshActions.Search, FshResources.Brands, IsBasic: true),
        new("Create Brands", FshActions.Create, FshResources.Brands),
        new("Update Brands", FshActions.Update, FshResources.Brands),
        new("Delete Brands", FshActions.Delete, FshResources.Brands),
        new("Export Brands", FshActions.Export, FshResources.Brands),

        //categories
        new("View Categories", FshActions.View, FshResources.Categories, IsBasic: true),
        new("Search Categories", FshActions.Search, FshResources.Categories, IsBasic: true),
        new("Create Categories", FshActions.Create, FshResources.Categories),
        new("Update Categories", FshActions.Update, FshResources.Categories),
        new("Delete Categories", FshActions.Delete, FshResources.Categories),
        new("Export Categories", FshActions.Export, FshResources.Categories),

        //inventories
        new("View Inventories", FshActions.View, FshResources.Inventories, IsBasic: true),
        new("Search Inventories", FshActions.Search, FshResources.Inventories, IsBasic: true),
        new("Create Inventories", FshActions.Create, FshResources.Inventories),
        new("Update Inventories", FshActions.Update, FshResources.Inventories),
        new("Delete Inventories", FshActions.Delete, FshResources.Inventories),
        new("Export Inventories", FshActions.Export, FshResources.Inventories),

         //suppliers
        new("View Suppliers", FshActions.View, FshResources.Suppliers, IsBasic: true),
        new("Search Suppliers", FshActions.Search, FshResources.Suppliers, IsBasic: true),
        new("Create Suppliers", FshActions.Create, FshResources.Suppliers),
        new("Update Suppliers", FshActions.Update, FshResources.Suppliers),
        new("Delete Suppliers", FshActions.Delete, FshResources.Suppliers),
        new("Export Suppliers", FshActions.Export, FshResources.Suppliers),

        //purchases
        new("View Purchases", FshActions.View, FshResources.Purchases, IsBasic: true),
        new("Search Purchases", FshActions.Search, FshResources.Purchases, IsBasic: true),
        new("Create Purchases", FshActions.Create, FshResources.Purchases),
        new("Update Purchases", FshActions.Update, FshResources.Purchases),
        new("Delete Purchases", FshActions.Delete, FshResources.Purchases),
        new("Export Purchases", FshActions.Export, FshResources.Purchases),

        //purchaseitems
        new("View PurchaseItems", FshActions.View, FshResources.PurchaseItems, IsBasic: true),
        new("Search PurchaseItems", FshActions.Search, FshResources.PurchaseItems, IsBasic: true),
        new("Create PurchaseItems", FshActions.Create, FshResources.PurchaseItems),
        new("Update PurchaseItems", FshActions.Update, FshResources.PurchaseItems),
        new("Delete PurchaseItems", FshActions.Delete, FshResources.PurchaseItems),
        new("Export PurchaseItems", FshActions.Export, FshResources.PurchaseItems),

        //todos
        new("View Todos", FshActions.View, FshResources.Todos, IsBasic: true),
        new("Search Todos", FshActions.Search, FshResources.Todos, IsBasic: true),
        new("Create Todos", FshActions.Create, FshResources.Todos),
        new("Update Todos", FshActions.Update, FshResources.Todos),
        new("Delete Todos", FshActions.Delete, FshResources.Todos),
        new("Export Todos", FshActions.Export, FshResources.Todos),

         new("View Hangfire", FshActions.View, FshResources.Hangfire),
         new("View Dashboard", FshActions.View, FshResources.Dashboard),

        //audit
        new("View Audit Trails", FshActions.View, FshResources.AuditTrails),
    ];

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
}

public record FshPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}


