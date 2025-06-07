using SportifyApi.Dtos;

namespace SportifyApi.DTOs
{
    public class FullFriendDto
    {
        public int Id { get; set; }
        public SimpleUserDto User { get; set; } = null!;
        public ProfileDto Profile { get; set; } = null!;
        public string Status { get; set; } = "pending";
    }
}
