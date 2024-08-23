using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

public class UserLoginDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}

public class ClientLoginDto : UserLoginDto
{
    public string PersonalId { get; set; }
    public string ProfilePhoto { get; set; }
    public string MobileNumber { get; set; }
    public Gender Gender { get; set; } 
}

public class HubUserLoginDto : UserLoginDto
{
    public HubUserRole Role { get; set; }
}