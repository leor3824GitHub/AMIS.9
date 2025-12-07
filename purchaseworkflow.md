# Asset Procurement & Inventory Workflow

This document outlines the end-to-end workflow for procuring new assets, from the initial purchase order to their final acceptance into the asset management inventory.

## 1. Workflow Overview

The entire process follows a clear status-based progression:

`[PO Created]` → `[Items Delivered]` → `[Pending Inspection]` → `[Inspection Complete]` → `[In-Inventory]`

---

## 2. Step 1: 🛍️ Purchase Order (PO) Creation

This step initiates the procurement process.

* **Actor:** Purchasing Manager or Staff
* **Goal:** To create a formal, trackable order for goods from a supplier.
* **Key UI Components:**
    * **Header:**
        * `PO Number`: Auto-generated (e.g., `PO-2025-00123`).
        * `Date`: Auto-filled.
        * `Status`: `Draft`
    * **Supplier Details:**
        * Searchable supplier database.
        * Auto-fills address, contact, and payment terms.
    * **Delivery Address:** Dropdown of company locations.
    * **Line Items Table:**
        * `Item`: Searchable from asset catalog.
        * `Description`: Auto-filled, editable.
        * `Quantity (Qty)`: User input.
        * `Unit Price`: Auto-filled, editable.
        * `Total`: Auto-calculated.
    * **Summary:** Subtotal, Tax, Shipping, Grand Total.
    * **Action Buttons:** `[Save as Draft]`, `[Submit for Approval]`

* **Trigger for Next Step:**
    * The PO is approved and the `[Issue PO]` button is clicked.
* **System Outcome:**
    * PO `Status` changes from `Draft` to `Issued`.
    * The PO is sent to the supplier (email integration).
    * The system now expects a delivery against this PO.

---

## 3. Step 2: 🚚 Goods Receipt (Delivery)

This step is performed when the physical items arrive at the warehouse or receiving dock.

* **Actor:** Warehouse Manager or Receiving Staff
* **Goal:** To record what was *actually* received versus what was *ordered*.
* **Key UI Components:**
    * **Find PO:** User must select the `Purchase Order No.` this delivery is for.
    * **Delivery Details:**
        * `Supplier`: Auto-filled from PO.
        * `Delivery Note #`: User input (from supplier's packing slip).
        * `Delivery Date`: User input.
    * **Received Items Table:**
        * This table is auto-populated from the selected PO.
        * `Item`: Read-only.
        * `Qty Ordered`: Read-only (e.g., `10`).
        * `Qty Previously Received`: Read-only (e.g., `4`).
        * `Quantity Received`: **Key user input** (e.g., `6`).
        * `Condition`: Dropdown (`Good`, `Damaged`, `Incorrect`).
    * **Action Buttons:** `[Save as Partial Receipt]`, `[Submit & Send to Inspection]`

* **Trigger for Next Step:**
    * User clicks `[Submit & Send to Inspection]`.
* **System Outcome:**
    * A new "Goods Receipt Note" (GRN) record is created.
    * The PO `Status` is updated (e.g., to `Partially Received` or `Received`).
    * **A new task is created in the "Inspection" queue.**

---

## 4. Step 3: 🔬 Inspection & Acceptance

This step is performed by a Quality Assurance (QA) team to verify the received items meet specifications.

* **Actor:** Quality Assurance (QA) Manager or Inspector
* **Goal:** To formally accept or reject the items delivered by the supplier.
* **Key UI Components:**
    * **Header Info (Read-only):**
        * Linked `Source PO` and `Goods Receipt Note`.
        * `Supplier` name.
        * `Inspected By`: Dropdown of users.
    * **Inspection Checklist (Table):**
        * Auto-populated from the Goods Receipt Note.
        * `Item`: Read-only.
        * `Qty Received`: Read-only (e.g., `6`).
        * `Qty Accepted`: **Key user input** (defaults to Qty Received).
        * `Qty Rejected`: **Key user input** (defaults to `0`).
        * `Reason for Rejection`: Text field (required if Qty Rejected > 0).
    * **Attachments:** `[Upload Evidence]` button for photos of damaged goods.
    * **Action Buttons:** `[Reject All]`, `[Accept & Add to Inventory]`

* **Trigger for Next Step:**
    * User clicks the `[Accept & Add to Inventory]` button.
* **System Outcome:**
    * An "Inspection & Acceptance Report" (IAR) is created and marked as `Complete`.
    * This triggers the final, automated inventory update.

---

## 5. Step 4: 🤖 Automatic Inventory Update

This is a **backend, automated process** triggered by the successful completion of Step 3. No UI is required for this step.

* **Actor:** System
* **Goal:** To formally add the accepted assets to the company's inventory, update financial records, and close the loop.

* **Automated Actions:**
    1.  **Update Inventory Module:** The system takes the `Qty Accepted` for each line item and increases the "On-Hand" quantity in the inventory database.
    2.  **Generate Asset Tags:** If the items are tracked individually (e.g., laptops, machinery), the system generates new, unique asset tags (e.g., `ASSET-1001`, `ASSET-1002`, etc.) for the accepted quantity.
    3.  **Update PO Status:** If all ordered items have been received and accepted, the main PO `Status` is automatically set to `Closed`.
    4.  **Notify Finance:** The system flags the supplier's invoice as "Approved for Payment." This completes the "Three-Way Match" (PO vs. Goods Receipt vs. Invoice).