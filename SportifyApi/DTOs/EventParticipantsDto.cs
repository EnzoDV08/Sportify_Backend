namespace SportifyApi.DTOs
{
    public class EventParticipantDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
    }
}