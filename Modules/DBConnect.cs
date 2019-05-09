using System;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

//todo
//add room creation, so it creates a room
//add guild id and guild name, so you can see what guild the user is connected from

namespace NewTestBot.Modules
{
    public class DBConnect : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnail = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";

        [Command("Connect", RunMode = RunMode.Async)] 
        public async Task ConnectDB(string response, params string[] args)
        {
            //taking the response from the user, converts it to string and removing the .connect part
            string userMessage = Context.Message.ToString();
            string name = userMessage.Substring(userMessage.IndexOf(' ') + 1);
            
            //replacing space with "%20"
            string account = name.Replace(" ", "%20");
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
            //using "i" and "f" for id
            //if account doesnt exist - throw error and end
            JObject f = null;
            try
            {
                string responsename = c.DownloadString("https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + account + "?api_key=" + apikey + "");
                JObject i = JObject.Parse(responsename);
                f = i;
                
            }
            catch (Exception)
            {
                 var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Summoner account doesnt exist!")
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

            //getting id, name and icon id from response
            var id = f["id"];
            var lolname = f["name"];
            var icon = f["profileIconId"];

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
                string usedtier = null;

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
                        usedtier = tier.ToLower();
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
                 }

            myconn.Close();

                //if the user already exists in the DB - just tell them and do nothing.
            if (Result == UserID)
            {
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your League and Discord account already exist in the Database!")
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

                    var username = Context.User;
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == usedtier);
                    await (username as IGuildUser).AddRoleAsync(role);
                    var UnrankedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "unranked");
                    await (username as IGuildUser).RemoveRoleAsync(UnrankedRole);

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
                string UserID = Context.User.Id.ToString();
                string Query = "DELETE FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
                string getID = "SELECT League_id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
                string id = null;

                MySqlConnection myconn = new MySqlConnection(connect);
                MySqlCommand command = new MySqlCommand(Query, myconn);
                MySqlCommand fetchID = new MySqlCommand(getID, myconn);
                MySqlDataReader myreader;
                MySqlDataReader myreader2;

                //first connection
                myconn.Open();
                myreader2 = fetchID.ExecuteReader();
                while (myreader2.Read())
                {
                    data = myreader2.GetString(0);
                    id = data;
                }
                myconn.Close();

                //second connection
                myconn.Open();
                myreader = command.ExecuteReader();
                myconn.Close();

                WebClient c = new WebClient();
                //getting league rank from ID
                //using "r" for rank
                string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/positions/by-summoner/" + id + "?api_key=" + apikey + "");
                JArray r = JArray.Parse(responserank);
                string rank = null;
                string tierused = null;
                
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
                        tierused = tier.ToLower();
                    }
                }
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

                var username = Context.User;
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == tierused);
                var UnrankedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "unranked");
                await (username as IGuildUser).RemoveRoleAsync(role);
                await (username as IGuildUser).AddRoleAsync(UnrankedRole);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

          
        }

//-------------------------------------------------------------------------------------------------------------------------
        [Command("update", RunMode = RunMode.Async)]
        public async Task UpdateAccount()
        {
            //getting id of sender and selecting lol id
            string UserID = Context.User.Id.ToString();
            string getID = "SELECT League_id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
            string getIcon = "SELECT Icon_id FROM users_testing WHERE Discord_Id like '%" + UserID + "%';";
            string id = null;

            //getting DB information
            string data = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(data);
            string apikey = (string)o["lolapi"]["apikey"];
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

            //getting the lol id from the sender of the update command
            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand command = new MySqlCommand(getID, myconn);
            MySqlCommand geticon = new MySqlCommand(getIcon, myconn);
            MySqlDataReader myreader;
            MySqlDataReader iconreader;

            //getting the League ID from the DB
            myconn.Open();
            myreader = command.ExecuteReader();
            while (myreader.Read())
            {
                data = myreader.GetString(0);
                id = data;
            }
            myconn.Close();

            //getting the icon ID from the DB
            myconn.Open();
            string iconid = null;
            iconreader = geticon.ExecuteReader();
            while (iconreader.Read())
            {
                data = iconreader.GetString(0);
                iconid = data;
            }
            myconn.Close();
            //using c for webclient connections
            WebClient c = new WebClient();

            //getting league rank from ID
            //using "r" for rank
            string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/positions/by-summoner/" + id + "?api_key=" + apikey + "");
            JArray r = JArray.Parse(responserank);
            string rank = null;

            //getting icon for thumbnail
            string findlatestlolversion = c.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
            JArray latestlolversion = JArray.Parse(findlatestlolversion);
            var version = latestlolversion[0];
            string thumbnailURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/profileicon/" + iconid + ".png";

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
            //updating the rank of the user
            string updaterank = "UPDATE users_testing SET `SOLO_QUEUE` = '" + rank + "' WHERE Discord_Id like  '%" + UserID + "%';";
                MySqlCommand updatecommand = new MySqlCommand(updaterank, myconn);
                MySqlDataReader myreader2;
                myconn.Open();
                myreader2 = updatecommand.ExecuteReader();
                myconn.Close();

            var embed = new EmbedBuilder();
                    embed.AddField("updating your account...",
                    "Your rank has been updated!")
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

        //-------------------------------------------------------------------------------------------------------------------------
        [Command("create", RunMode = RunMode.Async),RequireOwner]
        public async Task Createroles()
        {
            var allRanks = new[] { "Challenger", "GrandMaster", "Master", "Diamond", "Platinum", "Gold", "Silver", "Bronze", "Iron","Unranked" };
            var allrankcolors = new[] { new Color(240,140,15), new Color(253,7,7), new Color(192,7,146),new Color(32,102,148),new Color(46,204,113),new Color(241,196,15),new Color(151,156,159),new Color(187,121,68),new Color(255,255,255),new Color(124, 136, 120)};

            //my int for the color array
            int y = 0;

            //running through all the different roles and create them
            for (int x = 0; x < allRanks.GetLength(0); x++, y++)
            {
                GuildPermissions permissions = default;
                bool ishoisted = true;
                RequestOptions options = null;
                await Context.Guild.CreateRoleAsync(allRanks[x], permissions, allrankcolors[y], ishoisted, options);

            }
                    var embed = new EmbedBuilder();
                    embed.AddField("Creating roles for you now...",
                    "All of the rank roles has been created!")
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
    }
}
