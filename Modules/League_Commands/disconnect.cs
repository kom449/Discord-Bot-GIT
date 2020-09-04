using System;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace NewTestBot.Modules
{
    public class Disconnect : ModuleBase<SocketCommandContext>
    {
        [Command("disconnect", RunMode = RunMode.Async)]
        public async Task RemoveAccount()
        {
                //removing the account from the DB 
                string UserID = Context.User.Id.ToString();
                string Query = "DELETE FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
                string getID = "SELECT League_id FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
                string id = null;
                string data;
                
                MySqlConnection myconn = new MySqlConnection(Global.connect);
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

                string responserank = null;

                WebClient c = new WebClient();
                try
                {
                    responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/" + id + "?api_key=" + Global.apikey + "");
                }
                catch (Exception)
                {
                    var embed2 = new EmbedBuilder();
                    embed2.AddField("Removing your account...",
                    "failed to remove your account \n no accounts are linked to your discord!")
                    .WithAuthor(author =>{ author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>{ footer
                    .WithText(Global.Botcreatorname)
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed2);
                    await Task.Delay(5000);
                    var messages2 = await Context.Channel.GetMessagesAsync(2).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages2);
                return;
                }
                //getting league rank from ID
                //using "r" for rank

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
                .WithAuthor(author =>{ author
                .WithName("Birdie Bot")
                .WithIconUrl(Global.Birdieicon);})
                .WithThumbnailUrl(Global.Birdiethumbnail)
                .WithColor(new Color(255, 83, 13))
                .WithTitle("Birdie Bot notification")
                .WithFooter(footer =>{ footer
                .WithText(Global.Botcreatorname)
                .WithIconUrl(Global.Birdieicon);})
                .WithCurrentTimestamp()
                .Build();

                //removing their role and giving them unranked
                var username = Context.User;
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == tierused);
                var UnrankedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "unranked");
                await (username as IGuildUser).RemoveRoleAsync(role);
                await (username as IGuildUser).AddRoleAsync(UnrankedRole);
                await Context.Channel.SendMessageAsync("", false, embed);
                Console.WriteLine("Removed role from "+Context.User.Username);
                await Task.Delay(5000);
                var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                await Context.Channel.DeleteMessagesAsync(messages);

        }
    }
}
