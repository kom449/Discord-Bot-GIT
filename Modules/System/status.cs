using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace NewTestBot.Modules
{
    public class Status : ModuleBase<SocketCommandContext>
    {
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
                    DiscordSocketClient _client = Context.Client;
                    await _client.SetGameAsync(input);
                    await Task.CompletedTask;

                 var embed = new EmbedBuilder();
                 embed.AddField("Game status has been changed!",
                 "game status has been changed to: "+"**"+input+"**")
                 .WithAuthor(author => { author
                 .WithName("Birdie Bot")
                 .WithIconUrl(Global.Birdieicon);
                 })
                 .WithThumbnailUrl(Global.Birdiethumbnail)
                 .WithColor(new Color(255, 83, 13))
                 .WithTitle("Birdie Bot nortification")
                 .WithFooter(footer => { footer
                 .WithText(Global.Botcreatorname)
                 .WithIconUrl(Global.Birdieicon);
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
