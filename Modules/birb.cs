using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.Net;


namespace NewTestBot.Modules
{
    public class birb : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";


        
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
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);
            })
            .WithColor(new Color(13, 255, 107))
            .WithTitle("Enjoy your Birb :bird:")
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
