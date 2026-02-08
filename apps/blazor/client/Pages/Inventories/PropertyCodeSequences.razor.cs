using AMIS.Blazor.Client.Components.EntityTable;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Inventories;

public partial class PropertyCodeSequences
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    [Inject]
    protected ISnackbar? Snackbar { get; set; }

    protected EntityServerTableContext<PropertyCodeSequenceDto, int, PropertyCodeSequenceViewModel> Context { get; set; } = default!;

    private EntityTable<PropertyCodeSequenceDto, int, PropertyCodeSequenceViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "PropertyCodeSequence",
            entityNamePlural: "Property Code Sequences",
            entityResource: FshResources.PropertyCodeSequences,
            fields: new()
            {
                new(seq => seq.Classification, "Classification", "Classification"),
                new(seq => seq.CategoryCode, "Category Code", "CategoryCode"),
                new(seq => seq.ClassCode, "Class Code", "ClassCode"),
                new(seq => seq.ItemCode, "Item Code", "ItemCode"),
                new(seq => seq.ItemDescription, "Item Description", "ItemDescription"),
                new(seq => seq.LastSequenceValue, "Last Sequence", "LastSequenceValue")
            },
            enableAdvancedSearch: true,
            idFunc: seq => seq.Id,
            searchFunc: async filter =>
            {
                try
                {
                    var result = await ApiClient.PropertyCodeSequenceEndpointsListAsync(
                        "1",
                        pageNumber: filter.PageNumber > 0 ? filter.PageNumber : 1,
                        pageSize: filter.PageSize > 0 ? filter.PageSize : 10,
                        searchTerm: filter.Keyword);
                    
                    return new PaginationResponse<PropertyCodeSequenceDto>
                    {
                        Items = result.Items?.ToList() ?? new List<PropertyCodeSequenceDto>(),
                        TotalCount = result.TotalCount,
                        CurrentPage = result.PageNumber,
                        PageSize = result.PageSize
                    };
                }
                catch (Exception ex)
                {
                    Snackbar?.Add($"Error loading sequences: {ex.Message}", Severity.Error);
                    return new PaginationResponse<PropertyCodeSequenceDto> { Items = new() };
                }
            },
            createFunc: async sequence =>
            {
                try
                {
                    var request = new CreatePropertyCodeSequenceRequest
                    {
                        Classification = sequence.Classification,
                        Category = sequence.Category
                    };
                    await ApiClient.PropertyCodeSequenceEndpointsCreateAsync("1", request);
                    Snackbar?.Add("Property code sequence created successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar?.Add($"Error creating sequence: {ex.Message}", Severity.Error);
                    throw;
                }
            },
            updateFunc: async (id, sequence) =>
            {
                try
                {
                    var request = new UpdatePropertyCodeSequenceRequest
                    {
                        Classification = sequence.Classification,
                        Category = sequence.Category,
                        LastSequenceValue = sequence.LastSequenceValue
                    };
                    await ApiClient.PropertyCodeSequenceEndpointsUpdateAsync("1", id, request);
                    Snackbar?.Add("Property code sequence updated successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar?.Add($"Error updating sequence: {ex.Message}", Severity.Error);
                    throw;
                }
            },
            deleteFunc: async id =>
            {
                try
                {
                    await ApiClient.PropertyCodeSequenceEndpointsDeleteAsync("1", id);
                    Snackbar?.Add("Property code sequence deleted successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar?.Add($"Error deleting sequence: {ex.Message}", Severity.Error);
                    throw;
                }
            });
}

public class PropertyCodeSequenceViewModel
{
    public int Id { get; set; }
    public string Classification { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ClassCode { get; set; } = string.Empty;
    public string ItemCode { get; set; } = string.Empty;
    public string ItemDescription { get; set; } = string.Empty;
    public string? GLAccount { get; set; }
    public int LastSequenceValue { get; set; }
}
