using AMIS.Blazor.Client.Components.EntityTable;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace AMIS.Blazor.Client.Pages.Catalog;

public partial class Categories
{
    [Inject]
    protected IApiClient Client { get; set; } = default!;

    protected EntityServerTableContext<CategoryResponse, Guid, CategoryViewModel> Context { get; set; } = default!;

    private EntityTable<CategoryResponse, Guid, CategoryViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Category",
            entityNamePlural: "Categories",
            entityResource: FshResources.Categories,
            fields: new()
            {
                new(category => category.Id, "Id", "Id"),
                new(category => category.Name, "Name", "Name"),
                new(category => category.Description, "Description", "Description")
            },
            enableAdvancedSearch: true,
            idFunc: category => category.Id,
            searchFunc: async filter =>
            {
                var categoryFilter = filter.Adapt<SearchCategorysCommand>();
                var result = await Client.SearchCategorysEndpointAsync("1", categoryFilter);
                return result.Adapt<PaginationResponse<CategoryResponse>>();
            },
            createFunc: async category =>
            {
                await Client.CreateCategoryEndpointAsync("1", category.Adapt<CreateCategoryCommand>());
            },
            updateFunc: async (id, category) =>
            {
                await Client.UpdateCategoryEndpointAsync("1", id, category.Adapt<UpdateCategoryCommand>());
            },
            deleteFunc: async id => await Client.DeleteCategoryEndpointAsync("1", id));
}

public class CategoryViewModel : UpdateCategoryCommand
{
}
