# User Guide: PH NFA COA-Compliant Inventory System

## **Overview**
This guide outlines the operational sequence for using the NFA Inventory System. It ensures full compliance with **COA Circular 2022-002**, **PPSAS 12**, and the **Updated ₱50,000 Semi-Expendable Threshold**.

---

## **Phase 1: System Configuration (Admin & Setup)**
*Before transactions begin, ensure system parameters align with current regulatory standards.*

### **1. Verify Threshold Locks**
* **Action:** Administrator must verify that the global capitalization threshold is locked at **₱50,000**.
* **Rule:** Items costing **≤ ₱50,000** are Semi-Expendable or Consumable; items **> ₱50,000** are PPE.

### **2. User Role Assignment**
* **Action:** Assign specific personnel to distinct roles to enforce the required audit trail:
    * *Requisitioner*
    * *Supply Officer*
    * *Accounting*
    * *Approving Authority*
* **Compliance:** All edits require user role authorization to maintain accountability logs (Encoded by, Approved by, Released by, Received by).

### **3. RCA Mapping Check**
* **Action:** Confirm the Chart of Accounts is mapped to the **Revised Chart of Accounts (RCA) 2019**.
* **Compliance:** The system must block posting under outdated 2015 RCA accounts.

---

## **Phase 2: Procurement & Receipt (Inflow)**
*Triggered when goods are delivered to the warehouse.*

### **Step 1: Encode Receipt Details**
* **User:** Supply Officer
* **Input:** Purchase Order (PO) Number, Delivery Receipt (DR), Stock Number, Unit of Measure, and Acquisition Cost.
* **System Logic:** The system increases stock quantity and updates the cost using the **Weighted Average Method** (FIFO is not used for valuation).

### **Step 2: Automated Classification (Decision Tree)**
The system automatically assigns the inventory category based on the encoded cost and item nature:

* **Scenario A: Consumables**
    * *Criteria:* Cost ≤ ₱50,000 + Short useful life (e.g., paper, toner).
    * *Record:* **Supplies Ledger Card (SLC)**.
    * *Account:* Supplies and Materials Inventory (10501000).

* **Scenario B: Semi-Expendables**
    * *Criteria:* Cost ≤ ₱50,000 + Durable (e.g., calculator, chair).
    * *Record:* **Semi-Expendable Property Inventory**.
    * *Account:* Semi-Expendable Property Inventory (10599020).

* **Scenario C: PPE**
    * *Criteria:* Cost > ₱50,000 (e.g., vehicle, heavy machinery).
    * *Record:* **Property, Plant and Equipment (PPE)** Ledger.
    * *Account:* PPE Asset Account (1-06-xx-xxx).

### **Step 3: Generate Receipt JEV**
* **User:** Accounting
* **Output:** The system auto-generates a **Journal Entry Voucher (JEV)**.
* **Entry:** Dr: *Inventory Account*, Cr: *Accounts Payable/Cash*.
* **Note:** The system ensures the JEV uses the correct RCA 2019 codes.

---

## **Phase 3: Requisition & Issuance (Outflow)**
*Triggered when an NFA employee requests items.*

### **Step 4: Requesting Supplies**
* **User:** Requisitioner
* **Action:** File a **Requisition and Issue Slip (RIS)** digitally within the system.

### **Step 5: Approval & Stock Check**
* **User:** Supply Officer
* **Action:** Review RIS. System verifies **Stock Availability**.
* **Constraint:** System blocks issuance if the specific custodian has unsettled accountabilities (for semi-expendables/PPE).

### **Step 6: Document Generation (Issuance)**
Upon approval, the system generates the mandatory COA form based on the item classification:

* **For Consumables:** Generates **RSMI** (Report of Supplies and Materials Issued).
* **For Semi-Expendables (≤ ₱50k):** Generates **ICS** (Inventory Custodian Slip).
* **For PPE (> ₱50k):** Generates **PAR** (Property Acknowledgment Receipt).

### **Step 7: Recording the Expense**
* **System Action:** Automatically updates the Stock Card and SLC.
* **Accounting Entry:** Generates a JEV recognizing the expense.
    * *Example (Consumable):* Dr: Supplies Expense (50203010), Cr: Inventory (10501000).
    * *Example (Semi-Ex):* Dr: Semi-Expendable Expense (50299010), Cr: Inventory (10599020).

---

## **Phase 4: Reporting & Audit (Compliance)**
*Required for monthly and annual statutory requirements.*

### **Step 8: Monthly Reporting**
* **User:** Supply Officer / Accounting
* **Output:** Export the **RSMI** and **Monthly Inventory Summary** for submission to the COA Auditor.
* **Format:** Reports are available in PDF or XML/JSON for GAS-COA integration.

### **Step 9: Disposal (Waste Management)**
* **Scenario:** Item is broken, obsolete, or consumed.
* **Action:** Process a return/disposal transaction.
* **Output:** System generates a **Waste Materials Report (WMR)**.
* **Effect:** Accountability is cleared from the user, and the item is removed from active Stock Cards.

### **Step 10: Year-End Audit**
* **User:** Accounting
* **Output:** Generate the **Year-End Inventory Report**.
* **FS Automation:** The system auto-generates a "Notes to Financial Statements" disclosure confirming:
    1.  Adherence to the **₱50,000 PPE threshold**.
    2.  Compliance with **COA Circular 2022-002** (RCA Conversion).

---

## **Quick Reference: Document Requirements**

| Item Type | Cost Threshold | Receipt Doc | Issuance Doc | Inventory Record |
| :--- | :--- | :--- | :--- | :--- |
| **Consumable** | Any (usually low) | PO / DR | **RSMI** | SLC / Stock Card |
| **Semi-Expendable** | ≤ ₱50,000 | PO / DR | **ICS** | Semi-Ex Inventory |
| **PPE** | > ₱50,000 | PO / DR | **PAR** | PPE Ledger |

*Reference: [readme_and_copilot_instructions.md]*