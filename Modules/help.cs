using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord.Rest;



namespace DingoBot.Modules
{
    public class help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            string prefix = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(prefix);
            string p = o["cmdPrefix"].ToString();
            string version = o["version"].ToString();

            var embed = new EmbedBuilder();
            embed.AddField("Here is a list of my commands",

            p+"bab\n"
            +p+"kick (Requires permissions)"
            +"\n"+p+"ban (Requires permissions)"+
            "\n"+p+"birb"+"\n"+p+"prefix"+"\n"+p+"purge"+"\n"+p+"status"+
            "\n"+p+"calc"+"\n" + "\n" + "\n"+
            "Current version of Birdie bot is: "+"***"+version+"***")
            .WithAuthor(author => { author
            .WithName("Dingo Bot")
            .WithIconUrl(Global.Birdieicon);
            })
            .WithThumbnailUrl(Global.Birdiethumbnail)
            .WithColor(new Color(255, 83, 13))
            .WithTitle("Hello! my name is Birdie bot and i'm here to have fun ^v^")
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(Global.Birdieicon);
            })
            .WithCurrentTimestamp()
            .Build();
            //RestUserMessage msg = Context.Channel.SendMessageAsync("",false,embed);
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
