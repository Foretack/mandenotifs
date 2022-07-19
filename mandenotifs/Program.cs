using System.Text;
using System.Text.Json;
using mandenotifs;

HttpClient Requests = new HttpClient();
var RecurringTimer = new System.Timers.Timer();

// Authorize requests
Requests.DefaultRequestHeaders.Add("Authorization", $"Bearer {Config.AccessToken}");
Requests.DefaultRequestHeaders.Add("Client-Id", Config.ClientId);

// Stream data
bool Live = false;
string Title = string.Empty;
string GameName = string.Empty;
string GameId = string.Empty;
DateTime StartTime = DateTime.Now;

RecurringTimer.Interval = 7500; // 7.5 seconds
RecurringTimer.Enabled = true;
RecurringTimer.Elapsed += async (_, _) => await CheckStream();

// Checks stream data, if offline then it will check channel data through the CheckChannel() method
async Task CheckStream()
{
    try
    {
        Stream streamCall = await Requests.GetStreamAsync($"https://api.twitch.tv/helix/streams?user_id={Config.ChannelId}");
        RootStream? stream = await JsonSerializer.DeserializeAsync<RootStream>(streamCall);
        if (stream is null || stream.Data is null) throw new Exception();
        if (Title != stream.Data.Title) await TriggerTitleChangEvent(stream.Data.Title);
        if (GameName != stream.Data.GameName) await TriggerGameChangeEvent(stream.Data.GameName, stream.Data.GameId);
        if (!Live) await TriggerLiveEvent();
        StartTime = stream.Data.StartedAt;
    }
    catch
    {
        if (Live) await TriggerOfflineEvent();
        await CheckChannel();
        Console.WriteLine($"{Config.ChannelName} is offline. ");
    }
}

// Checks the channel data
async Task CheckChannel()
{
    try
    {
        Stream channelCall = await Requests.GetStreamAsync($"https://api.twitch.tv/helix/channels?broadcaster_id={Config.ChannelId}");
        RootChannel? channel = await JsonSerializer.DeserializeAsync<RootChannel>(channelCall);
        if (channel is null || channel.Data is null) throw new AccessViolationException();
        if (Title != channel.Data.Title) await TriggerTitleChangEvent(channel.Data.Title);
        if (GameName != channel.Data.GameName) await TriggerGameChangeEvent(channel.Data.GameName, channel.Data.GameId);
    }
    catch
    {
        Console.WriteLine("Connection or serialization error");
    }
}

async Task TriggerLiveEvent()
{
    Live = true;
    WebhookMessage message = new();

    WebhookMessage.Embed embed = new();
    embed.title = $"{Config.TwitchEmote} {Title}";
    embed.url = $"https://twitch.tv/{Config.ChannelName}";
    embed.description = Config.LiveMessages.Choice();
    embed.color = 16713736;
    embed.image = new()
    {
        url = $"https://static-cdn.jtvnw.net/previews-ttv/live_user_{Config.ChannelName}-1280x720.jpg"
    };

    WebhookMessage.Embed.Field field = new()
    {
        name = "Game:",
        value = GameName
    };

    embed.fields = new[] { field };
    message.embeds = new[] { embed };
    message.content = Config.RolePing;

    string jsonString = JsonSerializer.Serialize(message);
    StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
    await Requests.PostAsync(Config.WebhookUrl, content);
    Console.WriteLine($"Went live at {DateTime.Now:F}");
}

async Task TriggerTitleChangEvent(string newTitle)
{
    Title = newTitle;
    WebhookMessage message = new();

    WebhookMessage.Embed embed = new();
    embed.title = $"{Config.TwitchEmote} New Title!";
    embed.description = Title;
    embed.color = 8622005;

    message.embeds = new[] { embed };

    string jsonString = JsonSerializer.Serialize(message);
    StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
    await Requests.PostAsync(Config.WebhookUrl, content);
    Console.WriteLine($"Title changed at {DateTime.Now:F}");
}

async Task TriggerGameChangeEvent(string newGame, string id)
{
    GameName = newGame;
    GameId = id;
    WebhookMessage message = new();

    WebhookMessage.Embed embed = new();
    embed.title = $"{Config.TwitchEmote} New Game!";
    embed.description = GameName;
    embed.color = 5994715;
    embed.image = new()
    {
        url = $"https://static-cdn.jtvnw.net/ttv-boxart/{GameId}-710x1000.jpg"
    };

    message.embeds = new[] { embed };

    string jsonString = JsonSerializer.Serialize(message);
    StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
    await Requests.PostAsync(Config.WebhookUrl, content);
    Console.WriteLine($"Game changed at {DateTime.Now:F}");
}

async Task TriggerOfflineEvent()
{
    Live = false;
    WebhookMessage message = new();

    WebhookMessage.Embed embed = new();
    embed.title = $"{Config.TwitchEmote} The stream is now offline!";
    embed.description = Config.OfflineMessages.Choice();
    embed.color = 15984156;

    WebhookMessage.Embed.Field socialsField = new()
    {
        name = "Stay updated with Mande:",
        value = "[Twitter](https://twitter.com/MandeOW/)\n"
              + "[Youtube](https://www.youtube.com/channel/UCTXqDy7RTOoi_p2JyySAa_A)\n"
              + "[Variety Youtube](https://www.youtube.com/channel/UCzBcQIBnxb5mMqnTba-OB0A)\n"
              + "[Instagram](https://www.instagram.com/mikkel.hestbek/)\n"
              + "[TikTok](https://www.tiktok.com/@mandetrodser)"
    };
    WebhookMessage.Embed.Field uptimeField = new()
    {
        name = "Streamed for: ",
        value = (DateTime.Now - StartTime.ToLocalTime()).FormatTime()
    };

    embed.fields = new[] { socialsField, uptimeField };
    message.embeds = new[] { embed };

    string jsonString = JsonSerializer.Serialize(message);
    StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
    await Requests.PostAsync(Config.WebhookUrl, content);
    Console.WriteLine($"Went offline at {DateTime.Now:F}");
}

Console.ReadLine();
