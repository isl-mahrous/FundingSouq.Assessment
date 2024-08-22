using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

public class Client : User
{
    public string PersonalId { get; set; }
    public string ProfilePhoto { get; set; }
    public string MobileNumber { get; set; }
    public Gender Gender { get; set; } 
    public ICollection<Address>  Addresses { get; set; } = new List<Address>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}