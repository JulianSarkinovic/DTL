namespace DikkeTennisLijst.Core.Shared.Helpers
{
    public class AppSettings
    {
        public string? AdminEmail { get; set; }
        public string? AdminPassword { get; set; }
        public string? ApplicationEmail { get; set; }
        public string? ApplicationName { get; set; }
        public string? JwtSecretKey { get; set; }
        public string? SendGridKey { get; set; }
        public string? UrlFrontend { get; set; }
    }
}