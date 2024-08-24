using PhoneNumbers;

namespace FundingSouq.Assessment.Core.Extensions;

/// <summary>
/// Provides helper methods.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Validates whether the provided phone number is in a valid format.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns><c>true</c> if the phone number is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            // Parse the phone number using the default region ("ZZ" for unknown region)
            var number = phoneNumberUtil.Parse(phoneNumber, "ZZ");
            return phoneNumberUtil.IsValidNumber(number); // Check if the number is valid
        }
        catch (NumberParseException)
        {
            return false; // Return false if parsing fails
        }
    }

    /// <summary>
    /// Validates whether the provided email address is in a valid format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns><c>true</c> if the email address is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(string email)
    {
        try
        {
            // Validate the email format using MailAddress
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email; // Return true if the address matches the input
        }
        catch
        {
            return false; // Return false if validation fails
        }
    }
}
