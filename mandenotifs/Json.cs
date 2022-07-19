using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace mandenotifs;
public class ChannelDatum
{

    [JsonPropertyName("game_id")]
    public string GameId { get; set; }

    [JsonPropertyName("game_name")]
    public string GameName { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("delay")]
    public int Delay { get; set; }
}

public class RootChannel
{
    [JsonPropertyName("data")]
    public ChannelDatum[] _ { get; set; }
    public ChannelDatum Data => _.First();
}

public class StreamDatum
{


    [JsonPropertyName("game_id")]
    public string GameId { get; set; }

    [JsonPropertyName("game_name")]
    public string GameName { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("started_at")]
    public DateTime StartedAt { get; set; }
}
public class RootStream
{
    [JsonPropertyName("data")]
    public StreamDatum[] _ { get; set; }
    public StreamDatum? Data => _.FirstOrDefault();
}

public class WebhookMessage
{
    public string content { get; set; }
    public Embed[] embeds { get; set; }

    public class Embed
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public int color { get; set; }
        public Image image { get; set; }
        public Thumbnail thumbnail { get; set; }
        public Field[] fields { get; set; }

        public class Image
        {
            public string url { get; set; }
            public string proxy_url { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }
        public class Thumbnail
        {
            public string url { get; set; }
            public string proxy_url { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }
        public class Field
        {
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
