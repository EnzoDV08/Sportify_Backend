namespace SportifyApi.DTOs
{
    public class TwoFactorSetupDto
    {
        public string QrCodeImageUrl { get; set; } = string.Empty;
        public string ManualEntryKey { get; set; } = string.Empty;
    }
}
