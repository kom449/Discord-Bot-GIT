using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace NewTestBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Message by " + Context.User.Username);
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
