using System.Threading.Tasks;
using Discord;
using Discord.Commands;

using System.Linq;

namespace DingoBot.Modules
{
    public class kick : ModuleBase<SocketCommandContext>
    {
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
