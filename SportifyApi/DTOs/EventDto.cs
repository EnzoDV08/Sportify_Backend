namespace SportifyApi.DTOs
{
    public class EventDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Visibility { get; set; }
        public string? Status { get; set; }
    }
}
