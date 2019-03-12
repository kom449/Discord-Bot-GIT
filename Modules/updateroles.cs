using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace NewTestBot.Modules
{
    public class Updateroles : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnailURL = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";

        [Command("updateroles"),RequireOwner]
        public async Task Updaterole()
        {
            
                //getting the JSON file and turning it into a JSON object
                string role = File.ReadAllText("SystemLang/users.json");
                JObject o = JObject.Parse(role);

                //string to save the userID and roleID
                string userId = Context.User.Id.ToString();
                Debug.WriteLine(userId);
                string roleId = "";


            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

                //if not then add the user
                if (!userExists)
                {
                    o["Users"][userId] = o["UserTemplate"];
                }

                //saving the roleID into the userID
                o["Users"][userId]["role"] = (string)o["Users"][userId][roleId];

                //writing all new stuff into the JSON file
                string gatheredroles = o.ToString();
                File.WriteAllText("SystemLang/users.json", gatheredroles);
            

                    var embed = new EmbedBuilder();
                    embed.AddField("**"+"Role updater!"+"**",
                    "Updating user roles and saving them to archive!")
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
        }
    }
}
