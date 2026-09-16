namespace VLRLiveBackEnd.Models;

public class TeamResponse
{
    public string status { get; set; } = "";
    public TeamData data { get; set; } = null!;
    public object? meta { get; set; }
    public object? message { get; set; }
}

public class TeamData
{
    public int status { get; set; }
    public TeamSegment[] segments { get; set; } = [];
}

public class TeamSegment
{
    public string id { get; set; } = "";
    public string name { get; set; } = "";
    public string tag { get; set; } = "";
    public string successor { get; set; } = "";
    public string logo { get; set; } = "";
    public string country { get; set; } = "";
    public string country_name { get; set; } = "";
    public string description { get; set; } = "";

    public TeamRating rating { get; set; } = null!;

    public TeamRoster[] roster { get; set; } = [];
}

public class TeamRating
{
    public string rank { get; set; } = "";
    public string rating { get; set; } = "";
    public string peak_rating { get; set; } = "";
    public string streak { get; set; } = "";
}

public class TeamRoster
{
    public string id { get; set; } = "";
    public string url { get; set; } = "";
    public string alias { get; set; } = "";
    public string real_name { get; set; } = "";
    public string avatar { get; set; } = "";
    public string country { get; set; } = "";
    public bool is_captain { get; set; }
    public string role { get; set; } = "";
    public bool is_staff { get; set; }
}