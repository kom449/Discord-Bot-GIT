using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace NewTestBot.Modules
{
    public class babv2 : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        [Command("babv2")]
        public async Task Bab(string message)
        {

            foreach (SocketGuildUser user in Context.Guild.Users)
            {
                if (Context.IsPrivate)
                {
                    await ReplyAsync("Cant call command from a direct message");
                    return;
                }

                if (message == Context.User.ToString())
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Your options are:",
                    "Go commit rope :thinking: \n BAB back :fist: \n Go cry in a corner :cry: ")
                    .WithColor(new Color(253, 246, 56))
                    .WithTitle("BABBER-BOT9000")
                    .WithDescription(user + " Has been bab'd by " + Context.User.Mention)
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                    // await Context.Channel.SendMessageAsync("", false, embed);
                }
                else if (message != Context.User.ToString())
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Syntax Error");
                    embed.WithDescription("I NEED A FUCKING TARGET YOU RETARD");
                    embed.WithColor(new Color(255, 0, 0));

                    //await Context.Channel.SendMessageAsync("", false, embed);
                }
            }
        }
    }
}
