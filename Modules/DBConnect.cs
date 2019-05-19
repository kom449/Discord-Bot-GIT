﻿using System;
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
//make it check if the roles already exists before creating them to avoid duplicates
//add guild id and guild name, so you can see what guild the user is connected from

namespace NewTestBot.Modules
{
    public class DBConnect : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("Connect", RunMode = RunMode.Async)] 
        public async Task ConnectDB(string response = null,params string[] args)
        {
                //taking the response from the user, converts it to string and removing the .connect part
                if (response == null)
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "no account name was provided!")
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

                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(2000);
                    var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                return;
                }
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
                string DiscordName = Context.User.Username;
                string Query = "INSERT INTO users_testing (Discord_Id,Discord_Name,League_Id,League_Name,Icon_Id) VALUES ('" + UserID + "','" + DiscordName + "','" + id + "','" + lolname + "','" + icon + "');";
                string Duplicate = "SELECT Discord_Id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
                string Result;

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

                myconn.Close();

                //if the user already exists in the DB - just tell them and do nothing.
                if (Result == UserID)
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your League and Discord account already exist in the Database!")
                    .WithAuthor(author =>
                    {
                        author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
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

                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(2000);
                    var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
            }
                //if they dont exist - open connection and run the command to push to DB.
                else
                {
                    myconn.Open();
                    myreader = command.ExecuteReader();
                    myconn.Close();

                    var embed = new EmbedBuilder();
                    embed.AddField("Connecting you...",
                    "Your league account " + "`" + lolname + "`"+" has been added to the Database!")
                    .WithAuthor(author =>
                    {
                        author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
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
                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(2000);
                    var messages = await this.Context.Channel.GetMessagesAsync(2).Flatten();
                  
                    await Context.Channel.DeleteMessagesAsync(messages);


            }
        }
    }
}
