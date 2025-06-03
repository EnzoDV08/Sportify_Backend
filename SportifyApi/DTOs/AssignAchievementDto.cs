namespace SportifyApi.Dtos
{
    public class AssignAchievementDto
    {
        public int AchievementId { get; set; }
        public int UserId { get; set; }
        public int? EventId { get; set; } 
        public int AwardedByUserId { get; set; }
    }
}
