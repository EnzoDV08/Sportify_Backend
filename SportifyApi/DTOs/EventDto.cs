namespace SportifyApi.Dtos
{
  public class EventDto
  {
    public int EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? RequiredItems { get; set; }
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Visibility { get; set; }
    public string? Status { get; set; }
    public string? ImageUrl { get; set; }
    public int CreatorUserId { get; set; }
    public List<int> InvitedUserIds { get; set; } = new();
    public string CreatorUserType { get; set; } = string.Empty;
    public string? CreatorName { get; set; } 
    }
}