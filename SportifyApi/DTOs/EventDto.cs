namespace SportifyApi.Dtos
{
    public class EventDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Visibility { get; set; }
        public string? Status { get; set; }

        public bool IsPrivate { get; set; }
        public List<int>? InvitedUserIds { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
