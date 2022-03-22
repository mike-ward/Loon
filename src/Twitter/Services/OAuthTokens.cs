namespace Twitter.Services
{
    public sealed class OAuthTokens
    {
        public string? OAuthToken  { get; set; }
        public string? OAuthSecret { get; set; }
        public string? UserId      { get; set; }
        public string? ScreenName  { get; set; }
    }
}