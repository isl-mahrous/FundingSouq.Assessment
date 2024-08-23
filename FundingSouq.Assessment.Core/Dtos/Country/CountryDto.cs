namespace FundingSouq.Assessment.Core.Dtos;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string PhonePrefix { get; set; }
    public List<CityDto> Cities { get; set; } = new();
}

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CountryId { get; set; }
}