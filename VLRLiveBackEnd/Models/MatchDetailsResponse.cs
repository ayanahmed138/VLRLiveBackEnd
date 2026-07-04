namespace VLRLiveBackEnd.Models.MatchDetails;

public class MatchDetailsResponse
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
    public string match_id { get; set; }
    public Event _event { get; set; }
    public string date { get; set; }
    public string map_vetos { get; set; }
    public string status { get; set; }
    public Team[] teams { get; set; }
    public Stream[] streams { get; set; }
    public object[] vods { get; set; }
    public Map[] maps { get; set; }
    public Head_To_Head[] head_to_head { get; set; }
    public Performance performance { get; set; }
    public Economy1[] economy { get; set; }
    public Economy_By_Map[] economy_by_map { get; set; }
}

public class Event
{
    public string name { get; set; }
    public string series { get; set; }
    public string logo { get; set; }
}

public class Performance
{
    public Kill_Matrix[] kill_matrix { get; set; }
    public Advanced_Stats[] advanced_stats { get; set; }
    public By_Map[] by_map { get; set; }
}

public class Kill_Matrix
{
    public string player { get; set; }
    public Kills_Vs kills_vs { get; set; }
}

public class Kills_Vs
{
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
}

public class Advanced_Stats
{
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
    public string _6 { get; set; }
    public string _7 { get; set; }
    public string _8 { get; set; }
    public string _9 { get; set; }
    public string _10 { get; set; }
    public string _11 { get; set; }
    public string _12 { get; set; }
    public string _13 { get; set; }
    public string player { get; set; }
}

public class By_Map
{
    public string game_id { get; set; }
    public Kill_Matrix1[] kill_matrix { get; set; }
    public Advanced_Stats1[] advanced_stats { get; set; }
}

public class Kill_Matrix1
{
    public string player { get; set; }
    public Kills_Vs1 kills_vs { get; set; }
}

public class Kills_Vs1
{
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
}

public class Advanced_Stats1
{
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
    public string _6 { get; set; }
    public string _7 { get; set; }
    public string _8 { get; set; }
    public string _9 { get; set; }
    public string _10 { get; set; }
    public string _11 { get; set; }
    public string _12 { get; set; }
    public string _13 { get; set; }
    public string player { get; set; }
}

public class Team
{
    public string id { get; set; }
    public string name { get; set; }
    public string tag { get; set; }
    public string logo { get; set; }
    public string score { get; set; }
    public bool is_winner { get; set; }
}

public class Stream
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Map
{
    public string map_name { get; set; }
    public string picked_by { get; set; }
    public string duration { get; set; }
    public Score score { get; set; }
    public Score_Ct score_ct { get; set; }
    public Score_T score_t { get; set; }
    public Score_Ot score_ot { get; set; }
    public Players players { get; set; }
    public Round[] rounds { get; set; }
    public Performance1 performance { get; set; }
    public Economy[] economy { get; set; }
}

public class Score
{
    public int team1 { get; set; }
    public int team2 { get; set; }
}

public class Score_Ct
{
    public string team1 { get; set; }
    public string team2 { get; set; }
}

public class Score_T
{
    public string team1 { get; set; }
    public string team2 { get; set; }
}

public class Score_Ot
{
    public string team1 { get; set; }
    public string team2 { get; set; }
}

public class Players
{
    public Team1[] team1 { get; set; }
    public Team2[] team2 { get; set; }
}

public class Team1
{
    public string name { get; set; }
    public string agent { get; set; }
    public string rating { get; set; }
    public string acs { get; set; }
    public string kills { get; set; }
    public string deaths { get; set; }
    public string assists { get; set; }
    public string kd_diff { get; set; }
    public string kast { get; set; }
    public string adr { get; set; }
    public string hs_pct { get; set; }
    public string fk { get; set; }
    public string fd { get; set; }
    public string fk_diff { get; set; }
}

public class Team2
{
    public string name { get; set; }
    public string agent { get; set; }
    public string rating { get; set; }
    public string acs { get; set; }
    public string kills { get; set; }
    public string deaths { get; set; }
    public string assists { get; set; }
    public string kd_diff { get; set; }
    public string kast { get; set; }
    public string adr { get; set; }
    public string hs_pct { get; set; }
    public string fk { get; set; }
    public string fd { get; set; }
    public string fk_diff { get; set; }
}

public class Performance1
{
    public Kill_Matrix2[] kill_matrix { get; set; }
    public Advanced_Stats2[] advanced_stats { get; set; }
}

public class Kill_Matrix2
{
    public string player { get; set; }
    public Kills_Vs2 kills_vs { get; set; }
}

public class Kills_Vs2
{
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
}

public class Advanced_Stats2
{
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
    public string _6 { get; set; }
    public string _7 { get; set; }
    public string _8 { get; set; }
    public string _9 { get; set; }
    public string _10 { get; set; }
    public string _11 { get; set; }
    public string _12 { get; set; }
    public string _13 { get; set; }
    public string player { get; set; }
}

public class Round
{
    public int round_num { get; set; }
    public string winner { get; set; }
    public string side { get; set; }
}

public class Economy
{
    public string _0 { get; set; }
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
}

public class Head_To_Head
{
    public string _event { get; set; }
    public string date { get; set; }
    public Team3[] teams { get; set; }
    public string score { get; set; }
    public string url { get; set; }
}

public class Team3
{
    public string name { get; set; }
    public bool is_winner { get; set; }
}

public class Economy1
{
    public string _0 { get; set; }
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
}

public class Economy_By_Map
{
    public string game_id { get; set; }
    public Row[] rows { get; set; }
}

public class Row
{
    public string _0 { get; set; }
    public string _1 { get; set; }
    public string _2 { get; set; }
    public string _3 { get; set; }
    public string _4 { get; set; }
    public string _5 { get; set; }
}


