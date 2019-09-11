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
        [Command("restart"), RequireOwner]
        public async Task PerformRestart()
        {
                    var embed = new EmbedBuilder();
                    embed.AddField("Restart request has been sent!",
                    "The bot will restart in a moment.")
                    .WithAuthor(author => { author
                    .WithName("Dingo Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Dingo Bot nortification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();
                    await Context.Channel.SendMessageAsync("", false, embed);
 
                    Process.Start("NewTestBot.exe");
                    Process.GetCurrentProcess().Kill();
        }
    }
}
