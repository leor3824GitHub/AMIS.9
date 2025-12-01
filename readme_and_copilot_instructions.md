# **README.md – PH NFA COA‑Compliant Inventory System (Consumables & Office Supplies)**

## **Overview**
This system is designed to support the National Food Authority (NFA) in managing consumables and office supplies in full compliance with:
- **Commission on Audit (COA) Circulars** (e.g., COA Circular 2012‑001, 2022‑004, and all relevant audit guidelines)
- **Department of Budget and Management (DBM) Issuances** (e.g., DBM Budget Circulars on Inventory and Supplies)
- **Government Procurement Policy Board (GPPB) Guidelines**
- **NFA Internal SOPs and Manuals** (e.g., Property/Asset Management, Procurement, and Warehouse Operations)
- **Philippine Government Accounting Standards (PGAS)**
- **Philippine Public Sector Accounting Standards (PPSAS)**, particularly PPSAS 12 (Inventories)

This repository contains the implementation notes, coding standards, and system behavior that ensure compliance with these laws and guidelines.

---

## **1. System Objectives**
- Ensure **accurate and real‑time tracking** of NFA consumables and office supplies.
- Support **COA‑compliant recording and reporting**, including: 
  - Supplies Ledger Cards (SLC)
  - Stock Cards
  - Report of Supplies and Materials Issued (RSMI)
  - Waste Materials Report
  - Inventory Custodian Slip (ICS) and Property Acknowledgement Receipt (PAR) (for semi‑expendables)
- Maintain **audit trails** for accountability and monitoring.
- Automate **document generation** consistent with required forms.
- Align with **NFA SOPs on Inventory Management**.

---

## **2. Legal and Regulatory Compliance**
### **2.1 COA Circulars and Guidelines Integrated**
- **COA Circular 2012‑001** – Accounting Guidelines on the Use of Inventory Accounts.
- **COA Circular 2022‑004** – Guidelines for Semi‑Expendable Property (₱1,000–₱14,999).
- **COA Circular 2005‑002** – Guidelines on Documentary Requirements.
- **COA Audit Templates** – Using SLC, ICS, PAR, RSMI, Waste Material Report.

### **2.2 DBM Compliance**
- **DBM Budget Circulars** on inventory management and reporting.
- **Annual PFO (Physical Financial Reports)** requirements.
- **PSAS and PPSAS** regarding recognition and measurement of supplies.

### **2.3 NFA SOP Alignment**
- SOP on **Procurement of Supplies and Materials**.
- SOP on **Warehouse Management and Issuances**.
- SOP on **Property and Supply Management**.

The system enforces **FIFO**, unit cost monitoring, and mandatory documentary requirements for every transaction.

---

## **3. Core Features**
### **3.1 Inventory Tracking**
- Tracking of consumables and office supplies by: 
  - Stock Number
  - Unit of Measure
  - Beginning Balance
  - Receipts
  - Issuances
  - Ending Balance
- Automatic ledger generation for SLC and Stock Cards.

### **3.2 COA‑Compliant Records and Reports**
The system automatically produces:
- **SLC (Supplies Ledger Card)**
- **Stock Card** (Warehouse)
- **RSMI (Report of Supplies and Materials Issued)**
- **ICS/PAR** (if semi‑expendable)
- **Waste Materials Report**
- **Inventory Reports for Year‑End COA Audit**
- **Monthly Inventory Summary for Accounting Unit**

### **3.3 Accounting Integration**
Supports required recognition under **PPSAS 12**:
- Inventory is recorded at **cost**.
- Issuance uses **Weighted Average Method** (for consistency with COA).
- Regular posting to:
  - **Inventory – Supplies**
  - **Inventory – Semi‑Expendables**
  - **Inventory – Office Supplies Issued** (expense)

### **3.4 Audit Trail and Accountability**
- Logs: Encoded by, Approved by, Released by, Received by.
- All edits require user role authorization.
- Auto‑generated tracking reference numbers per NFA SOP.

---

## **4. Document Flow (NFA‑Compliant)**
1. **Requisitioner files RIS (Requisition and Issue Slip).**
2. **Supply Officer checks stock availability.**
3. **ICS/PAR is generated** for semi‑expendable supplies.
4. **Issuance recorded** in Stock Card and SLC.
5. **RSMI generated** and submitted to Accounting.
6. **Monthly abstract** forwarded to COA Auditor.

---

## **5. Technical Implementation Notes**
- Clean Architecture using **AMIS.Framework.Core.Domain**.
- Entities include:
  - `SupplyItem`, `StockLedger`, `Issuance`, `Receipt`, `WasteMaterialReport`.
