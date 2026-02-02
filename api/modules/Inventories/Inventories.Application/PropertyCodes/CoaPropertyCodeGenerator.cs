using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.Acceptances.Services;
using AMIS.WebApi.Inventories.Application.Employees.Search.v1;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Ardalis.Specification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AMIS.WebApi.Inventories.Application.PropertyCodes;

/// <summary>
/// COA/DBM-compliant property code generator.
/// Format: {Year}-{Agency}-{Office}-{Class}-{Category}-{Item}-{Sequence.Decimal}
/// OfficeCode serves as the tenant identifier (each office is a unique tenant).
/// </summary>
public sealed class CoaPropertyCodeGenerator : IAssetPropertyCodeGenerator
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Employee> _employeeRepo;
    private readonly IReadRepository<PpeCategoryCode> _classRepo;
    private readonly IReadRepository<PpeTypeCode> _categoryRepo;
    private readonly IReadRepository<PpeItemCode> _itemRepo;
    private readonly IRepository<PropertyCodeSequence> _sequenceRepo;
    private readonly CoaPropertyCodeOptions _options;

    public CoaPropertyCodeGenerator(
        ICurrentUser currentUser,
        [FromKeyedServices("inventories:employees")] IReadRepository<Employee> employeeRepo,
        [FromKeyedServices("inventories:ppeCategoryCodes")] IReadRepository<PpeCategoryCode> classRepo,
        [FromKeyedServices("inventories:ppeTypeCodes")] IReadRepository<PpeTypeCode> categoryRepo,
        [FromKeyedServices("inventories:ppeItemCodes")] IReadRepository<PpeItemCode> itemRepo,
        [FromKeyedServices("inventories:propertyCodeSequences")] IRepository<PropertyCodeSequence> sequenceRepo,
        IOptions<CoaPropertyCodeOptions> options)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _employeeRepo = employeeRepo ?? throw new ArgumentNullException(nameof(employeeRepo));
        _classRepo = classRepo ?? throw new ArgumentNullException(nameof(classRepo));
        _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
        _itemRepo = itemRepo ?? throw new ArgumentNullException(nameof(itemRepo));
        _sequenceRepo = sequenceRepo ?? throw new ArgumentNullException(nameof(sequenceRepo));
        _options = options?.Value ?? new CoaPropertyCodeOptions();
    }

    public Task<string> GenerateAsync(AcceptanceItem item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        var acquisitionDate = item.Acceptance?.AcceptanceDate ?? DateTime.UtcNow;
        var classification = item.PurchaseItem?.UnitPrice >= 50000m
            ? PropertyClassification.PropertyPlantEquipment
            : PropertyClassification.SemiExpendable;

        var request = new CoaPropertyCodeRequest(
            acquisitionDate,
            classification,
            OfficeCode: null,
            ClassCode: null,
            CategoryCode: null,
            ItemCode: null,
            SequenceSuffix: "0");

        return GenerateAsync(request, cancellationToken);
    }

    public async Task<string> GenerateAsync(CoaPropertyCodeRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var year = request.AcquisitionDate.Year;
        var agency = string.IsNullOrWhiteSpace(_options.AgencyCode) ? "NFA" : _options.AgencyCode.Trim().ToUpperInvariant();
        var officeCode = await ResolveOfficeCodeAsync(request.OfficeCode, cancellationToken).ConfigureAwait(false);

        var classCode = await NormalizeClassCodeAsync(request.ClassCode, cancellationToken).ConfigureAwait(false);
        var categoryCode = await NormalizeCategoryCodeAsync(classCode, request.CategoryCode, cancellationToken).ConfigureAwait(false);
        var itemCode = await NormalizeItemCodeAsync(classCode, categoryCode, request.ItemCode, cancellationToken).ConfigureAwait(false);

        var yearKey = _options.ResetSequenceAnnually ? year : 0;
        var sequence = await NextSequenceAsync(
            yearKey: yearKey,
            officeCode: officeCode,
            classCode: classCode,
            categoryCode: categoryCode,
            itemCode: itemCode,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        var seq = sequence.ToString().PadLeft(Math.Max(1, _options.SequenceLength), '0');
        var suffix = string.IsNullOrWhiteSpace(request.SequenceSuffix) ? "0" : request.SequenceSuffix!.Trim();

        return $"{year}-{agency}-{officeCode}-{classCode}-{categoryCode}-{itemCode}-{seq}.{suffix}";
    }

    private async Task<string> ResolveOfficeCodeAsync(string? officeCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(officeCode))
            return officeCode.Trim();

        var userId = _currentUser.GetUserId();
        if (userId != Guid.Empty)
        {
            var spec = new EmployeeByUserIdSpec(userId);
            var employee = await _employeeRepo.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(employee?.ResponsibilityCode))
            {
                return employee!.ResponsibilityCode.Trim();
            }
        }

        return _options.DefaultOfficeCode;
    }

    private async Task<string> NormalizeClassCodeAsync(string? classCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(classCode))
        {
            var normalized = classCode.Trim().ToUpperInvariant();
            var match = await _classRepo.FirstOrDefaultAsync(new PpeCategoryByCodeSpec(normalized), cancellationToken).ConfigureAwait(false);
            if (match is not null)
                return match.Code;
            return normalized;
        }

        return _options.DefaultClassCode;
    }

    private async Task<string> NormalizeCategoryCodeAsync(string classCode, string? categoryCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(categoryCode))
        {
            var normalized = categoryCode.Trim().ToUpperInvariant();
            var match = await _categoryRepo.FirstOrDefaultAsync(new PpeTypeByClassAndCodeSpec(classCode, normalized), cancellationToken).ConfigureAwait(false);
            if (match is not null)
                return match.Code;
            return normalized;
        }

        return _options.DefaultCategoryCode;
    }

    private async Task<string> NormalizeItemCodeAsync(string classCode, string categoryCode, string? itemCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(itemCode))
        {
            var normalized = itemCode.Trim();
            var match = await _itemRepo.FirstOrDefaultAsync(new PpeItemByCodesSpec(classCode, categoryCode, normalized), cancellationToken).ConfigureAwait(false);
            if (match is not null)
                return match.Code;
            return normalized;
        }

        return _options.DefaultItemCode;
    }

    private async Task<int> NextSequenceAsync(
        int yearKey,
        string officeCode,
        string classCode,
        string categoryCode,
        string itemCode,
        CancellationToken cancellationToken)
    {
        var spec = new PropertyCodeSequenceByKeySpec(yearKey, officeCode, classCode, categoryCode, itemCode);
        var sequence = await _sequenceRepo.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (sequence is null)
        {
            sequence = PropertyCodeSequence.Create(yearKey, officeCode, classCode, categoryCode, itemCode, _options.ResetSequenceAnnually);
            await _sequenceRepo.AddAsync(sequence, cancellationToken).ConfigureAwait(false);
        }

        var next = sequence.Increment();
        await _sequenceRepo.UpdateAsync(sequence, cancellationToken).ConfigureAwait(false);

        return next;
    }

    private sealed class PpeCategoryByCodeSpec : Ardalis.Specification.Specification<PpeCategoryCode>
    {
        public PpeCategoryByCodeSpec(string code) => Query.Where(x => x.Code == code && x.IsActive);
    }

    private sealed class PpeTypeByClassAndCodeSpec : Ardalis.Specification.Specification<PpeTypeCode>
    {
        public PpeTypeByClassAndCodeSpec(string classCode, string code) =>
            Query.Where(x => x.Category.Code == classCode && x.Code == code && x.IsActive);
    }

    private sealed class PpeItemByCodesSpec : Ardalis.Specification.Specification<PpeItemCode>
    {
        public PpeItemByCodesSpec(string classCode, string categoryCode, string code) =>
            Query.Where(x => x.ClassCode == classCode && x.CategoryCode == categoryCode && x.Code == code && x.IsActive);
    }

    private sealed class PropertyCodeSequenceByKeySpec : Ardalis.Specification.Specification<PropertyCodeSequence>
    {
        public PropertyCodeSequenceByKeySpec(
            int yearKey,
            string officeCode,
            string classCode,
            string categoryCode,
            string itemCode) =>
            Query.Where(x => x.YearKey == yearKey
                && x.OfficeCode == officeCode
                && x.ClassCode == classCode
                && x.CategoryCode == categoryCode
                && x.ItemCode == itemCode);
    }
}
