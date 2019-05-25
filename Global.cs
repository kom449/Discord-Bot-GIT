using Discord.WebSocket;

namespace NewTestBot
{
    internal static class Global
    {
        internal static ulong MessageidToTrack { get; set; }
        internal static string Username { get; }
    }
}
