using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;



namespace NewTestBot.Modules
{
    public class help : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnailURL = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";

        [Command("help")]
        public async Task Help()
        {
            string prefix = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(prefix);
            string p = o["cmdPrefix"].ToString();

            var embed = new EmbedBuilder();
            embed.AddField("Here is a list of my commands",
            p+"bab\n"+p+"kick (Requires permissions)"+"\n"+p+"ban (Requires permissions)"+"\n"+p+"kaffe"+"\n"+p+"kaffetotal"+"\n"+p+"birb"+"\n"+p+"prefix")
            .WithAuthor(author => { author
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);
            })
            .WithThumbnailUrl(thumbnailURL)
            .WithColor(new Color(255, 83, 13))
            .WithTitle("Hello! my name is birdie bot and i'm here to have fun ^v^")
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
