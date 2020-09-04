using System.Threading.Tasks;
using Discord;
using Discord.Commands;

using System.Linq;

namespace NewTestBot.Modules
{
    public class kick : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

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
                .WithText(Global.Botcreatorname)
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
                .WithText(Global.Botcreatorname)
                .WithIconUrl(IconURL);
                })
                .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }

    }
}
