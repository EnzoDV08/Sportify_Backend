namespace SportifyApi.Dtos
{
    public class EventDto
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Visibility { get; set; }
        public string? Status { get; set; }
    }
}
