using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace DingoBot.Modules
{
    public class ban : ModuleBase<SocketCommandContext>
    {
        [Command("ban"),RequireUserPermission(GuildPermission.Administrator),]
        [RequireBotPermission(GuildPermission.BanMembers)]

        public async Task Banuser(IGuildUser user, string reason = "")   
        {
            if (reason == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No reason was provided");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
         
            else if (reason != "")
            {
                await Context.Guild.AddBanAsync(user,0,reason);

                var embed = new EmbedBuilder();
                embed.AddField("User banned!",
                user + " Has been banned from the server!" + "\n \n Reason:" + "\n \n" + reason)
                .WithColor(new Color(255, 0, 0))
                .WithAuthor(author => { author
                .WithName("Dingo Bot nortification")
                .WithIconUrl(Global.Birdieicon);
                })
                .WithCurrentTimestamp()
                .WithFooter(footer => { footer
                .WithText("Need help? Contact Birdie Zukira#3950")
                .WithIconUrl(Global.Birdieicon);
                })
                .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }
    }
}
