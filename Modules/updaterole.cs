using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System;
using Discord.WebSocket;

namespace NewTestBot.Modules
{
    public class Updaterole : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        [Command("update", RunMode = RunMode.Async)]
        public async Task UpdateAccount()
        {
            //getting id of sender and selecting lol id
            string DiscordName = Context.User.Username;
            string UserID = Context.User.Id.ToString();
            string getID = "SELECT League_id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
            string getIcon = "SELECT Icon_id FROM users_testing WHERE Discord_Id like '%" + UserID + "%';";
            string getstatus = "SELECT verified FROM users_testing WHERE Discord_ID like '%" + UserID + "%';";
            string InsertDiscordName = "UPDATE users_testing SET `Discord_Name` = '" + DiscordName + "' WHERE Discord_Id like  '%" + UserID + "%';";
            string getToken = "SELECT TOKEN FROM users_testing WHERE Discord_Id like '%" + UserID + "%';";
            string id = null;
            string token = null;
            string status = null;

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
            MySqlCommand GetToken = new MySqlCommand(getToken, myconn);
            MySqlCommand GetStatus = new MySqlCommand(getstatus, myconn);
            MySqlCommand InsertDiscordname = new MySqlCommand(InsertDiscordName, myconn);
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

            //get verification status
            myconn.Open();
            myreader = GetStatus.ExecuteReader();
            while (myreader.Read())
            {
                data = myreader.GetString(0);
                status = data;
            }
            myconn.Close();
                
                if(id == "")
                {                   
                    var embed2 = new EmbedBuilder();
                    embed2.AddField("updating your account...",
                    "No account was found!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed2);
                    await Task.Delay(5000);
                    var messages2 = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages2);
                    return;
                }

                if (status == "false")
                {                   
                    var embed2 = new EmbedBuilder();
                    embed2.AddField("updating your account...",
                    "Your account is not verified!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed2);
                    await Task.Delay(5000);
                    var messages2 = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages2);
                    return;
                }

            myconn.Open();
            myreader = GetToken.ExecuteReader();
            while (myreader.Read())
            {
                data = myreader.GetString(0);
                token = data;
            }
            myconn.Close();

                if(token == "")
                {                   
                    var embed2 = new EmbedBuilder();
                    embed2.AddField("updating your account...",
                    "Your account is not verified!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed2);
                    await Task.Delay(5000);
                    var messages3 = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages3);
                    return;
                }

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
            string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/" + id + "?api_key=" + apikey + "");

            JArray r = JArray.Parse(responserank);
            string rank = null;
            string usedtiersolo = null;

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
                    var tiersolo = (string)r[x]["tier"];
                    var division = (string)r[x]["rank"];
                    string soloq = tiersolo + " " + division;
                    rank = soloq;
                    usedtiersolo = tiersolo.ToLower();
                }
                else
                {
                    rank = "Unranked";
                    usedtiersolo = "unranked";
                }
            }

            myconn.Open();
            myreader = InsertDiscordname.ExecuteReader();
            myconn.Close();

            //updating the rank of the user
            string Discordname = Context.User.Username;
            string updaterank = "UPDATE users_testing SET `SOLO_QUEUE` = '" + rank + "', `Discord_Name` = '" + Discordname + "' WHERE Discord_Id like  '%" + UserID + "%';";
            MySqlCommand updatecommand = new MySqlCommand(updaterank, myconn);
            myconn.Open();
            myreader = updatecommand.ExecuteReader();
            myconn.Close();

            var allRanks = new[] { "challenger", "grandMaster", "master", "diamond", "platinum", "gold", "silver", "bronze", "iron","unranked", "new ones :)" };
            var username = Context.User as SocketGuildUser;            
            
            //running through all the different roles
            for (int x = 0; x < allRanks.GetLength(0); x++)
            {
                    try
                    {
                        var roles = Context.Guild.Roles.FirstOrDefault(y => y.Name.ToLower() == allRanks[x]);
                        if(username.Roles.Contains(roles))
                        {
                        await (username as IGuildUser).RemoveRoleAsync(roles);
                        }
                    }
                    catch(Exception)
                    {
                        //nothing 
                    }
            }
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == usedtiersolo);
            await (username as IGuildUser).AddRoleAsync(role);
           

            var embed = new EmbedBuilder();
                    embed.AddField("updating your account...",
                    "Your rank has been updated!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                

        }       
    }
}
