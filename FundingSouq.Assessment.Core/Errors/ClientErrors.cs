using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

/// <summary>
/// Provides a collection of predefined error messages related to client operations.
/// </summary>
public static class ClientErrors
{
    /// <summary>
    /// Error indicating that the client was not found.
    /// </summary>
    public static readonly Error ClientNotFound = new Error("CLIENT_NOT_FOUND", "Client not found.");

    /// <summary>
    /// Error indicating that the account associated with the client was not found.
    /// </summary>
    public static readonly Error AccountNotFound = new Error("ACCOUNT_NOT_FOUND", "Account not found.");

    /// <summary>
    /// Error indicating that the address associated with the client was not found.
    /// </summary>
    public static readonly Error AddressNotFound = new Error("ADDRESS_NOT_FOUND", "Address not found.");

    /// <summary>
    /// Error indicating that the specified country was not found.
    /// </summary>
    public static readonly Error CountryNotFound = new Error("COUNTRY_NOT_FOUND", "Country not found.");

    /// <summary>
    /// Error indicating that the specified city was not found.
    /// </summary>
    public static readonly Error CityNotFound = new Error("CITY_NOT_FOUND", "City not found.");

    /// <summary>
    /// Error indicating that the city does not belong to the selected country.
    /// </summary>
    public static readonly Error CityNotBelongToCountry = new Error("CITY_NOT_BELONG_TO_COUNTRY", "City does not belong to the country selected.");

    /// <summary>
    /// Error indicating that the provided account type is invalid.
    /// </summary>
    public static readonly Error InvalidAccountType = new Error("INVALID_ACCOUNT_TYPE", "Invalid account type.");

    /// <summary>
    /// Error indicating that the provided gender value is invalid.
    /// </summary>
    public static readonly Error InvalidGender = new Error("INVALID_GENDER", "Invalid gender.");

    /// <summary>
    /// Error indicating that the provided country is invalid.
    /// </summary>
    public static readonly Error InvalidCountry = new Error("INVALID_COUNTRY", "Invalid country.");

    /// <summary>
    /// Error indicating that the provided city is invalid.
    /// </summary>
    public static readonly Error InvalidCity = new Error("INVALID_CITY", "Invalid city.");

    /// <summary>
    /// Error indicating that the email is already in use by another client.
    /// </summary>
    public static readonly Error EmailInUse = new Error("EMAIL_IN_USE", "Email is already in use.");

    /// <summary>
    /// Error indicating that the mobile number is already in use by another client.
    /// </summary>
    public static readonly Error MobileNumberInUse = new Error("MOBILE_NUMBER_IN_USE", "Mobile number is already in use.");

    /// <summary>
    /// Error indicating that the personal ID is already in use by another client.
    /// </summary>
    public static readonly Error PersonalIdInUse = new Error("PERSONAL_ID_IN_USE", "Personal ID is already in use.");

    /// <summary>
    /// Error indicating that a profile photo is required.
    /// </summary>
    public static readonly Error ProfilePhotoRequired = new Error("PROFILE_PHOTO_REQUIRED", "Profile photo is required.");

    /// <summary>
    /// Error indicating that the provided profile photo is invalid.
    /// </summary>
    public static readonly Error InvalidProfilePhoto = new Error("INVALID_PROFILE_PHOTO", "Invalid profile photo.");
}
