using System.Threading.Tasks;
using Discord;
using Discord.Commands;

using System.Linq;

namespace NewTestBot.Modules
{
    public class kick : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        [Command("kick"),RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]

        public async Task KickUser(IGuildUser user, string reason = "")
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
                var invite = await Context.Guild.GetInvitesAsync();
                await user.SendMessageAsync(invite.Select(x => x.Url).FirstOrDefault());
                await user.KickAsync();


                var embed = new EmbedBuilder();
                embed.AddField("User kicked!",
                user + " Has been kicked from the server!" + "\n \n Reason:" + "\n \n" + "**"+reason+"**")
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
//---------------------------------------------------------------------------------------------------------

               [RequireOwner]
        [Command("kickoverride")]
        [RequireBotPermission(GuildPermission.KickMembers)]

        public async Task KickUserbyowner(IGuildUser user, string reason = "")
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
                var invite = await Context.Guild.GetInvitesAsync();
                await user.SendMessageAsync(invite.Select(x => x.Url).FirstOrDefault());
                await user.KickAsync();


                var embed = new EmbedBuilder();
                embed.AddField("User kicked!",
                user + " Has been kicked from the server!" + "\n \n Reason:" + "\n \n" + reason)
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
