namespace SportifyApi.DTOs
{
    public class UserAchievementDto
    {
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public int? EventId { get; set; }
        public int? AwardedByAdminId { get; set; }
    }
}
