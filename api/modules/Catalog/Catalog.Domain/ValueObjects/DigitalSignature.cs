namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Represents a digital signature with audit trail information.
/// Used for tracking acceptance signatures on asset issuances.
/// </summary>
public class DigitalSignature : IEquatable<DigitalSignature>
{
    public string SignatureData { get; private set; } = default!;
    public DateTime SignedOn { get; private set; }
    public Guid SignedByEmployeeId { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? DeviceFingerprint { get; private set; }

    private DigitalSignature() { }

    private DigitalSignature(
        string signatureData,
        DateTime signedOn,
        Guid signedByEmployeeId,
        string? ipAddress = null,
        string? userAgent = null,
        string? deviceFingerprint = null)
    {
        if (string.IsNullOrWhiteSpace(signatureData))
            throw new ArgumentException("Signature data cannot be empty.", nameof(signatureData));

        if (signedByEmployeeId == Guid.Empty)
            throw new ArgumentException("Employee ID must be provided.", nameof(signedByEmployeeId));

        if (signedOn == default || signedOn > DateTime.UtcNow)
            throw new ArgumentException("Signed date must be valid and not in the future.", nameof(signedOn));

        SignatureData = signatureData;
        SignedOn = signedOn;
        SignedByEmployeeId = signedByEmployeeId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        DeviceFingerprint = deviceFingerprint;
    }

    public static DigitalSignature Create(
        string signatureData,
        Guid employeeId,
        string? ipAddress = null,
        string? userAgent = null,
        string? deviceFingerprint = null)
    {
        return new DigitalSignature(
            signatureData,
            DateTime.UtcNow,
            employeeId,
            ipAddress,
            userAgent,
            deviceFingerprint);
    }

    public override bool Equals(object? obj)
    {
        return obj is DigitalSignature signature && Equals(signature);
    }

    public bool Equals(DigitalSignature? other)
    {
        if (other is null) return false;
        return SignatureData == other.SignatureData &&
               SignedOn == other.SignedOn &&
               SignedByEmployeeId == other.SignedByEmployeeId &&
               IpAddress == other.IpAddress &&
               UserAgent == other.UserAgent &&
               DeviceFingerprint == other.DeviceFingerprint;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SignatureData, SignedOn, SignedByEmployeeId, IpAddress, UserAgent, DeviceFingerprint);
    }
}
