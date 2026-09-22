namespace VLRLiveBackEnd.DTOs
{
    public class LiveMatchDetailsDto
    {
        public string MatchId { get; set; } = "";

        public string? Event { get; set; } = "";

        public string Team1 { get; set; } = "";
        public string Team2 { get; set; } = "";

        public string? Team1Id { get; set; }
        public string? Team2Id { get; set; }

        public string? Team1Logo { get; set; } = "";
        public string? Team2Logo { get; set; } = "";

        public string SeriesScore { get; set; } = "";

        public string? CurrentMap { get; set; } = "";

        public string? CurrentMapScore { get; set; } = "";

        // 1-based: which map of the series this is (map 2, map 3...). 0 if the
        // match hasn't started its first map yet. VLR's API has no explicit
        // best-of-N field, so this is the closest reliable substitute.
        public int MapNumber { get; set; }
    }
}
