using System;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Discord.WebSocket;
using System.Linq;

namespace NewTestBot.Modules
{
    public class Updateall : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        [Command("updateall", RunMode = RunMode.Async),RequireOwner]
        public async Task Updateallranks()
        {
            try
            {

            WebClient c = new WebClient();
            string data = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(data);
            string apikey = (string)o["lolapi"]["apikey"];
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);
            string QueryLeagueId = "SELECT League_Id FROM users_testing";
            List<string> results = new List<string>();
            List<string> DiscordIdResults = new List<string>();
            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand LeagueQuery = new MySqlCommand(QueryLeagueId, myconn);
            MySqlDataReader myreader;
            myconn.Open();
            myreader = LeagueQuery.ExecuteReader();

                //getting the current league id
                 while (myreader.Read())
                 {
                   string leagueid;
                   leagueid = myreader.GetString(0);     
                   results.Add(leagueid);
                 }
                myconn.Close();

                //using b as the variable for discord id list
                for (int x = 0; x  < results.Count; x++)
                {
                        string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/" + results[x] + "?api_key=" + apikey + "");
                        JArray r = JArray.Parse(responserank);
                        string ranksolo = null;
                        string rankflex5 = null;
                        string rankflex3 = null;
                        string usedtiersolo = null;
                        string ResponseName = c.DownloadString("https://euw1.api.riotgames.com/lol/summoner/v4/summoners/" + results[x] + "?api_key=" + apikey + "");
                        JObject i = JObject.Parse(ResponseName);
                        var lolname = i["name"];

                        string QueryDiscordName = "SELECT Discord_Name FROM users_testing WHERE League_Id like  '%" + results[x] + "%';";
                        string QueryDiscordId = "SELECT Discord_Id FROM users_testing WHERE League_Id like  '%" + results[x] + "%';";
                        string resultid = null;
                        string resultname;
                        
                        //gettting ID from of the entry that is being updated
                        MySqlCommand GetID = new MySqlCommand(QueryDiscordId, myconn);
                        myconn.Open();
                        myreader = GetID.ExecuteReader();
                        while (myreader.Read())
                        {
                            data = myreader.GetString(0);
                            resultid = data;
                        }
                        myconn.Close();
                        
                        //part that updates the user name from the ID
                        ulong xx = Convert.ToUInt64(resultid);
                        var GottenName = Context.Guild.GetUser(xx);
                        var username = GottenName as SocketGuildUser;
                        string QueryUpdateDiscordName = "UPDATE users_testing SET Discord_Name ='" + username + "'WHERE League_Id like  '%" + results[x] + "%';";
                        MySqlCommand Updatename = new MySqlCommand(QueryUpdateDiscordName, myconn);
                        myconn.Open();
                        myreader = Updatename.ExecuteReader();
                        myconn.Close();

                    //getting the user name of the entry being updated
                    MySqlCommand GetName = new MySqlCommand(QueryDiscordName, myconn);
                        myconn.Open();
                        myreader = GetName.ExecuteReader();
                        while (myreader.Read())
                        {
                            data = myreader.GetString(0);
                            resultname = data;
                        }
                        myconn.Close();

                        //using a for loop to check all the bodies of the json
                        //since each queue type is in another body
                        for (int y = 0; y < r.Count; y++)
                        {
                            if (((string)r[y]["queueType"] == "RANKED_SOLO_5x5"))
                            {
                                var tiersolo = (string)r[y]["tier"];
                                var divisionsolo = (string)r[y]["rank"];
                                string soloq = tiersolo + " " + divisionsolo;
                                ranksolo = soloq;
                                usedtiersolo = tiersolo.ToLower();
                            }
                        }

                        //using the same loop to get the Flex 5v5 rank
                        for (int z = 0; z < r.Count; z++)
                        {
                            if (((string)r[z]["queueType"] == "RANKED_FLEX_SR"))
                            {
                                var tierflex5v5 = (string)r[z]["tier"];
                                var divisionflex5v5 = (string)r[z]["rank"];
                                string flex5v5 = tierflex5v5 + " " + divisionflex5v5;
                                rankflex5 = flex5v5;
                            }

                        }

                        //again using the same loop to find the Flex 3v3 rank
                        for (int a = 0; a < r.Count; a++)
                        {
                            if (((string)r[a]["queueType"] == "RANKED_FLEX_TT"))
                            {
                                var tierflex3v3 = (string)r[a]["tier"];
                                var divisionflex3v3 = (string)r[a]["rank"];
                                string flex3v3 = tierflex3v3 + " " + divisionflex3v3;
                                rankflex3 = flex3v3;
                            }

                        }

                            
                            string QueryUpdateRank = "UPDATE users_testing SET SOLO_QUEUE = '"+ranksolo+ "',FLEX_3V3 ='"+rankflex3+ "',FLEX_5V5 = '" + rankflex5 + "',League_Name = '" + lolname + "' WHERE League_Id like  '%" + results[x]+ "%';";
                            //sql connection and command
                            MySqlCommand postdata = new MySqlCommand(QueryUpdateRank, myconn);
                            myconn.Open();
                            myreader = postdata.ExecuteReader();
                            myconn.Close();
                    try
                    {
                        var allRanks = new[] { "challenger", "grandMaster", "master", "diamond", "platinum", "gold", "silver", "bronze", "iron", "unranked", "new ones :)" };

                        //running through all the different roles and remove the found role
                        for (int xy = 0; xy < allRanks.GetLength(0); xy++)
                        {
                            try
                            {
                                var roles = Context.Guild.Roles.FirstOrDefault(yy => yy.Name.ToLower() == allRanks[xy]);
                                if (username.Roles.Contains(roles))
                                {
                                    await (username as IGuildUser).RemoveRoleAsync(roles);
                                }
                            }
                            catch (Exception)
                            {
                                //nothing 
                            }
                        }

                        var role = Context.Guild.Roles.FirstOrDefault(xyz => xyz.Name.ToLower() == usedtiersolo);
                        await (username as IGuildUser).AddRoleAsync(role);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Couldn't assign role to lol user "+ lolname + " Since the user isn't on the discord server!\n");
                        Thread.Sleep(5000);
                        continue;
                    }


                    Console.WriteLine("Updated User: " + lolname+ "\nDiscord name: " + GottenName+ "\n");
                    Thread.Sleep(5000);

                }
                
                    var embed = new EmbedBuilder();
                    embed.AddField("updating all accounts...",
                    "all ranks has been updated!")
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
                    await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
