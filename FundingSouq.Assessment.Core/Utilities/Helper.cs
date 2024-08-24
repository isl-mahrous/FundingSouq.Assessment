using PhoneNumbers;

namespace FundingSouq.Assessment.Core.Extensions;

public static class Helper
{
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var number = phoneNumberUtil.Parse(phoneNumber, "ZZ");
            return phoneNumberUtil.IsValidNumber(number);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
    
    public static bool IsValidEmail(string email)
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
}