- Domain events to ensure audit trails.
- Fully typed IDs and value objects.
- Optional offline‑capable computation.

---

# **copilot-instructions.md**
These instructions guide GitHub Copilot when assisting development for this NFA‑COA‑compliant inventory system.

## **1. Coding Standards**
- Use **C# (.NET)** with Clean Architecture layering.
- Always follow NFA SOP naming conventions.
- Always generate domain events for stock movements.
- Respect required accounting treatments under **PPSAS 12**.

---

## **2. Domain Rules For Copilot**
### **2.1 Receipts**
- Increase stock quantity.
- Update Weighted Average Cost.
- Log reference to **PO/PR No.**

### **2.2 Issuances**
- Use Weighted Average Cost.
- Automatically generate **RSMI** data.

### **2.3 Semi‑Expendables**
If unit cost is between **₱1,000–₱14,999**:
- Create ICS or PAR.
- Record under **Inventory – Semi‑Expendable**.

### **2.4 Waste or Disposal**
- Disposal transactions generate **Waste Materials Report**.
- Update SLC and Stock Cards accordingly.

---

## **3. Required Output Templates**
Copilot should be able to generate:
- SLC
- Stock Card
- RSMI
- ICS
- PAR
- Year‑End Inventory Report

All formats must follow **COA and NFA templates**.

---

## **4. Prohibited Actions for Copilot**
- Do not generate journal entries inconsistent with PPSAS.
- Do not assume FIFO; default is **Weighted Average**.
- Do not modify COA‑prescribed forms.

---

## **5. Goal for Copilot**
Ensure all code generated:
- Is audit‑ready
- Matches COA/NFA documentation workflows
- Supports year‑end accounting and audit trails

---

If you want, I can also generate:
- System flowcharts
- Database schema with COA‑aligned fields
- Auto‑generated form templates (ICS, PAR, RSMI)
- A full SOP rewritten for your exact NFA office


## COA Circular No. 2022-002 Compliance Integration

This system is further enhanced to comply with **COA Circular No. 2022-002 (January 24, 2022)** regarding the **conversion and proper use of Revised Chart of Accounts (RCA) 2019**. Key compliance points applied:

### Alignment with RCA (Updated 2019)
- All consumables, semi-expendable property, office supplies, and MRO items use **updated 2019 RCA account codes**.
- Inventory, issuance, disposal, and reporting modules are mapped to the **Matrix on the Conversion of Accounts (Annex A)**.
- System enforces proper classification under:
  - *Inventories* (Supplies and Materials Inventory, Semi-Expendable (≤ ₱50,000 as per latest COA/DBM guidance) Inventory)
  - *Expenses* (Supplies and Materials Expense)
  - *Property, Plant and Equipment* (if item exceeds capitalization threshold)

### Journal Entry Voucher (JEV) Compliance
- System auto-generates **COA-compliant JEV templates** following Annex B:
  - Conversion entries
  - Transfer balances
  - Realignment of accounts
- Every transaction (receipt, issuance, consumption, disposal) produces a **linked JEV entry** referencing:
  - Document number (ICS, PAR, RSMI, Waste Material Report, etc.)
  - Accountable officer
  - Updated RCA account code

### Required Documentation & Submission
- All JEVs have options to export:
  - PDF copy for COA Auditor
  - XML/JSON export for GAS-COA
- System maintains a **conversion log** with:
  - Date of mapping
  - RCA version
  - Account transitions
  - Notes to FS requirements

### Revised/Modified Accounts Incorporated
- Trust Liabilities – Disallowances/Charges (20401080) reflected in liability workflows.
- Scholarship Grants/Expenses (50202020) properly categorized for educational/training-related issuances.
- Development in Progress – Copyrights (10898040) available for agencies with intellectual property creation.
- Updated long-form account titles for inter-agency payables.

### Enforcement of Proper Account Use
To prevent misuse of accounts as emphasized by COA:
- System blocks posting under outdated 2015 RCA accounts.
- Validation rules prohibit incorrect classification of:
  - Capitalizable vs. semi-expendable vs. consumable
  - Trust liabilities vs. regular liabilities
  - Inventory vs. expense recognition

### Notes to Financial Statements Automation
The system includes:
- Auto-generated disclosure: *“The Agency completed the conversion from the 2015 RCA to the 2019 RCA as prescribed in COA Circular 2022-002 on [date].”*
- Reference attachment to the conversion JEV.

This ensures full readiness for COA post-audit and annual financial statement preparation.


## Updated Property Classification Rules (COA/DBM 2023–2025 Standards)

To fully comply with the latest COA and DBM issuances, including the updated **₱50,000 threshold for Semi‑Expendable Property**, the system now implements the following classification framework:

