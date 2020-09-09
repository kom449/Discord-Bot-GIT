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
            string getRank = "SELECT SOLO_QUEUE FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
            string rank = null;
            string data;

            MySqlConnection myconn = new MySqlConnection(Global.connect);
            MySqlCommand command = new MySqlCommand(Query, myconn);
            MySqlCommand fetchRank = new MySqlCommand(getRank, myconn);
            MySqlDataReader myreader;
            MySqlDataReader myreader2;


            //first connection
            myconn.Open();
            myreader2 = fetchRank.ExecuteReader();
            while (myreader2.Read())
            {
                data = myreader2.GetString(0);
                rank = data;
            }
            myconn.Close();
            string[] rankarray = rank.Split(' ');

            //second connection
            myconn.Open();
            myreader = command.ExecuteReader();
            myconn.Close();

            //using a for loop to check all the bodies of the json
            //since each queue type is in another body
            var embed = new EmbedBuilder();
            embed.AddField("Removing your account...",
            "Your account has been removed!")
            .WithAuthor(author =>
            {author
            .WithName("Birdie Bot")
            .WithIconUrl(Global.Birdieicon);
            })
            .WithThumbnailUrl(Global.Birdiethumbnail)
            .WithColor(new Color(255, 83, 13))
            .WithTitle("Birdie Bot notification")
            .WithFooter(footer =>
            {footer
            .WithText(Global.Botcreatorname)
            .WithIconUrl(Global.Birdieicon);
            })
            .WithCurrentTimestamp()
            .Build();

            //removing their role and giving them unranked
            var username = Context.User;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == rankarray[0].ToLower());                
            var UnrankedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "unranked");
            await (username as IGuildUser).RemoveRoleAsync(role);
            await (username as IGuildUser).AddRoleAsync(UnrankedRole);
            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine("Removed role and data from " + Context.User.Username);
            await Task.Delay(5000);
            var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

        }
    }
}
