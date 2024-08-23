using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

public class ClientDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalId { get; set; }
    public string ProfilePhoto { get; set; }
    public string MobileNumber { get; set; }
    public Gender Gender { get; set; }
    public string GenderStr => Gender.ToString();
    
    public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}

public class AddressDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public int CityId { get; set; }
    public string CityName { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
}

public class AccountDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string AccountNumber { get; set; }
    public BankAccountType AccountType { get; set; }
    public string AccountTypeStr => AccountType.ToString();
    public decimal Balance { get; set; }
}