using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Issuances;

public partial class IssuanceItemList
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }

    [Parameter]
    public ICollection<IssuanceItemDto> Items { get; set; } = new List<IssuanceItemDto>();
    [Parameter]
    public IReadOnlyList<ProductResponse> Products { get; set; } = Array.Empty<ProductResponse>();
    [Parameter]
    public Guid? IssuanceId { get; set; }
    [Parameter]
    public Action<double> OnTotalAmountChanged { get; set; } = _ => { };
    [Parameter]
    public bool? IsCreate { get; set; }

    private Guid? ProductId { get; set; }
    private int Qty { get; set; }
    private double UnitPrice { get; set; }
    private string Status { get; set; } = "Pending";
    private IssuanceItemDto? EditingItem { get; set; }

    protected override void OnParametersSet()
    {
        Items ??= new List<IssuanceItemDto>();
    }

    private void EditItem(IssuanceItemDto item)
    {
        EditingItem = item;
    }

    private async void SaveEdit()
    {
        if (EditingItem == null || EditingItem.Qty <= 0 || EditingItem.UnitPrice <= 0 || EditingItem.ProductId is null)
            return;

        try
        {
            if (IsCreate == false)
            {
                var model = new UpdateIssuanceItemCommand
                {
                    Id = EditingItem.Id,
                    IssuanceId = IssuanceId ?? Guid.Empty,
                    ProductId = EditingItem.ProductId!.Value,
                    Qty = EditingItem.Qty,
                    UnitPrice = EditingItem.UnitPrice,
                    Status = EditingItem.Status
                };

                await ApiClient.UpdateIssuanceItemEndpointAsync("1", model.Id, model);
                Snackbar?.Add("Item updated.", Severity.Success);
            }

            EditingItem = null;
            UpdateTotalAmount();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
            Snackbar?.Add("The item wasn't updated.", Severity.Error);
        }
    }

    private void CancelEdit()
    {
        EditingItem = null;
    }

    private async void AddNewItem()
    {
        if (ProductId == null || Qty <= 0 || UnitPrice <= 0)
            return;

        var newItem = new IssuanceItemDto
        {
            Id = Guid.NewGuid(),
            IssuanceId = IssuanceId,
            ProductId = ProductId,
            Qty = Qty,
            UnitPrice = UnitPrice,
            Status = Status
        };

        Items.Add(newItem);

        if (IsCreate == false)
        {
            var model = new CreateIssuanceItemCommand
            {
                IssuanceId = IssuanceId ?? Guid.Empty,
                ProductId = newItem.ProductId!.Value,
                Qty = newItem.Qty,
                UnitPrice = newItem.UnitPrice,
                Status = newItem.Status
            };

            await ApiClient.CreateIssuanceItemEndpointAsync("1", model);
            Snackbar?.Add("Item added.", Severity.Success);
        }

        // Reset fields after adding
        ProductId = null;
        Qty = 0;
        UnitPrice = 0;
        Status = "Pending";

        UpdateTotalAmount();
    }

    private async void RemoveItem(IssuanceItemDto item)
    {
        if (item.Id == Guid.Empty)
        {
            Snackbar?.Add("Item ID is empty and cannot be removed.", Severity.Error);
            return;
        }
        try
        {
            if (IsCreate == false)
            {
                await ApiClient.DeleteIssuanceItemEndpointAsync("1", item.Id);
                Snackbar?.Add("Item removed.", Severity.Success);
            }

            Items.Remove(item);
            UpdateTotalAmount();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
            Snackbar?.Add("The item wasn't removed.", Severity.Error);
        }
    }

    private void UpdateTotalAmount()
    {
        double total = Items.Sum(i => i.Qty * i.UnitPrice);
        OnTotalAmountChanged?.Invoke(total);
        StateHasChanged();
    }
}
