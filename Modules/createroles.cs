using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace NewTestBot.Modules
{
    public class Createroles : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("create", RunMode = RunMode.Async),RequireOwner]
        public async Task Rolecreation()
        {
            try { 

            var allRanks = new[] { "Challenger", "GrandMaster", "Master", "Diamond", "Platinum", "Gold", "Silver", "Bronze", "Iron","Unranked", "New ones :)" };
            var allrankcolors = new[] { new Color(240,140,15), new Color(253,7,7), new Color(192,7,146),new Color(32,102,148),new Color(46,204,113),new Color(241,196,15),new Color(151,156,159),new Color(187,121,68),new Color(255,255,255),new Color(124, 136, 120),new Color(188, 157, 154)};

                    //my int for the color array
                    int y = 0;
        
                    //running through all the different roles and create them
                    for (int x = 0; x < allRanks.GetLength(0); x++, y++)
                    {
                           GuildPermissions permissions = default;
                           bool ishoisted = true;
                           RequestOptions options = null;
                           await Context.Guild.CreateRoleAsync(allRanks[x], permissions, allrankcolors[y], ishoisted, options);

                    }

                    var embed = new EmbedBuilder();
                    embed.AddField("Creating roles for you now...",
                    "All of the rank roles has been created!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                
            
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        } 
    }
}
