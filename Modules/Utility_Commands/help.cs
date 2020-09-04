using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord.Rest;



namespace NewTestBot.Modules
{
    public class help : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("help")]
        public async Task Help()
        {
            string prefix = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(prefix);
            string p = o["cmdPrefix"].ToString();

            var embed = new EmbedBuilder();
            embed.AddField("Here is a list of my commands!",
            p+"bab\n"+p+"kick (Requires permissions)"+"\n"+p+"ban (Requires permissions)"+"\n"+p+"drink"+"\n"+p+"drinktotal"+"" +
            "\n"+p+"drinkleader"+"\n"+p+"drinklist"+"\n"+p+"birb"+"\n"+p+ "prefix (Requires permissions)" + "\n"+p+"purge"+"\n"+p+"status"+
            "\n"+p+"calc"+"\n"+ "\n" + p + "connect" + "\n"+ "\n" + p + "disconnect" + "\n"+ "\n" + p + "update" + "\n"+ "\n" + p + "create (requires permissions)" + "\n" + "\n"+
            "Current version of Birdie bot is: "+"***"+Global.version+"***")
            .WithAuthor(author => { author
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);
            })
            .WithThumbnailUrl(Global.Birdiethumbnail)
            .WithColor(new Color(255, 83, 13))
            .WithTitle("Hello! my name is birdie bot and i'm here to have fun ^v^")
            .WithFooter(footer => { footer
            .WithText(Global.Botcreatorname)
            .WithIconUrl(IconURL);
            })
            .WithCurrentTimestamp()
            .Build();
            //RestUserMessage msg = Context.Channel.SendMessageAsync("",false,embed);
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
