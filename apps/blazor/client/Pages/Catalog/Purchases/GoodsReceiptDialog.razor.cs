using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Blazor.Shared.Purchases;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMIS.Blazor.Client.Pages.Catalog.Purchases
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
            // Endpoint not yet implemented server-side; placeholder success.
            Snackbar.Add("Goods receipt captured (placeholder).", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
            await Task.CompletedTask;
        }

        private void Cancel() => MudDialog.Cancel();
    }
}
