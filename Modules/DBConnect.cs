using System;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;
using Discord.WebSocket;

namespace NewTestBot.Modules
{
    public class DBConnect : ModuleBase<SocketCommandContext>
    {

        [Command("Connect", RunMode = RunMode.Async)] 
        public async Task ConnectDB(string response = null,params string[] args)
        {
                //taking the response from the user, converts it to string and removing the .connect part
                if (response == null)
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "no account name was provided!")
                    .WithAuthor(author =>{ author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>{ footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                return;
                }
                string userMessage = Context.Message.ToString();
                string name = userMessage.Substring(userMessage.IndexOf(' ') + 1);

                //replacing space with "%20"
                string account = name.Replace(" ", "%20");;

                //using c for webclient connections
                WebClient c = new WebClient();

                //getting league account ID
                //using "i" and "f" for id
                //if account doesnt exist - throw error and end
                JObject f = null;
                try
                {
                    string responsename = c.DownloadString("https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + account + "?api_key=" + Global.apikey + "");
                    JObject i = JObject.Parse(responsename);
                    f = i;

                }
                catch (Exception)
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Summoner account doesnt exist!")
                    .WithAuthor(author =>{ author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>{ footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                    return;
                }

                //getting id, name and icon id from response
                var id = f["id"];
                var lolname = f["name"];
                var icon = f["profileIconId"];

                //to get the account icon, you first need to find the latest version of the game
                //which i could have written in manually - but where's the fun in that.
                // after getting the latest version of the game i use it with the DDragon api that have all the game assets
                //then the version and icon id is used in the DDragon api string to retrive the icon as a png
                string findlatestlolversion = c.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
                JArray latestlolversion = JArray.Parse(findlatestlolversion);
                var version = latestlolversion[0];
                string thumbnailURL = "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/profileicon/" + icon + ".png";


                string UserID = Context.User.Id.ToString();
                ulong resultid = Context.User.Id;
                ulong xx = Convert.ToUInt64(resultid);
                var GottenName = Context.Guild.GetUser(xx);
                var DiscordName = GottenName as SocketGuildUser;
                string Query = "INSERT INTO users_testing (Discord_Id,Discord_Name,League_Id,League_Name,Icon_Id) VALUES ('" + UserID + "','" + DiscordName + "','" + id + "','" + lolname + "','" + icon + "');";
                string Duplicate = "SELECT Discord_Id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
                string InsertDiscordName = "UPDATE users_testing SET `Discord_Name` = '" + DiscordName + "' WHERE Discord_Id like  '%" + UserID + "%';";
                string setstatus = "UPDATE users_testing SET Verified = '" + "false" + "' WHERE Discord_Id like  '%" + UserID + "%';";
                string Result;

                //sql connection and command
                MySqlConnection myconn = new MySqlConnection(Global.connect);
                MySqlCommand command = new MySqlCommand(Query, myconn);
                MySqlCommand SetStatus = new MySqlCommand(setstatus,myconn);
                MySqlCommand DuplicateCommand = new MySqlCommand(Duplicate, myconn);
                MySqlCommand InsertDiscordname = new MySqlCommand(InsertDiscordName,myconn);
                MySqlDataReader myreader;
                myconn.Open();
                Result = (string)DuplicateCommand.ExecuteScalar();

                //check for duplicate of discord Ids
                myreader = DuplicateCommand.ExecuteReader();
                if (myreader.Read())
                {
                    Result = myreader.GetString(myreader.GetOrdinal("Discord_Id"));
                }

                myconn.Close();

                //if the user already exists in the DB - just tell them and do nothing.
                if (Result == UserID)
                {
                    myconn.Open();
                    myreader = InsertDiscordname.ExecuteReader();
                    myconn.Close();
                    
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your League and Discord account already exist in the Database!")
                    .WithAuthor(author =>{ author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>{ footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                }
                //if they dont exist - open connection and run the command to push to DB.
                else
                {
                    //sends all info to DB
                    myconn.Open();
                    myreader = command.ExecuteReader();
                    myconn.Close();

                try
                {
                    //sets the user verification status to false
                    myconn.Open();
                    myreader = SetStatus.ExecuteReader();
                    myconn.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }


                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your league account " + "`" + lolname + "`"+" has been added to the Database!")
                    .WithAuthor(author =>{ author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>{ footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed);
                    Console.WriteLine(Context.User.Username + " Just added their lol account: " + lolname + " To the Database!");
                    await Task.Delay(5000);
                    var messages = await this.Context.Channel.GetMessagesAsync(2).Flatten();                 
                    await Context.Channel.DeleteMessagesAsync(messages);


                }
        }
    }
}
