using AMIS.Blazor.Infrastructure.Api;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories;

public partial class PropertyCodeGenerator
{
    private const string ApiVersion = "1";

    [CascadingParameter]
    private MudBlazor.IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject]
    protected IApiClient ApiClientService { get; set; } = default!;
    [Inject]
    protected ISnackbar? Snackbar { get; set; }

    private string _selectedClassification = string.Empty;
    private string _selectedItemDescription = string.Empty;
    private string _classCode = string.Empty;
    private string _categoryCode = string.Empty;
    private string _itemCode = string.Empty;
    private string _generatedPropertyCode = string.Empty;
    private int _allocatedSequence;
    private bool _generating;

    private Dictionary<string, List<string>> _itemDescriptionsByClassification = new();
    private readonly Dictionary<string, PropertyCodeSequenceDto> _propertyCodeSequenceLookup = new();
    private List<string> _itemDescriptions = new();
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadPropertyCodeSequencesAsync();
    }

    private async Task LoadPropertyCodeSequencesAsync()
    {
        _loading = true;
        try
        {
            // Load property code sequences from server to populate classifications and descriptions
            var response = await ApiClientService.PropertyCodeSequenceEndpointsListAsync(
                ApiVersion,
                pageNumber: 1,
                pageSize: 500,  // Get all records for dropdown
                searchTerm: null);

            if (response?.Items != null && response.Items.Any())
            {
                // Group by Classification and collect ItemDescriptions
                _itemDescriptionsByClassification = response.Items
                    .GroupBy(x => x.Classification ?? string.Empty)
                    .Where(g => !string.IsNullOrWhiteSpace(g.Key))
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ItemDescription ?? string.Empty)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Distinct()
                            .OrderBy(x => x)
                            .ToList());

                _propertyCodeSequenceLookup.Clear();
                foreach (var item in response.Items)
                {
                    var key = BuildSequenceLookupKey(item.Classification, item.ItemDescription);
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        _propertyCodeSequenceLookup[key] = item;
                    }
                }

                Snackbar?.Add($"Loaded {_itemDescriptionsByClassification.Count} classifications from server", Severity.Success);
            }
            else
            {
                Snackbar?.Add("No property code sequences found on server, using defaults", Severity.Warning);
                LoadDefaultData();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading from server: {ex.Message}. Using default data.", Severity.Error);
            LoadDefaultData();
        }
        finally
        {
            _loading = false;
        }
    }

    private void LoadDefaultData()
    {
        // Fallback to hardcoded data if API call fails
        _itemDescriptionsByClassification = new()
        {
            { "OFFICE EQUIPMENT", new() { "ADDING MACHINE", "AIR CONDITIONER", "CABINET", "CHAIR", "DESK", "FAN", "HEATER", "REFRIGERATOR", "SAFE", "TABLE" } },
            { "COMPUTER EQUIPMENT", new() { "CPU", "MONITOR", "KEYBOARD", "MOUSE", "PRINTER", "SCANNER", "ROUTER", "MODEM", "LAPTOP", "TABLET" } },
            { "FURNITURE & FIXTURES", new() { "BED", "WARDROBE", "SHELF", "LOCKER", "BENCH", "COUNTER", "CABINET", "FILING CABINET", "PARTITION", "STAND" } },
            { "VEHICLES", new() { "VAN", "CAR", "TRUCK", "MOTORCYCLE", "BUS", "JEEP", "AMBULANCE", "FIRE TRUCK", "TRAILER", "TANKER" } },
            { "MACHINERY", new() { "GENERATOR", "PUMP", "COMPRESSOR", "MIXER", "GRINDER", "MOTOR", "TURBINE", "CONVEYOR", "PRESS", "LATHE" } },
            { "BUILDING & STRUCTURES", new() { "BUILDING", "GATE", "FENCE", "WALL", "ROOF", "BRIDGE", "CULVERT", "PAVILION", "SHED", "TOWER" } },
            { "TOOLS & IMPLEMENTS", new() { "HAMMER", "WRENCH", "SCREWDRIVER", "SAW", "DRILL", "PLIER", "CHISEL", "AXE", "SHOVEL", "PICKAXE" } },
            { "LAND", new() { "AGRICULTURAL LAND", "COMMERCIAL LAND", "RESIDENTIAL LAND", "INDUSTRIAL LAND", "RECREATIONAL LAND", "FOREST LAND", "WATER BODY", "MINERAL LAND", "PASTURE LAND", "WASTE LAND" } },
            { "INTANGIBLE ASSETS", new() { "SOFTWARE LICENSE", "PATENT", "TRADEMARK", "COPYRIGHT", "DOMAIN NAME", "FRANCHISE", "LEASE", "SUBSCRIPTION", "WARRANTY", "SERVICE AGREEMENT" } },
            { "LIVESTOCK", new() { "COW", "BUFFALO", "GOAT", "SHEEP", "PIG", "CHICKEN", "HORSE", "DUCK", "TURKEY", "FISH" } }
        };
    }

    private Task OnClassificationChanged(string classification)
    {
        _selectedClassification = classification;
        _selectedItemDescription = string.Empty;

        if (_itemDescriptionsByClassification.TryGetValue(classification, out var items))
        {
            _itemDescriptions = items;
        }
        else
        {
            _itemDescriptions = new();
        }

        return Task.CompletedTask;
    }

    private async Task GeneratePropertyCode()
    {
        if (string.IsNullOrWhiteSpace(_selectedClassification) || string.IsNullOrWhiteSpace(_selectedItemDescription))
        {
            Snackbar?.Add("Classification and Item Description are required.", Severity.Warning);
            return;
        }

        _generating = true;
        try
        {
            var allocateRequest = new AllocatePropertyCodeSequenceRequest
            {
                Classification = _selectedClassification.Trim(),
                ItemDescription = _selectedItemDescription.Trim()
            };

            var response = await ApiClientService.PropertyCodeSequenceEndpointsAllocateAsync(ApiVersion, allocateRequest);

            // Use the directly returned response data instead of looking it up
            _classCode = response?.ClassCode ?? string.Empty;
            _categoryCode = response?.CategoryCode ?? string.Empty;
            _itemCode = response?.ItemCode ?? string.Empty;
            _allocatedSequence = response?.NextSequenceNumber ?? 0;

            _generatedPropertyCode = BuildPropertyCode();

            Snackbar?.Add("Property code generated successfully!", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error generating property code: {ex.Message}", Severity.Error);
            ResetGeneratedValues();
        }
        finally
        {
            _generating = false;
        }
    }

    private string BuildPropertyCode()
    {
        var currentYear = DateTime.Now.Year.ToString();
        var userOfficeCode = "NFA";
        var formattedSequence = _allocatedSequence.ToString("D4");

        return $"{currentYear}-{userOfficeCode}-{_classCode}-{_categoryCode}-{_itemCode}-{formattedSequence}";
    }

    private void ResetForm()
    {
        _selectedClassification = string.Empty;
        _selectedItemDescription = string.Empty;
        ResetGeneratedValues();
        _itemDescriptions = new();
    }

    private void ResetGeneratedValues()
    {
        _classCode = string.Empty;
        _categoryCode = string.Empty;
        _itemCode = string.Empty;
        _generatedPropertyCode = string.Empty;
        _allocatedSequence = 0;
    }

    private async Task CopyToClipboard()
    {
        try
        {
            await Task.Run(() =>
            {
                Snackbar?.Add($"Copied: {_generatedPropertyCode}", Severity.Info);
            });
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error copying to clipboard: {ex.Message}", Severity.Error);
        }
    }

    private async Task UseCodeInDialog()
    {
        if (string.IsNullOrWhiteSpace(_generatedPropertyCode))
        {
            Snackbar?.Add("Please generate a property code first.", Severity.Warning);
            return;
        }

        // Close dialog and return the generated property code
        MudDialog?.Close(DialogResult.Ok(_generatedPropertyCode));
        await Task.CompletedTask;
    }

    private void Cancel()
    {
        MudDialog?.Cancel();
    }

    private static string BuildSequenceLookupKey(string? classification, string? itemDescription)
    {
        if (string.IsNullOrWhiteSpace(classification) || string.IsNullOrWhiteSpace(itemDescription))
        {
            return string.Empty;
        }

        return $"{classification.Trim().ToUpperInvariant()}||{itemDescription.Trim().ToUpperInvariant()}";
    }
}
