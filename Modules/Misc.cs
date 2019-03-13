using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace NewTestBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {    
//annouce message not working
//idk why
//might be something related to not detecting people joining

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Welcome our newest member - " + user.Mention);
            embed.WithDescription("Remember to read our rules and have a nice stay!");
            embed.WithColor(new Color(0, 0, 255));
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }       
    }
}
