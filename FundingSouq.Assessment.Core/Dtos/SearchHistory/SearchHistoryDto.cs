namespace FundingSouq.Assessment.Core.Dtos;

public class SearchHistoryDto
{
    public int HubPageId { get; set; }
    public string HubPageKey { get; set; }
    public List<HubPageHistoryDto> History { get; set; }
}

public class HubPageHistoryDto
{
    public int Id { get; set; }
    public string SearchQuery { get; set; }
    public DateTime SearchDate { get; set; }
}

public class HubPageDto
{
    public int Id { get; set; }
    public string Key { get; set; }
}