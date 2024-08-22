using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

public class Account : BaseEntity
{
    public int ClientId { get; set; }
    public string AccountNumber { get; set; }
    public BankAccountType AccountType { get; set; }
    public decimal Balance { get; set; }
    
    public virtual Client Client { get; set; }
}