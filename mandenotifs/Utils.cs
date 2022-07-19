namespace mandenotifs;
internal static class Utils
{
    private static readonly Random Rng = new Random();

    // Picks random element from enumerables
    public static T Choice<T>(this IEnumerable<T> en) => en.ElementAt(Rng.Next(en.Count()));
    // Formats TimeSpans
    public static string FormatTime(this TimeSpan time) => time switch
    {
        { Days: > 1, Hours: > 1 } => $"{time:d' days and 'h' hours'}",
        { Days: 1, Hours: > 1 } => $"{time:d' day and 'h' hours'}",
        { Days: > 1, Hours: 1 } => $"{time:d' days and 'h' hour'}",

        { Days: > 1 } => $"{time:d' days'}",
        { Days: 1 } => $"{time:d' day'}",

        { Hours: >= 1, Minutes: >= 1, Seconds: >= 1 } => $"{time:h'h 'm'm 's's'}",

        { Hours: > 1, Minutes: > 1 } => $"{time:h' hours and 'm' minutes'}",
        { Hours: 1, Minutes: > 1 } => $"{time:h' hour and 'm' minutes'}",
        { Hours: > 1, Minutes: 1 } => $"{time:h' hours and 'm' minute'}",

        { Hours: > 1, Seconds: > 1 } => $"{time:h' hours and 's' seconds'}",
        { Hours: 1, Seconds: > 1 } => $"{time:h' hour and 's' seconds'}",
        { Hours: > 1, Seconds: 1 } => $"{time:h' hours and 's' second'}",

        { Hours: > 1 } => $"{time:h' hours'}",
        { Hours: 1 } => $"{time:h' hour'}",

        { Minutes: > 1, Seconds: > 1 } => $"{time:m' minutes and 's' seconds'}",
        { Minutes: 1, Seconds: > 1 } => $"{time:m' minute and 's' seconds'}",
        { Minutes: > 1, Seconds: 1 } => $"{time:m' minutes and 's' second'}",

        { Minutes: > 1 } => $"{time:m' minutes'}",
        { Minutes: 1 } => $"{time:m' minute'}",

        { Seconds: > 1 } => $"{time:s' seconds'}",
        { Seconds: 1 } => $"{time:s' second'}",

        _ => throw new NotImplementedException()
    };
}
