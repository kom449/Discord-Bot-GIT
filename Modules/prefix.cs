using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace NewTestBot.Modules
{
    public class Prefix : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnailURL = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";

        [Command ("prefix"),RequireUserPermission(GuildPermission.Administrator),RequireOwner]
         public async Task Prefixchange(string input = "")
         {
            if (input == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No new prefix was provided");
                embed.WithColor(new Color(255, 0, 0));
            }

            else if (input != "")
            {
                 try
                 {
                    string prefix = File.ReadAllText("Resources/config.json");
                    JObject o = JObject.Parse(prefix);

                    string visual = o["cmdPrefix"].ToString();

                    o["cmdPrefix"] = (string)o["cmdPrefix"];
                    o["cmdPrefix"] = input;

                    string xinput = o.ToString();

                    File.WriteAllText("Resources/config.json", xinput);

                    var embed = new EmbedBuilder();
                    embed.AddField("Prefix has been changed to: " + "**"+input+"**",
                    "The old prefix was: " + visual)
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
 
                    Process.Start("NewTestBot.exe");
                    Process.GetCurrentProcess().Kill();
                 }
                 catch (System.Exception e)
                 {
                    Debug.WriteLine(e);
                 }
            }
          
        }
    }
}
