using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;

public partial class PurchaseForm
{
    [Inject] private IApiClient PurchaseClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public Guid? Id { get; set; }

    private CreatePurchaseCommand Model { get; set; } = new();
    private bool? IsCreate { get; set; }

    private readonly IReadOnlyList<PurchaseStatus> _statusOptions = Enum.GetValues<PurchaseStatus>();

    private List<SupplierResponse> _suppliers = new();
    private List<ProductResponse> _products = new();
    private SupplierResponse? _selectedSupplier;
    private PurchaseFinancialSnapshot _financialSnapshot = PurchaseFinancialSnapshot.Empty;
    private StatusAdvisory? _statusAdvisory;
    private string? Notes { get; set; }
    private HashSet<Guid> _originalItemIds = new();
    private HashSet<Guid> _deletedItemIds = new();
    private EditContext? _editContext;
    private bool _isSaving;
    private bool _isDirty;

    private string FormattedTotal => Model.TotalAmount.ToString("C", CultureInfo.CurrentCulture);

    protected override async Task OnInitializedAsync()
    {
        Model.Items ??= new List<PurchaseItemDto>();

        // Determine if create or edit based on Id parameter
        IsCreate = !Id.HasValue || Id.Value == Guid.Empty;

        if (!IsCreate.Value && Id.HasValue)
        {
            // Load existing purchase for editing
            await LoadPurchaseAsync(Id.Value);
        }
        else
        {
            // Initialize new purchase
            Model.PurchaseDate = DateTime.Today;
            Model.Status = PurchaseStatus.Pending;
        }

        await Task.WhenAll(LoadSupplierAsync(), LoadProductAsync());

        ResolveSelectedSupplier();
        ComputeFinancialSnapshot();
        ResetEditContext();
    }

