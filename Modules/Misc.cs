using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace NewTestBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        //embed stuff
        //readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        //readonly string thumbnailURL = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";
    
//annouce message not working
//idk why
//might be something related to not detecting people joining

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            //_client.UserJoined += AnnounceJoinedUser;
            var embed = new EmbedBuilder();
            embed.WithTitle("Welcome our newest member - " + user.Mention);
            embed.WithDescription("Remember to read our rules and have a nice stay!");
            embed.WithColor(new Color(0, 0, 255));
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }       
    }
}
