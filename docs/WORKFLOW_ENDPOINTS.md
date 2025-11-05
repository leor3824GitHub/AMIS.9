# Catalog Workflow Endpoints Documentation

This document provides comprehensive documentation for all 37 workflow endpoints in the Catalog module, organized by entity type.

## Table of Contents
- [Purchase Workflows (15 endpoints)](#purchase-workflows)
- [Inventory Workflows (10 endpoints)](#inventory-workflows)
- [Inspection Workflows (10 endpoints)](#inspection-workflows)
- [InspectionRequest Workflows (2 endpoints)](#inspectionrequest-workflows)

---

## Purchase Workflows

### 1. Submit Purchase for Approval
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/submit-for-approval`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `SubmitPurchaseForApprovalHandler`

**Description**: Submits a draft purchase order for approval, transitioning it to PendingApproval status.

**Request**:
```json
{
  "purchaseId": "guid"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "PendingApproval",
  "submittedAt": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in Draft status
- Validates purchase has required information
- Transitions status: Draft → PendingApproval
- Records submission timestamp

---

### 2. Approve Purchase
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/approve`  
**Permission**: `Permissions.Purchases.Approve`  
**Handler**: `ApprovePurchaseHandler`

**Description**: Approves a pending purchase order, allowing it to proceed to fulfillment.

**Request**:
```json
{
  "purchaseId": "guid",
  "approvedBy": "string",
  "notes": "string"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Approved",
  "approvedAt": "datetime",
  "approvedBy": "string",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in PendingApproval status
- Records approval timestamp and approver
- Transitions status: PendingApproval → Approved
- Triggers notification to supplier

---

### 3. Reject Purchase
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/reject`  
**Permission**: `Permissions.Purchases.Approve`  
**Handler**: `RejectPurchaseHandler`

**Description**: Rejects a pending purchase order with a reason.

**Request**:
```json
{
  "purchaseId": "guid",
  "reason": "string"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Rejected",
  "rejectedAt": "datetime",
  "reason": "string",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in PendingApproval status
- Reason is required
- Transitions status: PendingApproval → Rejected
- Records rejection timestamp and reason

---

### 4. Acknowledge Purchase
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/acknowledge`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `AcknowledgePurchaseHandler`

**Description**: Acknowledges receipt of an approved purchase order by the supplier.

**Request**:
```json
{
  "purchaseId": "guid",
  "acknowledgedBy": "string",
  "estimatedDeliveryDate": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Acknowledged",
  "acknowledgedAt": "datetime",
  "estimatedDeliveryDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in Approved status
- Records acknowledgment timestamp
- Transitions status: Approved → Acknowledged
- Updates estimated delivery date

---

### 5. Mark Purchase In Progress
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-in-progress`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkInProgressHandler`

**Description**: Marks an acknowledged purchase as in progress (being fulfilled).

**Request**:
```json
{
  "purchaseId": "guid",
  "startedAt": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "InProgress",
  "startedAt": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in Acknowledged status
- Records start timestamp
- Transitions status: Acknowledged → InProgress

---

### 6. Mark Purchase Shipped
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-shipped`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkShippedHandler`

**Description**: Marks a purchase as shipped with tracking information.

**Request**:
```json
{
  "purchaseId": "guid",
  "shippedDate": "datetime",
  "trackingNumber": "string",
  "carrier": "string"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Shipped",
  "shippedDate": "datetime",
  "trackingNumber": "string",
  "carrier": "string",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in InProgress status
- Records shipping details
- Transitions status: InProgress → Shipped
- Triggers shipment tracking notification

---

### 7. Mark Purchase Partially Received
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-partially-received`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkPartiallyReceivedHandler`

**Description**: Records partial receipt of a purchase order.

**Request**:
```json
{
  "purchaseId": "guid",
  "receivedDate": "datetime",
  "receivedItems": [
    {
      "itemId": "guid",
      "quantityReceived": "decimal"
    }
  ]
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "PartiallyReceived",
  "receivedDate": "datetime",
  "totalItemsReceived": "int",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in Shipped or Approved status
- Validates received quantities don't exceed ordered
- Transitions status: Shipped/Approved → PartiallyReceived
- Updates item receipt records

---

### 8. Mark Purchase Fully Received
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-fully-received`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkFullyReceivedHandler`

**Description**: Marks all items in a purchase as received.

**Request**:
```json
{
  "purchaseId": "guid",
  "receivedDate": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "FullyReceived",
  "receivedDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Validates all purchase items have been accepted
- Transitions to FullyReceived status
- Triggers invoice generation process

---

### 9. Mark Purchase Pending Invoice
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-pending-invoice`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkPendingInvoiceHandler`

**Description**: Marks a fully received purchase as awaiting invoice.

**Request**:
```json
{
  "purchaseId": "guid"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "PendingInvoice",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in FullyReceived status
- Transitions status: FullyReceived → PendingInvoice
- Triggers accounting notification

---

### 10. Mark Purchase Invoiced
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-invoiced`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkInvoicedHandler`

**Description**: Records invoice receipt for a purchase.

**Request**:
```json
{
  "purchaseId": "guid",
  "invoiceNumber": "string",
  "invoiceDate": "datetime",
  "invoiceAmount": "decimal"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Invoiced",
  "invoiceNumber": "string",
  "invoiceDate": "datetime",
  "invoiceAmount": "decimal",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in PendingInvoice status
- Records invoice details
- Transitions status: PendingInvoice → Invoiced

---

### 11. Mark Purchase Pending Payment
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-pending-payment`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkPendingPaymentHandler`

**Description**: Marks an invoiced purchase as awaiting payment.

**Request**:
```json
{
  "purchaseId": "guid"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "PendingPayment",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in Invoiced status
- Transitions status: Invoiced → PendingPayment
- Triggers payment processing workflow

---

### 12. Mark Purchase Closed
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/mark-closed`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `MarkClosedHandler`

**Description**: Closes a completed purchase order.

**Request**:
```json
{
  "purchaseId": "guid",
  "closedDate": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Closed",
  "closedDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in Invoiced or PendingPayment status
- Records closure date
- Transitions status: Invoiced/PendingPayment → Closed
- Archives purchase record

---

### 13. Put Purchase On Hold
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/put-on-hold`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `PutPurchaseOnHoldHandler`

**Description**: Places a purchase order on hold with a reason.

**Request**:
```json
{
  "purchaseId": "guid",
  "reason": "string",
  "holdDate": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "OnHold",
  "reason": "string",
  "holdDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Can be applied from any active status
- Records hold reason and date
- Transitions to OnHold status
- Suspends all automated workflows

---

### 14. Release Purchase From Hold
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/release-from-hold`  
**Permission**: `Permissions.Purchases.Edit`  
**Handler**: `ReleasePurchaseFromHoldHandler`

**Description**: Releases a purchase from hold status.

**Request**:
```json
{
  "purchaseId": "guid",
  "releaseDate": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Draft",
  "releaseDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Purchase must be in OnHold status
- Transitions status: OnHold → Draft
- Resumes normal workflow processing

---

### 15. Cancel Purchase
**Endpoint**: `POST /api/v1/catalog/purchases/{id}/cancel`  
**Permission**: `Permissions.Purchases.Delete`  
**Handler**: `CancelPurchaseHandler`

**Description**: Cancels a purchase order with a reason.

**Request**:
```json
{
  "purchaseId": "guid",
  "reason": "string",
  "cancelledDate": "datetime"
}
```

**Response**:
```json
{
  "purchaseId": "guid",
  "status": "Cancelled",
  "reason": "string",
  "cancelledDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Can cancel from Draft, PendingApproval, or Approved status
- Reason is required
- Transitions to Cancelled status
- Releases any reserved inventory
- Notifies supplier if already approved

---

## Inventory Workflows

### 1. Reserve Stock
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/reserve-stock`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `ReserveStockHandler`

**Description**: Reserves inventory quantity for orders or production.

**Request**:
```json
{
  "inventoryId": "guid",
  "quantity": "decimal",
  "reservedFor": "string",
  "reservationDate": "datetime"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "reservedQty": "decimal",
  "availableQty": "decimal",
  "message": "string"
}
```

**Business Logic**:
- Validates sufficient available quantity
- Increases reserved quantity
- Decreases available quantity
- Records reservation details
- Status must be Available

---

### 2. Release Reservation
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/release-reservation`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `ReleaseReservationHandler`

**Description**: Releases previously reserved inventory.

**Request**:
```json
{
  "inventoryId": "guid",
  "quantity": "decimal"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "releasedQty": "decimal",
  "reservedQty": "decimal",
  "availableQty": "decimal",
  "message": "string"
}
```

**Business Logic**:
- Validates quantity doesn't exceed reserved
- Decreases reserved quantity
- Increases available quantity
- Updates inventory availability

---

### 3. Quarantine Inventory
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/quarantine`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `QuarantineInventoryHandler`

**Description**: Places inventory in quarantine status.

**Request**:
```json
{
  "inventoryId": "guid",
  "location": "string",
  "reason": "string"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "stockStatus": "Quarantined",
  "location": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets status to Quarantined
- Updates location to quarantine area
- Makes inventory unavailable for allocation
- Records quarantine reason

---

### 4. Release From Quarantine
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/release-from-quarantine`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `ReleaseFromQuarantineHandler`

**Description**: Releases inventory from quarantine back to available status.

**Request**:
```json
{
  "inventoryId": "guid"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "stockStatus": "Available",
  "message": "string"
}
```

**Business Logic**:
- Inventory must be in Quarantined status
- Transitions status: Quarantined → Available
- Makes inventory available for allocation

---

### 5. Mark As Damaged
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/mark-damaged`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `MarkAsDamagedHandler`

**Description**: Marks inventory as damaged.

**Request**:
```json
{
  "inventoryId": "guid",
  "damageDescription": "string",
  "damageDate": "datetime"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "stockStatus": "Damaged",
  "message": "string"
}
```

**Business Logic**:
- Sets status to Damaged
- Records damage details
- Reduces available quantity to zero
- Triggers damage assessment workflow

---

### 6. Mark As Obsolete
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/mark-obsolete`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `MarkAsObsoleteHandler`

**Description**: Marks inventory as obsolete.

**Request**:
```json
{
  "inventoryId": "guid",
  "reason": "string"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "stockStatus": "Obsolete",
  "message": "string"
}
```

**Business Logic**:
- Sets status to Obsolete
- Makes inventory unavailable
- Records obsolescence reason
- Triggers disposal workflow

---

### 7. Record Cycle Count
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/record-cycle-count`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `RecordCycleCountHandler`

**Description**: Records physical inventory count and adjusts system quantity.

**Request**:
```json
{
  "inventoryId": "guid",
  "countedQty": "decimal",
  "countDate": "datetime",
  "countedBy": "string"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "previousQty": "decimal",
  "countedQty": "decimal",
  "variance": "decimal",
  "stockStatus": "string",
  "countDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Records physical count
- Calculates variance (counted - system)
- Adjusts system quantity to match count
- Creates inventory adjustment transaction
- Triggers variance investigation if threshold exceeded

---

### 8. Allocate To Production
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/allocate-to-production`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `AllocateToProductionHandler`

**Description**: Allocates inventory to a production order.

**Request**:
```json
{
  "inventoryId": "guid",
  "quantity": "decimal",
  "productionOrderId": "guid"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "allocatedQty": "decimal",
  "productionOrderId": "guid",
  "message": "string"
}
```

**Business Logic**:
- Validates sufficient available quantity
- Reserves quantity for production
- Records production order reference
- Decreases available quantity

---

### 9. Set Location
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/set-location`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `SetLocationHandler`

**Description**: Updates the storage location of inventory.

**Request**:
```json
{
  "inventoryId": "guid",
  "location": "string"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "location": "string",
  "message": "string"
}
```

**Business Logic**:
- Updates inventory location
- Records location change history
- Validates location exists in system

---

### 10. Set Costing Method
**Endpoint**: `POST /api/v1/catalog/inventories/{id}/set-costing-method`  
**Permission**: `Permissions.Inventories.Edit`  
**Handler**: `SetCostingMethodHandler`

**Description**: Sets the inventory costing method (FIFO, LIFO, Average).

**Request**:
```json
{
  "inventoryId": "guid",
  "costingMethod": "FIFO|LIFO|Average"
}
```

**Response**:
```json
{
  "inventoryId": "guid",
  "costingMethod": "string",
  "message": "string"
}
```

**Business Logic**:
- Updates costing method
- Validates method is supported
- Triggers cost recalculation if changed
- Records method change history

---

## Inspection Workflows

### 1. Schedule Inspection
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/schedule`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `ScheduleInspectionHandler`

**Description**: Schedules an inspection with a planned date.

**Request**:
```json
{
  "inspectionId": "guid",
  "scheduledDate": "datetime"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "string",
  "inspectedOn": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Sets inspection scheduled date
- Updates inspection status
- Assigns inspection resources
- Sends notification to inspector

---

### 2. Quarantine Inspection
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/quarantine`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `QuarantineInspectionHandler`

**Description**: Places an inspection in quarantine status.

**Request**:
```json
{
  "inspectionId": "guid",
  "reason": "string"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets inspection to quarantine
- Records quarantine reason
- Prevents inspection completion
- Triggers escalation workflow

---

### 3. Conditionally Approve
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/conditionally-approve`  
**Permission**: `Permissions.Inspections.Approve`  
**Handler**: `ConditionallyApproveHandler`

**Description**: Approves inspection with conditions.

**Request**:
```json
{
  "inspectionId": "guid",
  "conditions": "string"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "ConditionallyApproved",
  "conditions": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets status to ConditionallyApproved
- Records approval conditions
- Requires condition fulfillment before final approval
- Notifies responsible party of conditions

---

### 4. Require Re-Inspection
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/require-re-inspection`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `RequireReInspectionHandler`

**Description**: Flags inspection for re-inspection.

**Request**:
```json
{
  "inspectionId": "guid",
  "reason": "string"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "RequiresReInspection",
  "reason": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets status to RequiresReInspection
- Records re-inspection reason
- Schedules new inspection
- Notifies quality team

---

### 5. Put Inspection On Hold
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/put-on-hold`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `PutInspectionOnHoldHandler`

**Description**: Places inspection on hold.

**Request**:
```json
{
  "inspectionId": "guid",
  "reason": "string"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "OnHold",
  "reason": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets status to OnHold
- Records hold reason
- Suspends inspection activities
- Triggers hold notification

---

### 6. Release Inspection From Hold
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/release-from-hold`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `ReleaseInspectionFromHoldHandler`

**Description**: Releases inspection from hold status.

**Request**:
```json
{
  "inspectionId": "guid"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "InProgress",
  "message": "string"
}
```

**Business Logic**:
- Inspection must be in OnHold status
- Transitions status: OnHold → InProgress
- Resumes inspection activities

---

### 7. Partially Approve
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/partially-approve`  
**Permission**: `Permissions.Inspections.Approve`  
**Handler**: `PartiallyApproveHandler`

**Description**: Partially approves inspection results.

**Request**:
```json
{
  "inspectionId": "guid",
  "partialDetails": "string"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "PartiallyApproved",
  "partialDetails": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets status to PartiallyApproved
- Records which items/criteria are approved
- Identifies items requiring additional inspection
- Updates inventory for approved items

---

### 8. Complete Inspection
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/complete`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `CompleteInspectionHandler`

**Description**: Marks inspection as complete.

**Request**:
```json
{
  "inspectionId": "guid",
  "completedDate": "datetime"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "Completed",
  "completedDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Validates all inspection criteria are met
- Sets status to Completed
- Records completion date
- Triggers final approval workflow
- Releases inspected items

---

### 9. Release Inspection From Quarantine
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/release-from-quarantine`  
**Permission**: `Permissions.Inspections.Edit`  
**Handler**: `ReleaseInspectionFromQuarantineHandler`

**Description**: Releases inspection from quarantine.

**Request**:
```json
{
  "inspectionId": "guid"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "InProgress",
  "message": "string"
}
```

**Business Logic**:
- Inspection must be in quarantine
- Transitions to InProgress status
- Allows inspection to continue
- Records release timestamp

---

### 10. Reject Inspection (Batch 1)
**Endpoint**: `POST /api/v1/catalog/inspections/{id}/reject`  
**Permission**: `Permissions.Inspections.Approve`  
**Handler**: `RejectInspectionHandler`

**Description**: Rejects inspection results.

**Request**:
```json
{
  "inspectionId": "guid",
  "reason": "string"
}
```

**Response**:
```json
{
  "inspectionId": "guid",
  "status": "Rejected",
  "reason": "string",
  "message": "string"
}
```

**Business Logic**:
- Sets status to Rejected
- Records rejection reason
- Prevents item acceptance
- Triggers corrective action workflow

---

## InspectionRequest Workflows

### 1. Mark Inspection Request Completed
**Endpoint**: `POST /api/v1/catalog/inspection-requests/{id}/mark-completed`  
**Permission**: `Permissions.InspectionRequests.Edit`  
**Handler**: `MarkCompletedHandler`

**Description**: Marks an inspection request as completed.

**Request**:
```json
{
  "inspectionRequestId": "guid",
  "completedDate": "datetime"
}
```

**Response**:
```json
{
  "inspectionRequestId": "guid",
  "status": "Completed",
  "completedDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Sets status to Completed
- Records completion date
- Closes inspection request
- Triggers notification to requestor

---

### 2. Mark Inspection Request Accepted
**Endpoint**: `POST /api/v1/catalog/inspection-requests/{id}/mark-accepted`  
**Permission**: `Permissions.InspectionRequests.Edit`  
**Handler**: `MarkAcceptedHandler`

**Description**: Marks an inspection request as accepted.

**Request**:
```json
{
  "inspectionRequestId": "guid",
  "acceptedDate": "datetime"
}
```

**Response**:
```json
{
  "inspectionRequestId": "guid",
  "status": "Accepted",
  "acceptedDate": "datetime",
  "message": "string"
}
```

**Business Logic**:
- Sets status to Accepted
- Records acceptance date
- Creates inspection record
- Assigns inspector

---

## Common Response Codes

All endpoints follow standard HTTP response codes:

- **200 OK**: Successful operation
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data or business rule violation
- **401 Unauthorized**: Missing or invalid authentication
- **403 Forbidden**: User lacks required permissions
- **404 Not Found**: Entity not found
- **500 Internal Server Error**: Unexpected server error

## Error Response Format

```json
{
  "type": "string",
  "title": "string",
  "status": 400,
  "detail": "string",
  "errors": {
    "fieldName": ["error message"]
  }
}
```

## API Versioning

All endpoints are versioned using URL path versioning (e.g., `/api/v1/`). The current version is **v1**.

## Authentication & Authorization

All endpoints require:
1. Valid JWT authentication token
2. Appropriate permission as specified for each endpoint
3. Multi-tenant context (tenant ID in request header)

## Rate Limiting

API endpoints are subject to rate limiting:
- 100 requests per minute per user
- 1000 requests per minute per tenant

## Audit Logging

All workflow operations are automatically logged with:
- User identity
- Timestamp
- Action performed
- Entity ID
- Previous and new state
- Tenant context