---

# **1. Classification Decision Tree (Automated in System)**

The system applies the following logic whenever an item is encoded, purchased, received, or issued:

### **Step 1 — Determine Acquisition Cost**
- **≤ ₱50,000** → Go to Step 2
- **> ₱50,000** → Automatically classified as **Property, Plant and Equipment (PPE)**

### **Step 2 — Nature of Item**
- **Consumable/short‑life items** (used within 1 year) → Classified as **Consumables / Office Supplies**
- **Non‑consumable item with useful life > 1 year** → Classified as **Semi‑Expendable Property**

### **Step 3 — Issue Document Requirement**
- **Consumables** → RSMI (Requisition and Issue Slip)
- **Semi‑Expendables (≤ ₱50,000)** → ICS (Inventory Custodian Slip)
- **PPE (> ₱50,000)** → PAR (Property Acknowledgment Receipt)

---

# **2. Updated Classification Table (As Required by COA)**

| Cost | Type of Item | Classification | Required Document | Relevant Account Code (RCA 2019) |
|------|--------------|----------------|-------------------|----------------------------------|
| **≤ ₱50,000** | Consumable, used within a year | Office/Consumable Supplies | RSMI | **Supplies and Materials Inventory (10501000)** then **Supplies and Materials Expense (50203010)** upon issuance |
| **≤ ₱50,000** | Non‑consumable with >1 year useful life | **Semi‑Expendable Property** | **ICS** | **Semi‑Expendable Property Inventory (10599020)** → Semi‑Expendable Property Expense (50299010) upon issuance |
| **> ₱50,000** | Durable, useful life >1 year | PPE | **PAR** | PPE accounts under **1-06-*** depending on type (e.g., Machinery, ICT Equipment, Furniture & Fixtures) |

All classification is **automatic** and enforced through validation rules.

---

# **3. Updated JEV Posting Templates (Automated)**

### **A. Consumables (Office Supplies)**
**Upon Receipt:**
> Dr: Supplies and Materials Inventory (10501000)
> Cr: Accounts Payable / Cash

**Upon Issuance:**
> Dr: Supplies and Materials Expense (50203010)
> Cr: Supplies and Materials Inventory (10501000)

---

### **B. Semi‑Expendable Property (≤ ₱50,000)**
**Upon Receipt:**
> Dr: Semi‑Expendable Property Inventory (10599020)
> Cr: Accounts Payable / Cash

**Upon Issuance (ICS):**
> Dr: Semi‑Expendable Property Expense (50299010)
> Cr: Semi‑Expendable Property Inventory (10599020)

ICS is generated automatically and tied to the JEV.

---

### **C. Property, Plant and Equipment (> ₱50,000)**
**Upon Receipt and Issuance of PAR:**
> Dr: PPE Asset Account (1-06-xx-xxx)
> Cr: Accounts Payable / Cash

Depreciation entries auto-generated per PPSAS.

---

# **4. Updated Workflow Integration**

### **Procurement → Inventory → Issuance Pathways**
System now implements three compliant flows:

#### **A. Consumables Flow**
PO → DR/GRN → Supplies Inventory → RSMI → Expense Recognition

#### **B. Semi‑Expendable Flow (≤ ₱50,000)**
PO → DR/GRN → Semi‑Expendable Inventory → ICS Issuance → Expense Recognition

#### **C. PPE Flow (> ₱50,000)**
PO → DR/GRN → PPE Recognition → PAR Assignment → Depreciation

Each path has:  
✔ Correct RCA 2019 account mapping  
✔ Correct COA form (RSMI, ICS, PAR)  
✔ Correct audit logs + JEV generation

---

# **5. Enforcement Controls Added**
To ensure COA audit readiness:

- Items **cannot be misclassified** (e.g., PPE listed as semi-expendable)  
- Threshold is **system-locked at ₱50,000**, editable only by authorized accounting officials  
- PPE cannot be issued using ICS or RSMI  
- Semi‑Expendables cannot be directly expensed upon receipt  
- Automated reminders if:
  - ICS custodian has pending accountability
  - PPE issued without PAR
  - Supplies inventory balance goes negative

---

# **6. Notes to Financial Statements (FS) Automation**
System auto-generates disclosures:

- Confirmation that classification follows latest COA/DBM thresholds  
- PPE capitalization policy referencing ₱50,000 rule  
- Semi‑expendable policy under COA Circulars
- Inventory valuation and expense recognition policies under PPSAS

---

All updates now fully integrate the **₱50,000 Semi‑Expendable Property threshold** and reinforce COA, DBM, PPSAS, and NFA SOP compliance.

