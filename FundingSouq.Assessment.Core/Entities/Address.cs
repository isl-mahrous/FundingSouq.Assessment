namespace FundingSouq.Assessment.Core.Entities;

public class Address : BaseEntity
{
    public int ClientId { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    
    public virtual Client Client { get; set; }
    public virtual Country Country { get; set; }
    public virtual City City { get; set; }
}