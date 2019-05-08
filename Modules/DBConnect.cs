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
    public class DBConnect : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnail = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";

        [Command("Connect", RunMode = RunMode.Async)] 
        public async Task ConnectDB(string response)
        {
            //replacing space with "%20"
            string account = response.Replace("_", "%20");

            //reading db info and apikey from file
            string data = File.ReadAllText("Resources/config.json");
            
            //using c for webclient connections
            WebClient c = new WebClient();

            //getting DB information
            JObject o = JObject.Parse(data);
            string apikey = (string)o["lolapi"]["apikey"];
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

            //getting league account ID
            //using "i" for id
                string responsename = c.DownloadString("https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + account + "?api_key=" + apikey + "");
                JObject i = JObject.Parse(responsename);
                var id = i["id"];
                var lolname = i["name"];
                var icon = i["profileIconId"];

            //getting league account icon
                string findlatestlolversion = c.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
                JArray latestlolversion = JArray.Parse(findlatestlolversion);
                var version = latestlolversion[0];
                string thumbnailURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/profileicon/" + icon + ".png";




            //getting league rank from ID
            //using "r" for rank
                string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/positions/by-summoner/" + id + "?api_key=" + apikey + "");
                JArray r = JArray.Parse(responserank);
                string rank = null;
            //using a for loop to check all the bodies of the json
            //since each queue type is in another body
                for (int x = 0; x < r.Count; x++)
                {
                    if (((string)r[x]["queueType"] == "RANKED_SOLO_5x5"))
                    {
                        var tier = (string)r[x]["tier"];
                        var division = (string)r[x]["rank"];
                        string soloq = tier + " " + division;
                        rank = soloq;
                    }
                }

            string UserID = Context.User.Id.ToString();
            string DiscordName = Context.User.Username;
            string Query = "INSERT INTO users_testing (Discord_Id,Discord_Name,League_Id,League_Name,SOLO_QUEUE,Icon_Id) VALUES ('" + UserID + "','" + DiscordName + "','" + id + "','" + lolname + "','" + rank+ "','" + icon + "');";
            string Duplicate = "SELECT Discord_Id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
            string Result;

            try
            {
                //sql connection and command
                    MySqlConnection myconn = new MySqlConnection(connect);
                    MySqlCommand command = new MySqlCommand(Query, myconn);
                    MySqlCommand DuplicateCommand = new MySqlCommand(Duplicate, myconn);
                    MySqlDataReader myreader;
                    myconn.Open();
                    Result = (string)DuplicateCommand.ExecuteScalar();

                //check for duplicate of discord Ids
                myreader = DuplicateCommand.ExecuteReader();
                if (myreader.Read())
                {
                    Result = myreader.GetString(myreader.GetOrdinal("Discord_Id"));
                }

                 while (myreader.Read())
                 {
                    Console.WriteLine("my result is: " + Result);
                 }

            myconn.Close();

                //if the user already exists in the DB - just tell them and do nothing.
            if (Result == UserID)
            {
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your League Discord account already exist in the Database!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
            }
            //if they dont exist - open connection and run the command to push to DB.
            else
            {
                    myconn.Open();
                    myreader = command.ExecuteReader();
                    myconn.Close();

                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your league account "+"`"+lolname+"`"+" with the rank: "+"`"+rank+"`"+" has been added to the Database!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
            }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
//-------------------------------------------------------------------------------------------------------------------------
        [Command("disconnect", RunMode = RunMode.Async)]
        public async Task RemoveAccount()
        {
            try
            {
                string data = File.ReadAllText("Resources/config.json");
                JObject o = JObject.Parse(data);
                string apikey = (string)o["lolapi"]["apikey"];
                string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
                (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

                //removing the account from the DB 
                string user = Context.User.Id.ToString();
                string Query = "DELETE FROM users_testing WHERE Discord_Id like  '%" + user + "%'; ";

                MySqlConnection myconn = new MySqlConnection(connect);
                MySqlCommand command = new MySqlCommand(Query, myconn);
                MySqlDataReader myreader;
                myconn.Open();
                myreader = command.ExecuteReader();
                myconn.Close();

                  var embed = new EmbedBuilder();
                    embed.AddField("Removing your account...",
                    "Your account has been removed!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

          
        }
    }
}