    private async Task LoadPurchaseAsync(Guid id)
    {
        try
        {
            var response = await PurchaseClient.GetPurchaseEndpointAsync("1", id);
            if (response != null)
            {
                Model = response.Adapt<CreatePurchaseCommand>();
                _originalItemIds = response.Items?
                    .Where(i => i.Id.HasValue)
                    .Select(i => i.Id!.Value)
                    .ToHashSet() ?? new HashSet<Guid>();
                ResetEditContext();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unable to load purchase: {ex.Message}", Severity.Error);
            NavigationManager.NavigateTo("/catalog/purchases");
        }
    }

    protected override void OnParametersSet()
    {
        Model.Items ??= new List<PurchaseItemDto>();
        ResolveSelectedSupplier();
        ComputeFinancialSnapshot();
    }

    private async Task LoadProductAsync()
    {
        if (_products.Count > 0)
        {
            return;
        }

        try
        {
            var request = new SearchProductsCommand
            {
                PageNumber = 1,
                PageSize = 50,
                OrderBy = new List<string> { "Name" }
            };

            var response = await PurchaseClient.SearchProductsEndpointAsync("1", request);
            if (response?.Items != null)
            {
                _products = response.Items.ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unable to load product catalog: {ex.Message}", Severity.Error);
        }
    }

    private async Task LoadSupplierAsync()
    {
        if (_suppliers.Count > 0)
        {
            return;
        }

        try
        {
            var request = new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 20,
                OrderBy = new List<string> { "Name" }
            };

            var response = await PurchaseClient.SearchSuppliersEndpointAsync("1", request);
            if (response?.Items != null)
            {
                _suppliers = response.Items.ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unable to load suppliers: {ex.Message}", Severity.Error);
        }
    }

    private void ResolveSelectedSupplier()
    {
        if (!Model.SupplierId.HasValue)
        {
            _selectedSupplier = null;
            return;
        }

        _selectedSupplier = _suppliers.FirstOrDefault(s => s.Id == Model.SupplierId)
            ?? _selectedSupplier;
    }

    private void ComputeFinancialSnapshot()
    {
        var items = Model.Items ?? new List<PurchaseItemDto>();

        if (items.Count == 0)
        {
            _financialSnapshot = PurchaseFinancialSnapshot.Empty;
            _statusAdvisory = ResolveStatusAdvisory(Model.Status, _financialSnapshot);
            return;
        }

        double commitment = items.Sum(item => item.Qty * item.UnitPrice);
        int lineItems = items.Count;
        int distinctProducts = items
            .Where(item => item.ProductId.HasValue)
            .Select(item => item.ProductId!.Value)
            .Distinct()
            .Count();

        int completedItems = items.Count(item => item.ItemStatus is PurchaseStatus.FullyReceived or PurchaseStatus.Delivered or PurchaseStatus.Invoiced or PurchaseStatus.PendingPayment or PurchaseStatus.Closed);
        int pendingItems = items.Count(item => item.ItemStatus is PurchaseStatus.PendingApproval or PurchaseStatus.Pending or PurchaseStatus.Draft or PurchaseStatus.OnHold or PurchaseStatus.Submitted or PurchaseStatus.InProgress or PurchaseStatus.Shipped or PurchaseStatus.PartiallyReceived or PurchaseStatus.PartiallyDelivered);

        _financialSnapshot = new PurchaseFinancialSnapshot(commitment, lineItems, distinctProducts, pendingItems, completedItems);
        Model.TotalAmount = commitment;
        _statusAdvisory = ResolveStatusAdvisory(Model.Status, _financialSnapshot);
    }

    private static StatusAdvisory? ResolveStatusAdvisory(PurchaseStatus status, PurchaseFinancialSnapshot snapshot)
    {
        return status switch
        {
            PurchaseStatus.PendingApproval or PurchaseStatus.Pending => new StatusAdvisory(Severity.Warning, Icons.Material.Filled.Gavel, "Awaiting approval to progress. Ensure approvers have the latest supplier terms."),
            PurchaseStatus.OnHold => new StatusAdvisory(Severity.Warning, Icons.Material.Filled.PauseCircle, "This purchase is on hold. Review pending blockers or compliance exceptions."),
            PurchaseStatus.Rejected => new StatusAdvisory(Severity.Error, Icons.Material.Filled.Block, "Purchase order rejected. Capture remediation notes before resubmitting."),
            PurchaseStatus.Cancelled => new StatusAdvisory(Severity.Error, Icons.Material.Filled.Cancel, "Order cancelled. Validate that downstream invoices or receipts are voided."),
            PurchaseStatus.Closed when snapshot.Commitment > 0 => new StatusAdvisory(Severity.Success, Icons.Material.Filled.Verified, "Procurement cycle closed. Ensure invoice reconciliation is archived."),
            _ => null
        };
    }

    private static string GetStatusDisplay(PurchaseStatus? status)
    {
        if (!status.HasValue)
        {
            return "Unspecified";
        }

        var memberInfo = typeof(PurchaseStatus).GetMember(status.Value.ToString()).FirstOrDefault();
        var description = memberInfo?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        return description ?? status.Value.ToString();
    }

    private async Task<IEnumerable<SupplierResponse>> SearchSuppliersAsync(string value, CancellationToken cancellationToken = default)
    {
        var keyword = value?.Trim();

        var request = new SearchSuppliersCommand
        {
            PageNumber = 1,
            PageSize = 10,
            Keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
            OrderBy = new List<string> { "Name" }
        };

        try
        {
            var response = await PurchaseClient.SearchSuppliersEndpointAsync("1", request, cancellationToken);
            _suppliers = response?.Items?.ToList() ?? new List<SupplierResponse>();
            ResolveSelectedSupplier();
            return _suppliers;
        }
        catch (OperationCanceledException)
        {
            return Enumerable.Empty<SupplierResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unable to search suppliers: {ex.Message}", Severity.Error);
            return Enumerable.Empty<SupplierResponse>();
        }
    }

    private Task OnSupplierChanged(SupplierResponse? supplier)
    {
        _selectedSupplier = supplier;
        Model.SupplierId = supplier?.Id;
        _isDirty = true;
        // Notify EditContext that SupplierId changed (programmatic update)
        _editContext?.NotifyFieldChanged(new FieldIdentifier(Model, nameof(Model.SupplierId)));
        return Task.CompletedTask;
    }

    private async Task OnValidSubmit()
    {
        _isSaving = true;
        StateHasChanged();
        if (IsCreate is not true && IsCreate is not false)
        {
            _isSaving = false;
            return;
        }

        // Validate that a supplier is selected
        if (_selectedSupplier is null || !_selectedSupplier.Id.HasValue)
        {
            Snackbar.Add("Please select a supplier before saving.", Severity.Warning);
            return;
        }

        Snackbar.Add(IsCreate.Value ? "Creating purchase order..." : "Updating purchase order...", Severity.Info);

        try
        {
            if (IsCreate.Value)
            {
                var model = Model.Adapt<CreatePurchaseCommand>();
                var response = await PurchaseClient.CreatePurchaseEndpointAsync("1", model);

                if (response.Id.HasValue)
                {
                    Model.Id = response.Id.Value;
                    Snackbar.Add("Purchase order created successfully!", Severity.Success);
                    NavigationManager.NavigateTo("/catalog/purchases");
                }
            }
            else
            {
                // Build aggregate update command
                var update = new UpdatePurchaseWithItemsCommand
                {
                    Id = Model.Id,
                    SupplierId = Model.SupplierId,
                    PurchaseDate = Model.PurchaseDate,
                    // UpdatePurchaseWithItemsCommand.TotalAmount is double per generated client; remove invalid decimal cast.
                    TotalAmount = Model.TotalAmount,
                    Status = Model.Status,
                    ReferenceNumber = Model.ReferenceNumber,
                    Notes = Notes ?? Model.Notes,
                    Currency = Model.Currency,
                    Items = Model.Items?.Select(i => new PurchaseItemUpsert
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        Qty = i.Qty,
                        // PurchaseItemUpsert.UnitPrice is double; remove decimal cast.
                        UnitPrice = i.UnitPrice,
                        ItemStatus = i.ItemStatus
                    }).ToList() ?? new List<PurchaseItemUpsert>(),
                    DeletedItemIds = _deletedItemIds.ToList()
                };

                var response = await PurchaseClient.UpdatePurchaseWithItemsEndpointAsync("1", update.Id, update);

                if (response != null)
                {
                    // Light refresh: fetch the latest purchase from the server and re-bind Model
                    var refreshed = await PurchaseClient.GetPurchaseEndpointAsync("1", update.Id);
                    if (refreshed is not null)
                    {
                        Model = refreshed.Adapt<CreatePurchaseCommand>();
                        _originalItemIds = refreshed.Items?.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet() ?? new HashSet<Guid>();
                        _deletedItemIds.Clear();
                        ComputeFinancialSnapshot();
                        _isDirty = false;
                        ResetEditContext();
                    }

                    Snackbar.Add("Purchase order updated successfully!", Severity.Success);
                    StateHasChanged();
                }
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isSaving = false;
            StateHasChanged();
        }
    }

    private void UpdateTotalAmount(double value)
    {
        Model.TotalAmount = value;
        ComputeFinancialSnapshot();
        StateHasChanged();
    }

    private static void Cancel()
    {
        // Navigation is handled in the UI
    }

    private void OnItemRemoved(Guid id)
    {
        if (_originalItemIds.Contains(id))
        {
            _deletedItemIds.Add(id);
        }
        _isDirty = true;
    }

    private void OnItemsChanged(ICollection<PurchaseItemDto> items)
    {
        // Model.Items is already bound by reference; ensure snapshot recomputation and UI update
        ComputeFinancialSnapshot();
        _isDirty = true;
        StateHasChanged();
    }

    private void ResetEditContext()
    {
        if (_editContext != null)
        {
            _editContext.OnFieldChanged -= HandleFieldChanged;
        }
        _editContext = new EditContext(Model);
        _editContext.OnFieldChanged += HandleFieldChanged;
        _isDirty = false;
    }

    private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _isDirty = true;
    }

    private readonly record struct PurchaseFinancialSnapshot(double Commitment, int LineItems, int DistinctProducts, int PendingItems, int CompletedItems)
    {
        public static PurchaseFinancialSnapshot Empty => new(0, 0, 0, 0, 0);
    }

    private readonly record struct StatusAdvisory(Severity Severity, string Icon, string Message);
}