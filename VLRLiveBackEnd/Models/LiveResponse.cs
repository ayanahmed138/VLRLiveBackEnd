namespace VLRLiveBackEnd.Models;

public class LiveResponse
{
    public string status { get; set; }
    public Data data { get; set; }
    public object meta { get; set; }
    public object message { get; set; }
}

public class Data
{
    public int status { get; set; }
    public Segment[] segments { get; set; }
}

public class Segment
{
    public string team1 { get; set; }
    public string team2 { get; set; }
    public string flag1 { get; set; }
    public string flag2 { get; set; }
    public string team1_logo { get; set; }
    public string team2_logo { get; set; }
    public string score1 { get; set; }
    public string score2 { get; set; }
    public string team1_round_ct { get; set; }
    public string team1_round_t { get; set; }
    public string team2_round_ct { get; set; }
    public string team2_round_t { get; set; }
    public string map_number { get; set; }
    public string current_map { get; set; }
    public string time_until_match { get; set; }
    public string match_event { get; set; }
    public string match_series { get; set; }
    public string unix_timestamp { get; set; }
    public string match_page { get; set; }
    public string match_id { get; set; }
}