using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;



namespace NewTestBot.Modules
{
    class kaffe : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        [Command("kaffe"),RequireOwner]
        public async Task Kaffe()
        {
            //get unique user id
            string userId = Context.User.Id.ToString();

            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/kaffe.json");

            JObject o = JObject.Parse(fileText);

            //increment total coffee
            o["Coffee"] = (int)o["Coffee"] + 1;

            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

            //if not then add the user
            if (!userExists) {
                o["Users"][userId] = o["UserTemplate"];
            }

            //increment user coffee
            o["Users"][userId]["Coffee"] = (int)o["Users"][userId]["Coffee"] + 1;

            //turning it back into json elements
            string serialziedJson = o.ToString();

            //writing the result back into the json file with 1 added to it
            File.WriteAllText("SystemLang/kaffe.json", serialziedJson);

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + " har lige drukket en kop kaffe";
            text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Coffee"] + " kopper kaffe!";
            

            var embed = new EmbedBuilder();
            embed.AddField(Context.User.Username+" - the great kaffe bæller!",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee: ")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
          [Command("kaffetotal"),RequireOwner]
        public async Task Kaffetotal()
        {

            //get unique user id
            string userId = Context.User.Id.ToString();

            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/kaffe.json");

            JObject o = JObject.Parse(fileText);

            //increment total coffee
            o["Coffee"] = (int)o["Coffee"];

            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

            //if not then add the user
            if (!userExists)
            {
                o["Users"][userId] = o["UserTemplate"];
            }

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange kopper kaffe er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Coffee"] + " kopper kaffe i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Coffee"] + " Kopper Kaffe!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper kaffe er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
