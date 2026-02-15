# Concurrency Conflict Root Cause Analysis

## Problem Statement
```
Concurrency conflict while issuing asset ba7a5b55-e9a6-4c5b-9b01-d45118acae33. 
Another process may be modifying this asset.

DbUpdateConcurrencyException: Asset was updated by another process. Please refresh the page and try again.
```

## Architecture Overview

### Request Flow
1. **API Endpoint** (`IssuePhysicalAssetEndpoint`) receives PUT request
2. **Command Handler** (`IssuePhysicalAssetHandler`) processes the command
3. **Domain Operation** (`PhysicalAsset.Issue()`) modifies entity state
4. **Repository Update** saves to database with version check
5. **Domain Event** (`PhysicalAssetIssued`) triggers event handler
6. **Event Handler** (`PhysicalAssetInventoryProjectionHandler`) updates related records

### Concurrency Control Mechanism
- **Concurrency Token**: `Version` property on `PhysicalAsset`
- **Configuration**: `builder.Property(x => x.Version).IsConcurrencyToken()`
- **Default Value**: 0
- **Increment**: Called by `IncrementVersion()` after each state change
- **Validation**: EF Core checks `WHERE id = X AND version = oldVersion` before update

## Root Cause Analysis

### The Problem: Split Query + Concurrent Requests

#### Code Flow in `PhysicalAssetByIdSpec`:
```csharp
public PhysicalAssetByIdSpec(Guid id)
{
    Query
        .Where(p => p.Id == id)
        .Include(p => p.AssignmentHistory)
        .Include(p => p.Product)
        .AsSplitQuery(); // ⚠️ KEY ISSUE
}
```

#### The Issue:
The `AsSplitQuery()` method means EF Core executes **two separate database queries**:
1. Query 1: Gets the `PhysicalAsset` record (including version)
2. Query 2: Gets the related `AssignmentHistory` collection

**Between these two queries, another concurrent request can modify the asset!**

### Scenario: Two Concurrent Issue Requests

```
Timeline:
─────────────────────────────────────────────────────────────────

Time T0:  Request A starts
          ├─ Fetches PhysicalAsset (Version = 0)
          └─ [Query 1 completes]

Time T1:  Request B starts  
          ├─ Fetches PhysicalAsset (Version = 0) <- Same version!
          └─ [Query 1 completes]

Time T2:  Request A (still fetching AssignmentHistory)
          ├─ [Query 2 completes]
          ├─ Calls asset.Issue() -> Version incremented to 1 in memory
          └─ Saves to DB: UPDATE PhysicalAsset SET Version=1 WHERE id=X AND Version=0
             ✓ SUCCESS (DB Version: 0 → 1)

Time T3:  Request B (still fetching AssignmentHistory)
          ├─ [Query 2 completes] 
          ├─ Calls asset.Issue() -> Version incremented to 1 in memory
          └─ Saves to DB: UPDATE PhysicalAsset SET Version=1 WHERE id=X AND Version=0
             ❌ FAIL - DB already has Version=1!
             → DbUpdateConcurrencyException thrown

```

## Why This Happens

### Root Causes:

1. **`AsSplitQuery()` creates a window for concurrent modifications**
   - The gap between Query 1 and Query 2 allows another request to modify the asset

2. **Multiple concurrent operations on the same asset**
   - User A clicks "Issue" at the same time User B clicks "Issue"
   - Both requests fetch the same asset with Version=0
   - Only the first one succeeds; the second fails

3. **Version number as concurrency token**
   - While correct in theory, it requires atomic fetches
   - Split queries break this atomicity

## Impact Analysis

### Affected Operations:
- `IssuePhysicalAssetHandler` - Issue asset to employee
- `ReturnPhysicalAssetHandler` - Return asset from employee  
- `TransferPhysicalAssetHandler` - Transfer to another employee
- Any operation modifying the same asset concurrently

### Why It's Hard to Reproduce:
- Requires precise timing of two concurrent requests
- Must target the exact same asset ID
- Network latency affects reproducibility
- User must click "Issue" button twice in quick succession

## Solution Approaches

### Option 1: Remove `AsSplitQuery()` (⭐ Recommended)
**Change the specification to use a single query:**
```csharp
// Current (problematic):
.AsSplitQuery(); 

// Solution: Use single query
// Remove AsSplitQuery() - let EF Core execute a JOIN instead
```

