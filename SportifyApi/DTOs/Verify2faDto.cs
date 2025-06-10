namespace SportifyApi.DTOs
{
    public class Verify2faDto
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
