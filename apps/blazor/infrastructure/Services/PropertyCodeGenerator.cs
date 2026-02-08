namespace AMIS.Blazor.Infrastructure.Services;

/// <summary>
/// Generates asset property codes in COA/DBM-compliant format.
/// Format: {Year}-{Agency}-{Office}-{Class}-{Category}-{Item}-{Sequence}
/// </summary>
public interface IPropertyCodeGenerator
{
    /// <summary>
    /// Generates a new property code with provided components.
    /// </summary>
    string Generate(
        int? year = null,
        string? agencyCode = null,
        string? officeCode = null,
        string? classCode = null,
        string? categoryCode = null,
        string? itemCode = null);

    /// <summary>
    /// Generates a simple temporary property code for immediate use.
    /// </summary>
    string GenerateTemporary();
}

public sealed class PropertyCodeGenerator : IPropertyCodeGenerator
{
    private const string DefaultAgency = "NFA";
    private const string DefaultOffice = "00";
    private const string DefaultClass = "01";
    private const string DefaultCategory = "01";
    private const string DefaultItem = "01";

    private static int _sequenceCounter;

    public PropertyCodeGenerator()
    {
        _sequenceCounter = new Random().Next(1000, 9999);
    }

    /// <summary>
    /// Generates a COA-compliant property code.
    /// Format: {Year}-{Agency}-{Office}-{Class}-{Category}-{Item}-{Sequence}
    /// Example: 2026-NFA-00-01-01-01-0001
    /// </summary>
    public string Generate(
        int? year = null,
        string? agencyCode = null,
        string? officeCode = null,
        string? classCode = null,
        string? categoryCode = null,
        string? itemCode = null)
    {
        var actualYear = year ?? DateTime.Now.Year;
        var agency = NormalizeCode(agencyCode ?? DefaultAgency, 3);
        var office = NormalizeCode(officeCode ?? DefaultOffice, 2);
        var classification = NormalizeCode(classCode ?? DefaultClass, 2);
        var category = NormalizeCode(categoryCode ?? DefaultCategory, 2);
        var item = NormalizeCode(itemCode ?? DefaultItem, 2);
        var sequence = FormatSequence(IncrementSequence());

        return $"{actualYear}-{agency}-{office}-{classification}-{category}-{item}-{sequence}";
    }

    /// <summary>
    /// Generates a simple temporary property code format.
    /// Format: PA-YYYYMMDDHHMMSS-XXXX
    /// Example: PA-20260205143022-5678
    /// </summary>
    public string GenerateTemporary()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"PA-{timestamp}-{random}";
    }

    /// <summary>
    /// Generates a structured property code with common defaults.
    /// Useful for quick generation without all parameters.
    /// </summary>
    private static string GenerateStructured()
    {
        var year = DateTime.Now.Year;
        var sequence = FormatSequence(IncrementSequence());
        return $"{year}-{DefaultAgency}-{DefaultOffice}-{DefaultClass}-{DefaultCategory}-{DefaultItem}-{sequence}";
    }

    private static string NormalizeCode(string code, int length)
    {
        if (string.IsNullOrWhiteSpace(code))
            return new string('0', length);

        var normalized = code.Trim().ToUpperInvariant();
        if (normalized.Length >= length)
            return normalized[..length];

        return normalized.PadRight(length, '0');
    }

    private static string FormatSequence(int sequence)
    {
        return sequence.ToString("D4");
    }

    private static int IncrementSequence()
    {
        _sequenceCounter++;
        if (_sequenceCounter > 9999)
            _sequenceCounter = 1;
        return _sequenceCounter;
    }
}
