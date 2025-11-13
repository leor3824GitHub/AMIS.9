using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMIS.Blazor.Client.Pages.Catalog.Acceptances;

public partial class AcceptanceDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject]
    private IApiClient ApiClient { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Parameter]
    public AcceptanceFormModel Model { get; set; } = AcceptanceFormModel.CreateDefault();

    [Parameter]
    public bool IsCreate { get; set; }

    [Parameter]
    public bool AllowItemEditing { get; set; } = true;

    [Parameter]
    public IReadOnlyList<EmployeeResponse> SupplyOfficers { get; init; } = Array.Empty<EmployeeResponse>();

    [Parameter]
    public IReadOnlyList<PurchaseResponse> Purchases { get; init; } = Array.Empty<PurchaseResponse>();

    private FshValidation? _validation;
    private DateTime? _acceptanceDate;
    private bool _loadingItems;
    private readonly List<PurchaseItemResponse> _purchaseItems = new();
    private bool CanChangePurchase => IsCreate;
    private bool _prerequisitesMet;
    private string? _prerequisiteMessage;
    private List<EmployeeResponse> _supplyOfficerOptions = new();
    private List<PurchaseResponse> _purchaseOptions = new();
    private bool IsSubmitDisabled => IsCreate && !_prerequisitesMet;

    protected override async Task OnParametersSetAsync()
    {
        if (!IsCreate)
        {
            _prerequisitesMet = true;
            _prerequisiteMessage = null;
        }
        else if (!Model.PurchaseId.HasValue)
        {
            _prerequisitesMet = false;
            _prerequisiteMessage ??= "Select a purchase with a completed inspection before creating an acceptance.";
        }

        _acceptanceDate = Model.AcceptanceDate == default ? DateTime.Today : Model.AcceptanceDate;
        _supplyOfficerOptions = SupplyOfficers?.ToList() ?? new List<EmployeeResponse>();
        _purchaseOptions = Purchases?.ToList() ?? new List<PurchaseResponse>();

        if (Model.PurchaseId.HasValue && _purchaseItems.Count == 0)
        {
            await LoadPurchaseItemsAsync(Model.PurchaseId.Value, seedFromAcceptance: true);
        }
    }

    private async Task LoadPurchaseItemsAsync(Guid purchaseId, bool seedFromAcceptance)
    {
        _loadingItems = true;
        try
        {
            var purchase = await ApiClient.GetPurchaseEndpointAsync("1", purchaseId);
            _purchaseItems.Clear();
            if (purchase?.Items != null)
            {
                _purchaseItems.AddRange(purchase.Items.Where(i => i.Id.HasValue));
            }
            if (purchase?.Id.HasValue == true)
            {
                var existing = _purchaseOptions.FirstOrDefault(p => p.Id == purchase.Id);
                if (existing is not null)
                {
                    var index = _purchaseOptions.IndexOf(existing);
                    _purchaseOptions[index] = purchase;
                }
                else
                {
                    _purchaseOptions.Add(purchase);
                }
            }
            SyncItems(seedFromAcceptance);

            // Refresh single-shot flags (AlreadyAccepted) for UX
            await RefreshComputedQuantitiesAsync(seedFromAcceptance);

            if (IsCreate)
            {
                await EvaluateAcceptanceReadinessAsync(purchaseId);
            }
            else
            {
                _prerequisitesMet = true;
                _prerequisiteMessage = null;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load purchase items: {ex.Message}", Severity.Error);
            _purchaseItems.Clear();
            Model.ClearItems();
            _prerequisitesMet = false;
            _prerequisiteMessage = "Unable to verify inspection prerequisites for the selected purchase.";
        }
        finally
        {
            _loadingItems = false;
            StateHasChanged();
        }
    }

    private void SyncItems(bool preserveExisting)
    {
        var current = Model.Items.ToDictionary(i => i.PurchaseItemId, i => i);
        var updated = new List<AcceptanceFormModel.AcceptanceItemInput>();

        foreach (var item in _purchaseItems)
        {
            if (!item.Id.HasValue)
            {
                continue;
            }

            var id = item.Id.Value;
            if (!current.TryGetValue(id, out var input))
            {
                input = new AcceptanceFormModel.AcceptanceItemInput
                {
                    PurchaseItemId = id,
                    OrderedQty = item.Qty,
                    QtyAccepted = preserveExisting ? 0 : item.Qty,
                    ProductName = item.Product?.Name ?? string.Empty,
                    Remarks = preserveExisting ? null : string.Empty
                };
            }
            else
            {
                input.OrderedQty = item.Qty;
                input.ProductName = item.Product?.Name ?? string.Empty;
                if (!preserveExisting && AllowItemEditing)
                {
                    input.QtyAccepted = item.Qty;
                }
            }

            updated.Add(input);
        }

        Model.ReplaceItems(updated);
    }

    private async Task RefreshComputedQuantitiesAsync(bool preserveExisting)
    {
        if (Model.Items.Count == 0)
        {
            return;
        }

        foreach (var input in Model.Items)
        {
            // Single-shot: mark as already accepted if any acceptance item exists (posted or not)
            var accSearch = new SearchAcceptanceItemsCommand
            {
                PageNumber = 1,
                PageSize = 100,
                PurchaseItemId = input.PurchaseItemId
            };

            var accItems = await ApiClient.SearchAcceptanceItemsEndpointAsync("1", accSearch);
            input.AlreadyAccepted = accItems?.Items?.Count > 0;

            if (!preserveExisting && AllowItemEditing)
            {
                // If this is initial seeding, set to 0 when already accepted; otherwise keep current default
                input.QtyAccepted = input.AlreadyAccepted ? 0 : input.QtyAccepted;
            }
        }

        StateHasChanged();
    }

    private async Task EvaluateAcceptanceReadinessAsync(Guid purchaseId)
    {
        if (!IsCreate)
        {
            _prerequisitesMet = true;
            _prerequisiteMessage = null;
            return;
        }

        try
        {
            var search = new SearchInspectionRequestsCommand
            {
                PageNumber = 1,
                PageSize = 1,
                PurchaseId = purchaseId
            };

            var result = await ApiClient.SearchInspectionRequestsEndpointAsync("1", search);
            var request = result?.Items?.FirstOrDefault();

            if (request is null)
            {
                _prerequisitesMet = false;
                _prerequisiteMessage = "Submit and complete an inspection request for this purchase before creating an acceptance.";
            }
            else if (request.Status is InspectionRequestStatus.Completed or InspectionRequestStatus.Accepted)
            {
                _prerequisitesMet = true;
                _prerequisiteMessage = null;
            }
            else
            {
                _prerequisitesMet = false;
                var statusText = request.Status.ToString();
                _prerequisiteMessage = $"Inspection request is currently {statusText}. Complete the inspection before creating an acceptance.";
            }
        }
        catch (Exception ex)
        {
            _prerequisitesMet = false;
            _prerequisiteMessage = $"Unable to verify inspection status: {ex.Message}";
        }
    }

    private async Task OnPurchaseChanged(Guid? purchaseId)
    {
        if (Model.PurchaseId == purchaseId)
        {
            return;
        }

        Model.PurchaseId = purchaseId;

        if (purchaseId.HasValue)
        {
            await LoadPurchaseItemsAsync(purchaseId.Value, seedFromAcceptance: false);
        }
        else
        {
            _purchaseItems.Clear();
            Model.ClearItems();
            _prerequisitesMet = false;
            _prerequisiteMessage = "Select a purchase with a completed inspection before creating an acceptance.";
            StateHasChanged();
        }
    }

    private void OnSupplyOfficerChanged(Guid? officerId) => Model.SupplyOfficerId = officerId;

    private void OnDateChanged(DateTime? date)
    {
        _acceptanceDate = date ?? DateTime.Today;
        Model.AcceptanceDate = _acceptanceDate.Value;
    }

    private void OnQtyAcceptedChanged(AcceptanceFormModel.AcceptanceItemInput item, int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        item.QtyAccepted = value;
        StateHasChanged();
    }

    private void OnItemRemarkChanged(AcceptanceFormModel.AcceptanceItemInput item, string? value)
    {
        item.Remarks = value;
        StateHasChanged();
    }

    private static string GetPurchaseLabel(PurchaseResponse purchase)
    {
        var supplier = purchase.Supplier?.Name ?? "Unknown supplier";
        var date = purchase.PurchaseDate?.ToString("yyyy-MM-dd") ?? "No date";
        return $"{supplier} · {date}";
    }

    private async Task OnValidSubmit()
    {
        if (!Model.PurchaseId.HasValue)
        {
            Snackbar.Add("Select a purchase before saving.", Severity.Error);
            return;
        }

        if (!Model.SupplyOfficerId.HasValue)
        {
            Snackbar.Add("Select a supply officer before saving.", Severity.Error);
            return;
        }

        if (!_acceptanceDate.HasValue)
        {
            Snackbar.Add("Pick an acceptance date.", Severity.Error);
            return;
        }

        Model.AcceptanceDate = _acceptanceDate.Value;

        var itemsToSubmit = Model.Items
            .Where(i => i.QtyAccepted > 0)
            .Select(i => new AcceptanceItemDto
            {
                AcceptanceId = Guid.Empty,
                PurchaseItemId = i.PurchaseItemId,
                QtyAccepted = i.QtyAccepted,
                Remarks = i.Remarks ?? string.Empty
            })
            .ToList();

        if (Model.Items.Any(i => i.QtyAccepted > i.OrderedQty))
        {
            Snackbar.Add("Accepted quantity cannot exceed ordered quantity.", Severity.Error);
            return;
        }

        if (IsCreate && itemsToSubmit.Count == 0)
        {
            Snackbar.Add("Add at least one item with an accepted quantity.", Severity.Error);
            return;
        }

        if (IsCreate && !_prerequisitesMet)
        {
            Snackbar.Add(_prerequisiteMessage ?? "Complete the inspection before creating an acceptance.", Severity.Error);
            return;
        }

        if (IsCreate)
        {
            // Use aggregate method for create with items
            var aggregateCommand = new UpdateAcceptanceWithItemsCommand
            {
                Id = Guid.NewGuid(),
                SupplyOfficerId = Model.SupplyOfficerId,
                AcceptanceDate = Model.AcceptanceDate,
                Remarks = Model.Remarks,
                Items = itemsToSubmit.Select(i => new AcceptanceItemUpsert
                {
                    PurchaseItemId = i.PurchaseItemId,
                    QtyAccepted = i.QtyAccepted,
                    Remarks = i.Remarks
                }).ToList(),
                DeletedItemIds = new List<Guid>()
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.UpdateAcceptanceWithItemsEndpointAsync("1", aggregateCommand.Id, aggregateCommand),
                Snackbar,
                Navigation,
                _validation);

            if (response != null)
            {
                Snackbar.Add("Acceptance created successfully.", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            if (!Model.Id.HasValue)
            {
                Snackbar.Add("Acceptance identifier is missing.", Severity.Error);
                return;
            }

            // For update, we need to get existing items and determine what to update/delete
            var existingItems = await ApiClient.SearchAcceptanceItemsEndpointAsync("1", new SearchAcceptanceItemsCommand
            {
                AcceptanceId = Model.Id.Value,
                PageNumber = 1,
                PageSize = 1000
            });

            var itemsToUpdate = new List<AcceptanceItemUpsert>();
            var deletedItemIds = new List<Guid>();

            // Process existing items
            if (existingItems?.Items != null)
            {
                foreach (var existing in existingItems.Items)
                {
                    var currentInput = Model.Items.FirstOrDefault(i => i.PurchaseItemId == existing.PurchaseItemId);
                    if (currentInput != null && currentInput.QtyAccepted > 0)
                    {
                        // Update existing item
                        itemsToUpdate.Add(new AcceptanceItemUpsert
                        {
                            Id = existing.Id,
                            PurchaseItemId = existing.PurchaseItemId,
                            QtyAccepted = currentInput.QtyAccepted,
                            Remarks = currentInput.Remarks ?? string.Empty
                        });
                    }
                    else
                    {
                        // Mark for deletion
                        deletedItemIds.Add(existing.Id);
                    }
                }
            }

            // Add new items
            foreach (var newItem in itemsToSubmit)
            {
                var existingItem = existingItems?.Items?.FirstOrDefault(i => i.PurchaseItemId == newItem.PurchaseItemId);
                if (existingItem == null)
                {
                    itemsToUpdate.Add(new AcceptanceItemUpsert
                    {
                        PurchaseItemId = newItem.PurchaseItemId,
                        QtyAccepted = newItem.QtyAccepted,
                        Remarks = newItem.Remarks
                    });
                }
            }

            var aggregateCommand = new UpdateAcceptanceWithItemsCommand
            {
                Id = Model.Id.Value,
                SupplyOfficerId = Model.SupplyOfficerId,
                AcceptanceDate = Model.AcceptanceDate,
                Remarks = Model.Remarks,
                Items = itemsToUpdate,
                DeletedItemIds = deletedItemIds
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.UpdateAcceptanceWithItemsEndpointAsync("1", Model.Id.Value, aggregateCommand),
                Snackbar,
                Navigation,
                _validation);

            if (response != null)
            {
                Snackbar.Add("Acceptance updated successfully.", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }

    private void Cancel() => MudDialog.Cancel();
}
