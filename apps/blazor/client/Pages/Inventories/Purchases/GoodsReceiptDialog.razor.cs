using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Blazor.Shared.Purchases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMIS.Blazor.Client.Pages.Inventories.Purchases
{
    public partial class GoodsReceiptDialog
    {
        [Parameter]
        public PurchaseResponse Purchase { get; set; } = new();

        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; } = default!;

        [Inject]
        private IApiClient ApiClient { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        private GoodsReceiptCommand _command = new();

        protected override void OnInitialized()
        {
            _command = new GoodsReceiptCommand
            {
                PurchaseId = Purchase.Id ?? Guid.Empty,
                DeliveryDate = DateTime.Now,
                Items = Purchase.Items?.Select(p => new GoodsReceiptItemDto
                {
                    PurchaseItemId = p.Id ?? Guid.Empty,
                    ProductId = p.ProductId ?? Guid.Empty,
                    ProductName = p.Product?.Name ?? string.Empty,
                    QtyOrdered = p.Qty,
                    QtyPreviouslyReceived = 0,
                    QtyReceived = p.Qty // Default to ordered quantity
                }).ToList() ?? new List<GoodsReceiptItemDto>()
            };
        }

        private async Task Submit()
        {
            if (_command.Items.Count == 0)
            {
                Snackbar.Add("No receipt lines to submit.", Severity.Warning);
                return;
            }

            var invalidQty = _command.Items.Any(i => i.QtyReceived < 0 || i.QtyReceived > (i.QtyOrdered - i.QtyPreviouslyReceived));
            if (invalidQty)
            {
                Snackbar.Add("Received quantity must be between 0 and the remaining ordered quantity.", Severity.Warning);
                return;
            }

            if (_command.Items.All(i => i.QtyReceived == 0))
            {
                Snackbar.Add("Enter at least one received quantity.", Severity.Warning);
                return;
            }

            var fullyReceived = _command.Items.All(i => (i.QtyPreviouslyReceived + i.QtyReceived) >= i.QtyOrdered);
            var newStatus = fullyReceived ? PurchaseStatus.Delivered : PurchaseStatus.PartiallyDelivered;

            var update = new UpdatePurchaseCommand
            {
                Id = Purchase.Id ?? Guid.Empty,
                SupplierId = Purchase.SupplierId,
                PurchaseDate = Purchase.PurchaseDate,
                Status = newStatus,
                DeliveryAddress = Purchase.DeliveryAddress ?? string.Empty
            };

            try
            {
                await ApiClient.UpdatePurchaseEndpointAsync("1", update.Id, update);
                Snackbar.Add("Goods receipt recorded. Purchase status updated.", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (ApiException ex)
            {
                Snackbar.Add($"Error recording goods receipt: {ex.Message}", Severity.Error);
            }
        }

        private void Cancel() => MudDialog.Cancel();
    }
}

