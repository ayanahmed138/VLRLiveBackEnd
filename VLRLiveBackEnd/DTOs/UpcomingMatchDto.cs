namespace VLRLiveBackEnd.DTOs;

public class UpcomingMatchDto
{
    public string Team1 { get; set; } = "";
    public string Team2 { get; set; } = "";

    public string Event { get; set; } = "";
    public string Series { get; set; } = "";

    public string StartsIn { get; set; } = "";

    public string MatchPage { get; set; } = "";

    public string UnixTimestamp { get; set; } = "";
}