using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.Net;


namespace NewTestBot.Modules
{
    public class birb : ModuleBase<SocketCommandContext>
    {      
        [Command("birb")]
        public async Task Birb()
        {
            // get the JSON IMG file name
            WebClient c = new WebClient();
            var data = c.DownloadString("http://random.birb.pw/tweet.json/");
            JObject o = JObject.Parse(data);

            //Sets the string url to domain + file extension
            var url = "https://random.birb.pw/img/"+o["file"];


            var embed = new EmbedBuilder();
            embed.AddField("Your daily dose of random birbs",
            url)
            .WithImageUrl(url)
            .WithAuthor(author => { author
            .WithName("Dingo Bot")
            .WithIconUrl(Global.Birdieicon);
            })
            .WithColor(new Color(13, 255, 107))
            .WithTitle("Enjoy your Birb :bird:")
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(Global.Birdieicon);
            })
            .WithCurrentTimestamp()
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
