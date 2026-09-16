using System.Text.Json.Serialization;

namespace VLRLiveBackEnd.Models.Event;

public class EventResponse
{
    public string status { get; set; } = null!;
    public EventData data { get; set; } = null!;
    public object? meta { get; set; }
    public object? message { get; set; }
}

public class EventData
{
    public int status { get; set; }
    public EventSegments segments { get; set; } = null!;
}

public class EventSegments
{
    [JsonPropertyName("event")]
    public EventInfo Event { get; set; } = null!;

    public EventTeam[] teams { get; set; } = [];
}

public class EventInfo
{
    public string name { get; set; } = "";
    public string series { get; set; } = "";
    public string subtitle { get; set; } = "";
    public string dates { get; set; } = "";
    public string prize { get; set; } = "";
    public string location { get; set; } = "";
    public string logo { get; set; } = "";
}

public class EventTeam
{
    public string id { get; set; } = "";
    public string name { get; set; } = "";
    public EventPlayer[] players { get; set; } = [];
    public string qualification { get; set; } = "";
}

public class EventPlayer
{
    public string id { get; set; } = "";
    public string name { get; set; } = "";
    public string flag { get; set; } = "";
}