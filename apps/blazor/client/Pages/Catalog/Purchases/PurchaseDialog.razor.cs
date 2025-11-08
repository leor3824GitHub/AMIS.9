using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;



namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;

[SuppressMessage("Design", "CA1052", Justification = "Blazor components must remain public for the generated markup partial class.")]
public partial class PurchaseDialog
{
    [Inject] private IApiClient PurchaseClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public CreatePurchaseCommand Model { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    

    private readonly IReadOnlyList<PurchaseStatus> _statusOptions = Enum.GetValues<PurchaseStatus>();

    private List<SupplierResponse> _suppliers = new();
    private List<ProductResponse> _products = new();
    private SupplierResponse? _selectedSupplier;
    private PurchaseFinancialSnapshot _financialSnapshot = PurchaseFinancialSnapshot.Empty;
    private StatusAdvisory? _statusAdvisory;

    private string DialogTitle => (IsCreate ?? Model.Id == Guid.Empty)
        ? "Create Purchase Order"
        : "Update Purchase Order";

    private string DialogSubtitle => Model.Id == Guid.Empty
        ? "Draft purchase order"
        : $"Tracking ID: {FormatIdentifier(Model.Id)}";

    protected override async Task OnInitializedAsync()
    {
        Model.Items ??= new List<PurchaseItemDto>();

        await Task.WhenAll(LoadSupplierAsync(), LoadProductAsync());

        ResolveSelectedSupplier();
        ComputeFinancialSnapshot();
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

    private static string FormatIdentifier(Guid id) => id == Guid.Empty
        ? "--"
        : id.ToString("N")[..8].ToUpperInvariant();

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

    private static Color GetStatusChipColor(PurchaseStatus? status) => status switch
    {
        PurchaseStatus.PendingApproval or PurchaseStatus.Pending => Color.Warning,
        PurchaseStatus.OnHold or PurchaseStatus.Rejected or PurchaseStatus.Cancelled => Color.Error,
        PurchaseStatus.FullyReceived or PurchaseStatus.Delivered or PurchaseStatus.Invoiced or PurchaseStatus.PendingPayment or PurchaseStatus.Closed => Color.Success,
        _ => Color.Info
    };

    private static string GetStatusIcon(PurchaseStatus? status) => status switch
    {
        PurchaseStatus.PendingApproval or PurchaseStatus.Pending => Icons.Material.Filled.Gavel,
        PurchaseStatus.OnHold => Icons.Material.Filled.PauseCircle,
        PurchaseStatus.Rejected => Icons.Material.Filled.Block,
        PurchaseStatus.Cancelled => Icons.Material.Filled.Cancel,
        PurchaseStatus.FullyReceived or PurchaseStatus.Delivered or PurchaseStatus.Closed => Icons.Material.Filled.Verified,
        PurchaseStatus.Invoiced => Icons.Material.Filled.ReceiptLong,
        PurchaseStatus.PendingPayment => Icons.Material.Filled.AccountBalance,
        _ => Icons.Material.Filled.Inventory2
    };

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
        return Task.CompletedTask;
    }

    private async Task OnValidSubmit()
    {
        if (IsCreate is not true && IsCreate is not false)
        {
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
                    await Refresh.InvokeAsync();
                }
            }
            else
            {
                var model = Model.Adapt<UpdatePurchaseCommand>();
                var response = await PurchaseClient.UpdatePurchaseEndpointAsync("1", model.Id, model);

                if (response != null)
                {
                    Snackbar.Add("Purchase order updated successfully!", Severity.Success);
                    await Refresh.InvokeAsync();
                }
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
    }    
    private void UpdateTotalAmount(double value)
    {
        Model.TotalAmount = value;
        ComputeFinancialSnapshot();
        StateHasChanged();
    }

    private void Cancel() 
    {
        MudDialog.Cancel();
        Refresh.InvokeAsync();
    } 

    private readonly record struct PurchaseFinancialSnapshot(double Commitment, int LineItems, int DistinctProducts, int PendingItems, int CompletedItems)
    {
        public static PurchaseFinancialSnapshot Empty => new(0, 0, 0, 0, 0);

        public double AverageUnitCost => LineItems == 0 ? 0 : Commitment / LineItems;

        public double CompletionRate => LineItems == 0 ? 0 : CompletedItems / (double)LineItems;

        public int RemainingItems => Math.Max(LineItems - CompletedItems, 0);
    }

    private readonly record struct StatusAdvisory(Severity Severity, string Icon, string Message);

}
