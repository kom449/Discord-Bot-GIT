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
    public class information : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        [Command("info", RunMode = RunMode.Async)]
        [Summary("Post information on how to connect")]
        public async Task Sendinformation()
        {
            string prefix = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(prefix);
            string p = o["cmdPrefix"].ToString();

            var embed = new EmbedBuilder();
            embed.AddField("How do i connect and get a rank?",
            ""+p+ "connect (summonername) [Really just your ingame name]\n\n" +
            "Then you type "+p+ "verify to start the verification process\n" +
            "Take the token and put it in the League client under:\n" +
            "Settings - Verification. After that press the :ok_hand:\n" +
            "under the message, to tell the bot that you put the token\n" +
            "in the league client. It will either say sucess or failed\n" +
            "If the verification failed, try restarting the League client\n" +
            "type "+p+ "verify again and paste the token into the league client again\n" +
            "If the verification was a sucess, type "+p+ "update to get your rank now\n" +
            "or wait 3 days for the next update cycle.")
            .WithAuthor(author =>{ author
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);})
            .WithThumbnailUrl(thumbnail)
            .WithColor(new Color(255, 83, 13))
            .WithTitle("Birdie Bot notification")
            .WithFooter(footer =>{ footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);})
            .WithCurrentTimestamp()
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
