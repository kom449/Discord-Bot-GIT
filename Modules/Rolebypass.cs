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
                DiscordSocketClient _client = Context.Client;
                
                //currently set for Serenity gaming discord server
                ulong guildid = 315967505422090260;
                var guild = _client.GetGuild(guildid);

                string userMessage = Context.Message.ToString();
                string input = userMessage.Substring(userMessage.IndexOf(' ') + 1);
                string[] inputarray = input.Split(',');
                ulong xx = Convert.ToUInt64(inputarray.First());
                var GottenName = guild.GetUser(xx);
                var username = GottenName as SocketGuildUser;
                var role = guild.Roles.FirstOrDefault(yy => yy.Name.ToLower() == inputarray.Last().ToString());
                await (username as IGuildUser).AddRoleAsync(role);
                Console.WriteLine("Assigned role to "+username);
        }

        [Command("removerole"), RequireOwner]
        public async Task Removerole(params string[] args)
        {
                DiscordSocketClient _client = Context.Client;
                
                //currently set for Serenity gaming discord server
                ulong guildid = 315967505422090260;
                var guild = _client.GetGuild(guildid);

                string userMessage = Context.Message.ToString();
                string input = userMessage.Substring(userMessage.IndexOf(' ') + 1);
                string[] inputarray = input.Split(',');
                ulong xx = Convert.ToUInt64(inputarray.First());
                var GottenName = guild.GetUser(xx);
                var username = GottenName as SocketGuildUser;
                var role = guild.Roles.FirstOrDefault(yy => yy.Name.ToLower() == inputarray.Last().ToString());
                await (username as IGuildUser).RemoveRoleAsync(role);
                Console.WriteLine("Removed role from " + username);
        }
    }
}
