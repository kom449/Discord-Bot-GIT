using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace DingoBot.Modules
{
    public class Prefix : ModuleBase<SocketCommandContext>
    {
        [Command ("prefix"),RequireOwner]
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
                    .WithName("Dingo Bot")
                    .WithIconUrl(Global.Birdieicon);
                    })
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Dingo Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);
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
