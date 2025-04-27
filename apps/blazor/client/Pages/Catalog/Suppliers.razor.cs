using AMIS.Blazor.Client.Components.EntityTable;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace AMIS.Blazor.Client.Pages.Catalog;

public partial class Suppliers
{
    [Inject]
    protected IApiClient _client { get; set; } = default!;

    protected EntityServerTableContext<SupplierResponse, Guid, SupplierViewModel> Context { get; set; } = default!;

    private EntityTable<SupplierResponse, Guid, SupplierViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Supplier",
            entityNamePlural: "Suppliers",
            entityResource: FshResources.Suppliers,
            fields: new()
            {
                new(supplier => supplier.Id, "Id", "Id"),
                new(supplier => supplier.Name, "Name", "Name"),
                new(supplier => supplier.Address, "Address", "Address"),
                new(supplier => supplier.Tin, "Tin", "Tin"),
                new(supplier => supplier.TaxClassification, "TaxClassification", "TaxClassification"),
                new(supplier => supplier.ContactNo, "ContactNo", "ContactNo"),
                new(supplier => supplier.Emailadd, "Emailadd", "Emailadd")
            },
            enableAdvancedSearch: true,
            idFunc: supplier => supplier.Id!.Value,
            searchFunc: async filter =>
            {
                var supplierFilter = filter.Adapt<SearchSuppliersCommand>();
                var result = await _client.SearchSuppliersEndpointAsync("1", supplierFilter);
                return result.Adapt<PaginationResponse<SupplierResponse>>();
            },
            createFunc: async supplier =>
            {
                await _client.CreateSupplierEndpointAsync("1", supplier.Adapt<CreateSupplierCommand>());
            },
            updateFunc: async (id, supplier) =>
            {
                await _client.UpdateSupplierEndpointAsync("1", id, supplier.Adapt<UpdateSupplierCommand>());
            },
            deleteFunc: async id => await _client.DeleteSupplierEndpointAsync("1", id));
}

public class SupplierViewModel : UpdateSupplierCommand
{
}
