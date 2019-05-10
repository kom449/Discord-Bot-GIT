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
    public class Rank : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("rank", RunMode = RunMode.Async)]
        public async Task Getranks(string response = null, params string[] args)
        {
            if (response == null)
            {
            string UserID = Context.User.Id.ToString();
            string data = File.ReadAllText("Resources/config.json");
            string Query = "SELECT League_id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
            string getIcon = "SELECT Icon_id FROM users_testing WHERE Discord_Id like '%" + UserID + "%';";
            string id = null;

            WebClient c = new WebClient();
            JObject o = JObject.Parse(data);
            string apikey = (string)o["lolapi"]["apikey"];
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand command = new MySqlCommand(Query, myconn);
            MySqlCommand GetIcon = new MySqlCommand(getIcon, myconn);
            MySqlDataReader myreader;
            myconn.Open();
            myreader = command.ExecuteReader();
            while (myreader.Read())
            {
                data = myreader.GetString(0);
                id = data;
            }
            myconn.Close();

            myconn.Open();
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

                      var embed = new EmbedBuilder();
                    embed.AddField("Getting your ranks...",
                    "\nSolo Queue: "+ranksolo+"\n \n Flex 5v5: "+rankflex5+"\n \n Flex 3v3: "+rankflex3+"")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                string userMessage = Context.Message.ToString();
                string name = userMessage.Substring(userMessage.IndexOf(' ') + 1);

                //replacing space with "%20"
                string account = name.Replace(" ", "%20");

                string data = File.ReadAllText("Resources/config.json");

                JObject o = JObject.Parse(data);
                string apikey = (string)o["lolapi"]["apikey"];
                string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
                (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

                WebClient c = new WebClient();
                JObject f = null;
                try
                {
                    string responsename = c.DownloadString("https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + account + "?api_key=" + apikey + "");
                    JObject i = JObject.Parse(responsename);
                    f = i;

                }
                catch (Exception)
                {
                    var embed2 = new EmbedBuilder();
                    embed2.AddField("Connecting you...",
                    "Summoner account doesnt exist!")
                    .WithAuthor(author =>
                    {
                        author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")

                    .WithFooter(footer =>
                    {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed2);
                    return;
                }

                //getting id, name and icon id from response
                var id = f["id"];
                var lolname = f["name"];
                var icon = f["profileIconId"];

                string findlatestlolversion = c.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
                JArray latestlolversion = JArray.Parse(findlatestlolversion);
                var version = latestlolversion[0];
                string thumbnailURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/profileicon/" + icon + ".png";

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

                var embed = new EmbedBuilder();
                    embed.AddField("Getting the ranks of user "+name+"...",
                    "\nSolo Queue: "+ranksolo+"\n \n Flex 5v5: "+rankflex5+"\n \n Flex 3v3: "+rankflex3+"")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed);
            }
          

        }
    }
}
