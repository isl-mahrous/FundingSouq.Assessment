namespace FundingSouq.Assessment.Core.Entities;

public class City : BaseEntity
{
    public string Name { get; set; }
    public int CountryId { get; set; }
    
    public virtual Country Country { get; set; }
}