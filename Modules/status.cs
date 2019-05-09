using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace NewTestBot.Modules
{
    public class Status : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnailURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("status"),RequireUserPermission(GuildPermission.Administrator),RequireOwner]
        public async Task Changestatus(string input = "")
        {
            if (input == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No game status written");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else if (input != "")
            {
                try
                {

                    await Context.Client.SetGameAsync(input);
                    await Task.CompletedTask;

                 var embed = new EmbedBuilder();
                 embed.AddField("Game status has been changed!",
                 "game status has been changed to: "+"**"+input+"**")
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
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                   
            }
        }
    }
}
