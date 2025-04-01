using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Product2;
public class CategoryAutoComplete : MudAutocomplete<Guid>
{
    [Inject]
    private IApiClient CategoriesClient { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<CategoryResponse> _categories = new();
    [Parameter]
    public EventCallback<List<CategoryResponse>> GetCategoryChanged { get; set; }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Categories";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchCategories;
        ToStringFunc = GetCategoryName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _value != Guid.Empty)
        {
            try
            {
                var category = await CategoriesClient.GetCategoryEndpointAsync("1", _value);

                _categories.Add(category);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading category: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task<IEnumerable<Guid>> SearchCategories(string? value, CancellationToken token)
    {
        var filter = new SearchCategorysCommand
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "Name" }, Keyword = value }
        };

        var response = await CategoriesClient.SearchCategorysEndpointAsync("1", filter, token);

        if (response is CategoryResponsePagedList { Items: { } categories })
        {
            _categories = categories.ToList();
        }

        return _categories.Select(x => x.Id); // Keeps it as IEnumerable<Guid?>
    }

private string GetCategoryName(Guid id)
    {
        var category = _categories.Find(b => b.Id == id);
        return category?.Name ?? string.Empty;
    }
}
