namespace FundingSouq.Assessment.Core.Enums;

/// <summary>
/// Represents the types of users in the system.
/// </summary>
public enum UserType
{
    /// <summary>
    /// A user associated with the hub.
    /// </summary>
    HubUser = 0,

    /// <summary>
    /// A client user.
    /// </summary>
    Client = 1
}

/// <summary>
/// Represents the roles that a hub user can have.
/// </summary>
public enum HubUserRole
{
    /// <summary>
    /// An administrative user with higher privileges.
    /// </summary>
    Admin = 0,

    /// <summary>
    /// A standard user with limited privileges.
    /// </summary>
    User = 1
}

/// <summary>
/// Represents the gender of a person.
/// </summary>
public enum Gender
{
    /// <summary>
    /// Male gender.
    /// </summary>
    Male = 0,

    /// <summary>
    /// Female gender.
    /// </summary>
    Female = 1,
}

/// <summary>
/// Represents different types of bank accounts.
/// </summary>
public enum BankAccountType
{
    /// <summary>
    /// A savings account.
    /// </summary>
    Savings = 0,

    /// <summary>
    /// A checking account.
    /// </summary>
    Checking = 1,

    /// <summary>
    /// A credit account.
    /// </summary>
    Credit = 2,

    /// <summary>
    /// A debit account.
    /// </summary>
    Debit = 3,

    /// <summary>
    /// A loan account.
    /// </summary>
    Loan = 4,

    /// <summary>
    /// An investment account.
    /// </summary>
    Investment = 5,

    /// <summary>
    /// A mortgage account.
    /// </summary>
    Mortgage = 6,

    /// <summary>
    /// A retirement account.
    /// </summary>
    Retirement = 7,

    /// <summary>
    /// A student account.
    /// </summary>
    Student = 8,

    /// <summary>
    /// A joint account.
    /// </summary>
    Joint = 9,

    /// <summary>
    /// A business account.
    /// </summary>
    Business = 10,
}

/// <summary>
/// Represents the direction in which a list should be sorted.
/// </summary>
public enum SortDirection
{
    /// <summary>
    /// Sort the list in ascending order.
    /// </summary>
    Ascending = 0,

    /// <summary>
    /// Sort the list in descending order.
    /// </summary>
    Descending = 1
}