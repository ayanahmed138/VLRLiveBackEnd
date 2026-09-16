namespace VLRLiveBackEnd.DTOs
{
    public class VLRTeamDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Tag { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? Logo { get; set; }
    }
}