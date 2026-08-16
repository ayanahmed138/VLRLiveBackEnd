namespace VLRLiveBackEnd.DTOs
{
    public class LiveMatchDetailsDto
    {
        public string MatchId { get; set; } = "";

        public string? Event { get; set; } = "";

        public string Team1 { get; set; } = "";
        public string Team2 { get; set; } = "";

        public string? Team1Logo { get; set; } = "";
        public string? Team2Logo { get; set; } = "";

        public string SeriesScore { get; set; } = "";

        public string? CurrentMap { get; set; } = "";

        public string? CurrentMapScore { get; set; } = "";
    }
}
