using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace NewTestBot.Modules
{
    public class Updateroles : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnailURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("updateroles"),RequireOwner]
        public async Task Updaterole()
        {
            
                //getting the JSON file and turning it into a JSON object
                string data = File.ReadAllText("SystemLang/users.json");
                JObject o = JObject.Parse(data);

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