**Pros:**
- Single atomic query = no window for concurrent modifications
- Simple, minimal code change
- Maintains existing logic

**Cons:**  
- Potential cartesian explosion if AssignmentHistory has many records
- Larger result sets with duplicate PhysicalAsset data

**Analysis:** 
- AssignmentHistory is typically small (5-10 records per asset)
- Cartesian explosion would be minimal
- Better than concurrency failures

---

### Option 2: Add NoTracking for Read Operations
If AssignmentHistory grows large, add AsNoTracking temporarily to reduce overhead:

```csharp
Query
    .AsNoTracking()  // Read-only, then re-fetch before update
    .Where(p => p.Id == id)
    .Include(p => p.AssignmentHistory)
    .AsSplitQuery();
```

**Pros:**
- Less memory overhead
- Avoids change tracking on AssignmentHistory

**Cons:**
- Requires re-fetching before update (defeats the purpose)
- More complex code

---

### Option 3: Use Pessimistic Lock (❌ Not Recommended)
Add `SKIP LOCKED` or `WITH (NOLOCK)` at database level

**Cons:**
- Complex, database-specific
- Negative performance impact
- Doesn't align with DDD principles

---

### Option 4: Implement Distributed Lock (❌ Overkill)
Use Redis or Saga pattern

**Cons:**
- Massive complexity
- Requires distributed lock infrastructure
- Slower (network latency)

---

## Recommended Solution

### Fix: Remove `AsSplitQuery()` from `PhysicalAssetByIdSpec`

**File:** `api/modules/Inventories/Inventories.Application/PhysicalAssets/Specifications/PhysicalAssetByIdSpec.cs`

**Change:**
```csharp
public sealed class PhysicalAssetByIdSpec : Specification<PhysicalAsset>
{
    public PhysicalAssetByIdSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.AssignmentHistory)
            .Include(p => p.Product)
            // REMOVE: .AsSplitQuery();  ← DELETE THIS LINE
    }
}
```

**Why This Works:**
- Forces EF Core to execute a single JOIN query
- The entire result set is fetched atomically
- Between fetch and update, no other process can change the version
- Concurrency check will properly detect conflicts only if **actual simultaneous modifications** occur (not timing windows)

---

## Verification Steps

After implementing the fix:

1. **Test Single Request** ✓
   - Verify issue/return/transfer still works normally

2. **Test Concurrent Requests** 
   - Simulate two requests hitting the same asset ID simultaneously
   - One should succeed, one should get proper error message
   - NOT a version mismatch - a genuine conflict

3. **Test Related Records**
   - Verify AssignmentHistory is still properly loaded
   - Verify Product data is accessible  
   - Verify event handlers still receive correct data

4. **Performance Check**
   - Monitor query performance
   - Compare before/after execution times
   - AssignmentHistory should be small enough that JOIN doesn't cause issues

---

## Why We Shouldn't Use Retry Logic

While retrying seems intuitive, it's wrong because:

1. **It masks a real problem**: If two users genuinely try to issue the same asset, one SHOULD fail
2. **It creates race conditions**: Retry could succeed by "accident" instead of detecting conflicts
3. **It violates concurrency semantics**: The version token exists to prevent lost updates
4. **It's not idempotent**: Retrying could execute the domain operation twice

The error message should be: **"This asset was just modified. Please refresh and try again."** - not a silent retry.

---

## Implementation Checklist

- [ ] Remove `.AsSplitQuery()` from `PhysicalAssetByIdSpec.cs`
- [ ] Test with single concurrent request (should work)
- [ ] Test with two rapid concurrent requests (one should fail cleanly)
- [ ] Verify AssignmentHistory is still loaded
- [ ] Verify event handlers receive correct data
- [ ] Check database query performance
- [ ] Verify error message displayed to user is clear

---

## References

**EF Core Documentation:**
- [Split Queries](https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries)
- [Concurrency Tokens](https://learn.microsoft.com/en-us/ef/core/saving/concurrency)
- [Change Tracking](https://learn.microsoft.com/en-us/ef/core/change-tracking/)

**DDD Pattern:**
- Optimistic concurrency is correct approach for DDD aggregates
- Version token detects genuine conflicts, not timing windows
- User should refresh, not retry
