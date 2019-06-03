using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using System;
using Discord.WebSocket;

namespace NewTestBot.Modules
{
    public class Rolebypass : ModuleBase<SocketCommandContext>
    {
        [Command ("addrole"),RequireOwner]
        public async Task Assignrole(params string[] args)
        {
            DiscordSocketClient _client = new DiscordSocketClient();
            
            ulong guildid = 478956379143405569;
            var guild = _client.GetGuild(guildid);
            Console.WriteLine(guild);

            string userMessage = Context.Message.ToString();
            string input = userMessage.Substring(userMessage.IndexOf(' ') + 1);
            string[] inputarray = input.Split(',');
            ulong xx = Convert.ToUInt64(inputarray.First());
            var GottenName = guild.GetUser(xx);
            var username = GottenName as SocketGuildUser;
            var role = guild.Roles.FirstOrDefault(yy => yy.Name.ToLower() == inputarray.Last().ToString());
            Console.WriteLine(role);
            await (username as IGuildUser).AddRoleAsync(role);
        }

        [Command("removerole"), RequireOwner]
        public async Task Removerole(params string[] args)
        {
            DiscordSocketClient _client = new DiscordSocketClient();
            ulong guildid = 478956379143405569;
            var guild = _client.GetGuild(guildid);

            string userMessage = Context.Message.ToString();
            string input = userMessage.Substring(userMessage.IndexOf(' ') + 1);
            string[] inputarray = input.Split(',');
            ulong xx = Convert.ToUInt64(inputarray.First());
            var GottenName = guild.GetUser(xx);
            var username = GottenName as SocketGuildUser;
            var role = guild.Roles.FirstOrDefault(yy => yy.Name.ToLower() == inputarray.Last().ToString());
            await (username as IGuildUser).RemoveRoleAsync(role);
        }
    }
}
