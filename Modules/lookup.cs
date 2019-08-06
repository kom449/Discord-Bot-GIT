using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using System;
using Discord.WebSocket;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using MySql.Data.MySqlClient;

namespace NewTestBot.Modules
{
    public class lookup : ModuleBase<SocketCommandContext>
    {
        [Command("account"), RequireOwner]
        public async Task Lookup(IGuildUser user)
        {
            var id = user.Id;
            WebClient c = new WebClient();
            string data = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(data);
            string apikey = (string)o["lolapi"]["apikey"];
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);
            string Query = "SELECT League_id FROM users_testing WHERE Discord_Id like  '%" + id + "%'; ";
            string getIcon = "SELECT Icon_id FROM users_testing WHERE Discord_Id like '%" + id + "%';";
            string lolid = null;

            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand command = new MySqlCommand(Query, myconn);
            MySqlCommand GetIcon = new MySqlCommand(getIcon, myconn);
            MySqlDataReader myreader;
            myconn.Open();
            myreader = command.ExecuteReader();
            while (myreader.Read())
            {
                string returnedid;
                returnedid = myreader.GetString(0);
                lolid = returnedid;
            }
            myconn.Close();

            if (lolid == "")
            {
                //no account was found in the DB
                return;
            }

            string iconid = null;
            myreader = GetIcon.ExecuteReader();
            while (myreader.Read())
            {
                data = myreader.GetString(0);
                iconid = data;
            }
            myconn.Close();

            string findlatestlolversion = c.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
            JArray latestlolversion = JArray.Parse(findlatestlolversion);
            var version = latestlolversion[0];
            string thumbnailURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/profileicon/" + iconid + ".png";

            string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/" + id + "?api_key=" + apikey + "");
            JArray r = JArray.Parse(responserank);
            string ranksolo = null;
            string rankflex5 = null;
            string rankflex3 = null;

            //using a for loop to check all the bodies of the json
            //since each queue type is in another body
            for (int x = 0; x < r.Count; x++)
            {
                if (((string)r[x]["queueType"] == "RANKED_SOLO_5x5"))
                {
                    var tiersolo = (string)r[x]["tier"];
                    var divisionsolo = (string)r[x]["rank"];
                    string soloq = tiersolo + " " + divisionsolo;
                    ranksolo = soloq;
                }
                else
                {
                    rankflex3 = "Unranked";
                }
            }

            //using the same loop to get the Flex 5v5 rank
            for (int y = 0; y < r.Count; y++)
            {
                if (((string)r[y]["queueType"] == "RANKED_FLEX_SR"))
                {
                    var tierflex5v5 = (string)r[y]["tier"];
                    var divisionflex5v5 = (string)r[y]["rank"];
                    string flex5v5 = tierflex5v5 + " " + divisionflex5v5;
                    rankflex5 = flex5v5;
                }
                else
                {
                    rankflex3 = "Unranked";
                }
            }

            //again using the same loop to find the Flex 3v3 rank
            for (int z = 0; z < r.Count; z++)
            {
                if (((string)r[z]["queueType"] == "RANKED_FLEX_TT"))
                {
                    var tierflex3v3 = (string)r[z]["tier"];
                    var divisionflex3v3 = (string)r[z]["rank"];
                    string flex3v3 = tierflex3v3 + " " + divisionflex3v3;
                    rankflex3 = flex3v3;
                }
                else
                {
                    rankflex3 = "Unranked";
                }
            }


        }
    }
}
