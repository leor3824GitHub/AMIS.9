using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

public sealed record ContactInformation
{
    public string Email { get; }
    public string PhoneNumber { get; }

    private ContactInformation(string email, string phoneNumber)
    {
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public static ContactInformation Create(string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new FshException("Email cannot be empty");

        if (!IsValidEmail(email))
            throw new FshException("Invalid email format");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new FshException("Phone number cannot be empty");

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var normalizedPhone = phoneNumber.Trim();

        return new ContactInformation(normalizedEmail, normalizedPhone);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public ContactInformation UpdateEmail(string newEmail)
    {
        return Create(newEmail, PhoneNumber);
    }

    public ContactInformation UpdatePhoneNumber(string newPhoneNumber)
    {
        return Create(Email, newPhoneNumber);
    }
}
