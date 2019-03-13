using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace NewTestBot.Modules
{
    public class ban : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        [Command("ban"),RequireUserPermission(Discord.GuildPermission.BanMembers), RequireOwner]
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
                .WithName("Birdie Bot nortification")
                .WithIconUrl(IconURL);
                })
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
}
