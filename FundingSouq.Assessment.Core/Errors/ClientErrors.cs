using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

public static class ClientErrors
{
    public static readonly Error ClientNotFound = new Error("CLIENT_NOT_FOUND", "Client not found.");
    public static readonly Error AccountNotFound = new Error("ACCOUNT_NOT_FOUND", "Account not found.");
    public static readonly Error AddressNotFound = new Error("ADDRESS_NOT_FOUND", "Address not found.");
    public static readonly Error CountryNotFound = new Error("COUNTRY_NOT_FOUND", "Country not found.");
    public static readonly Error CityNotFound = new Error("CITY_NOT_FOUND", "City not found.");
    public static readonly Error CityNotBelongToCountry = new Error("CITY_NOT_BELONG_TO_COUNTRY", "City does not belong to the country selected.");
    public static readonly Error InvalidAccountType = new Error("INVALID_ACCOUNT_TYPE", "Invalid account type.");
    public static readonly Error InvalidGender = new Error("INVALID_GENDER", "Invalid gender.");
    public static readonly Error InvalidCountry = new Error("INVALID_COUNTRY", "Invalid country.");
    public static readonly Error InvalidCity = new Error("INVALID_CITY", "Invalid city.");
    public static readonly Error EmailInUse = new Error("EMAIL_IN_USE", "Email is already in use.");

    public static readonly Error MobileNumberInUse =
        new Error("MOBILE_NUMBER_IN_USE", "Mobile number is already in use.");

    public static readonly Error PersonalIdInUse = new Error("PERSONAL_ID_IN_USE", "Personal ID is already in use.");

    public static readonly Error AccountNumberInUse =
        new Error("ACCOUNT_NUMBER_IN_USE", "Account number is already in use.");

    public static readonly Error ProfilePhotoRequired =
        new Error("PROFILE_PHOTO_REQUIRED", "Profile photo is required.");

    public static readonly Error InvalidProfilePhoto = new Error("INVALID_PROFILE_PHOTO", "Invalid profile photo.");
}