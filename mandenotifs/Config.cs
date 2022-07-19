namespace mandenotifs;
internal static class Config
{
    // User id of the channel
    public const string ChannelId = "128856353";
    // Username of the channel in lowercase
    public const string ChannelName = "mande";
    // Access token of the bot
    public const string AccessToken = "";
    // Client id of the bot
    public const string ClientId = "";
    // The Discord webhook url to send the updates to
    public const string WebhookUrl = "";
    // The role to ping when the channel goes live
    public const string RolePing = "<@&872428407609237555>";

    // Emotes & messages to send with events
    public const string TwitchEmote = "<:twitch:941283346321924157>";
    public static readonly string[] LiveMessages =
    {
        "<:FeelsOkayMan:518900729335775263> live",
        "<:Okayge:876567657141403678> we live",
        "<:peepoCute:873540276499652668> click on strim",
        "<:peepoSit:880264409463808020>",
        "<:pepeL:872453918213427240> mande is live",
        "<a:docArrive:891461812632031272>",
        "<:PagMan:852875885987364914> LIVE",
        "<:Okayeg:951153389847322624> 👉 🥚 ❔"
    };
    public static readonly string[] OfflineMessages =
    {
        "<:Sadge:852878109949820938>",
        "<:despair:872453082171179018>",
        "<:PepeHands:852879438696939551> anyways",
        "<a:docLeave:891461653902811156>",
        "<:Sadeg:951155055334146139> no egs",
        "<:FeelsWeirdMan:853571570245042206> 👇 click them",
        "<:nyoo:872453050525163582>",
        "<:TrollDespair:876565539475054593>",
        "<:TrollDespair:876565539475054593> now what",
        "<:TrollDespair:876565539475054593> back to reality"
    };
}
