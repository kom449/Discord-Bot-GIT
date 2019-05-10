using System;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;

namespace NewTestBot.Modules
{
    public class Verify : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        string token = "";
        [Command("verify", RunMode = RunMode.Async)]

        /*
            Generate a password

            Requirements for password:
            random 8 chars
            letters and numbers
            upper and lower case letters
        */
        public async Task Verifyaccounts()
        {
                const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
                token = (Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            
            
        }
    }
}
