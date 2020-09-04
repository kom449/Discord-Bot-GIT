using Discord;
using Discord.Rest;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NewTestBot
{
    internal static class Global
    {
        internal static ulong MessageidToTrack { get; set; }
        internal static string Username { get; }
        internal static string currentuserid { get; set; }
        internal static RestUserMessage message { get; set; }

        //icon and thumbnail images
        internal readonly static string Birdieicon = "https://i.gyazo.com/128e4e68ac991c96610dcd5d67eeed50.png";
        internal readonly static string Birdiethumbnail = "https://i.gyazo.com/128e4e68ac991c96610dcd5d67eeed50.png";
        internal readonly static string Botcreatorname = "Need help? Contact: Xy'Pex Zukira#0001";

        //current version of the bot - I increment the number depending on what amount of features or fixes i do
        internal readonly static string version = "0.5.4";

        //db Connection info
        static string data = File.ReadAllText("Resources/config.json");
        static JObject o = JObject.Parse(data);
        internal readonly static string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
        (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

        //apikey
        internal readonly static string apikey = (string)o["lolapi"]["apikey"];

    }


}
