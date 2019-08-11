using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;


namespace NewTestBot.Modules
{
    public class restart : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnailURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        [Command("restart"), RequireOwner]
        public async Task PerformRestart()
        {
                    var embed = new EmbedBuilder();
                    embed.AddField("Restart request has been sent!",
                    "The bot will restart in a moment.")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed);
 
                    Process.Start("NewTestBot.exe");
                    Process.GetCurrentProcess().Kill();
        }
    }
}
