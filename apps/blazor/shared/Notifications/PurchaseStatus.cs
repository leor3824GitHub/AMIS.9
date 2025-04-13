namespace AMIS.Blazor.Shared.Notifications;
public enum Status
{
    Draft,          //The purchase order has been created but is not yet finalized. It may still be edited.
    Submitted,      //The purchase order has been sent to the supplier or vendor but has not yet been acknowledged or accepted.
    Approved,       //The purchase order has been reviewed and authorized by the appropriate personnel within the organization.
    Acknowledged,   //The supplier has received and confirmed the purchase order.
    InProgress,     //The supplier is processing the order, but it has not yet been shipped.
    Shipped,        //The goods or services ordered have been dispatched by the supplier.
    PartiallyDelivered, //Only some of the items in the purchase order have been delivered.
    Delivered,      //All items in the purchase order have been successfully delivered.
    Closed,         //The purchase order is fully completed, and the transaction is finished. This status usually follows delivery and invoicing.
    Cancelled,      //The purchase order has been voided before completion. This could happen due to changes in needs or issues with the supplier.
    Pending         //The purchase order is waiting for further action, such as approval or payment.
}
