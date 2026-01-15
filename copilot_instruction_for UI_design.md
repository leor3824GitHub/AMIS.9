# UI Navigation & Logic Flow

This document outlines the user journey through the Asset Management System (AMIS) frontend. It highlights the critical decision points where Philippine Government compliance logic (COA/DBM rules) interacts with the user interface.

## Flowchart Diagram

```mermaid
graph TD
    %% Entry Point
    Start((Start)) --> Login[Login Page]
    Login --> RoleCheck{User Role?}

    %% Supply Officer Dashboard
    RoleCheck -- Supply Officer --> DashSupply[Supply Dashboard]
    
    %% Accountant Dashboard
    RoleCheck -- Accountant --> DashAcct[Accounting Dashboard]
    
    %% End User Dashboard
    RoleCheck -- End User --> DashUser[My Accountability Dashboard]

    %% --- MODULE: ACQUISITION (Supply Officer) ---
    DashSupply --> NavAcquire[Module: Acquisition/Receiving]
    NavAcquire --> FormIAR[Form: IAR Entry<br/>(Insp. & Acceptance Rpt)]
    FormIAR --> InputCost[Input: Unit Cost & Details]
    InputCost --> Logic50k{Cost >= 50k PHP?}
    
    Logic50k -- Yes --> TagPPE[Auto-Tag: PPE<br/>(Prop. Plant & Equip)]
    Logic50k -- No --> TagSemi[Auto-Tag: Semi-Expendable]
    
    TagPPE --> GenQR[Generate QR Code &<br/>Property Number]
    TagSemi --> GenQR
    GenQR --> PrintSticker[UI: Print Sticker Dialog]

    %% --- MODULE: ISSUANCE (Supply Officer) ---
    DashSupply --> NavIssue[Module: Issuance]
    NavIssue --> SelectAsset[Select Asset from Inventory]
    SelectAsset --> SelectUser[Select Custodian/Employee]
    SelectUser --> CheckType{Asset Type?}
    
    CheckType -- PPE --> GenPAR[Generate PAR Preview]
    CheckType -- Semi-Exp --> GenICS[Generate ICS Preview]
    
    GenPAR --> ConfirmIssue[Confirm Issuance]
    GenICS --> ConfirmIssue
    ConfirmIssue --> NotifyUser[Notify End User]

    %% --- MODULE: USER ACCEPTANCE (End User) ---
    DashUser --> ViewPending[View Pending Issuances]
    ViewPending --> ActionSign{Action?}
    ActionSign -- Accept --> DigitalSign[Digital Signature]
    ActionSign -- Reject --> Reason[Input Reason]
    DigitalSign --> UpdateDB[Update Database: Custodian Set]

    %% --- MODULE: ACCOUNTING (Accountant) ---
    DashAcct --> NavDepr[Module: Depreciation]
    NavDepr --> ViewSched[View Monthly Schedule]
    ViewSched --> CalcJEV[Calculate JEV<br/>(Straight Line Method)]
    CalcJEV --> ExportJEV[Export JEV to Excel/PDF]

    %% --- MODULE: DISPOSAL (Shared) ---
    DashSupply --> NavDispose[Module: Disposal