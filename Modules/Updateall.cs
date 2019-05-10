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
    class Updateall : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        [Command("updateall", RunMode = RunMode.Async)]
        public async Task Updateallranks()
        {
            string data = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(data);
            string apikey = (string)o["lolapi"]["apikey"];
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);
            string Query = "SELECT League_Id FROM users_testing";
            var returnedids = new[]{""};
            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand command = new MySqlCommand(Query, myconn);
            MySqlDataReader myreader;
            myconn.Open();
            myreader = command.ExecuteReader();
            while (myreader.Read())
            {
                var leagueids = new[] {myreader.ToString()};
                returnedids = leagueids;
                
            }

            for (int x = 0; x < returnedids.GetLength(0); x++)
            {

            }

        }
    }
}
