namespace VLRLiveBackEnd.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string VlrTeamId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Tag { get; set; } = null!;

        public string? Region { get; set; }

        public string? Country { get; set; }

        public string? LogoUrl { get; set; }

        public string? IconPath { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}