﻿using Discord.WebSocket;
using Discord.Rest;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NewTestBot
{
    internal static class Global
    {
        internal static ulong MessageidToTrack { get; set; }
        internal static string Username { get; }
        internal static string Command { get; set; }

        //icon and thumbnail images
        internal readonly static string Birdieicon = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        internal readonly static string Birdiethumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        //db Connection info
        static string data = File.ReadAllText("Resources/config.json");
        static JObject o = JObject.Parse(data);
        internal readonly static string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
        (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);
    }
}
