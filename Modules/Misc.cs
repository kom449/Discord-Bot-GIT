using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace NewTestBot.Modules
{

    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("bab")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("BAB OF DOOM");
            embed.WithDescription(message + " Has been bab'd by " + Context.User.Username);
            embed.WithColor(new Color(253, 246, 56));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
        [Command("kick")]
        [RequireBotPermission(Discord.GuildPermission.KickMembers)]
        [RequireUserPermission(Discord.GuildPermission.KickMembers)]
        public async Task KickAsync(Discord.IGuildUser user, [Remainder] string reason)
        {
            if (user.GuildPermissions.KickMembers)
            {
                var b = new Discord.EmbedBuilder();
                b.WithTitle("User Kicked");
                b.WithDescription(user.Username + "was kicked.");
                await Context.Channel.SendMessageAsync("", false, b);
                await user.KickAsync();
            }
        }
//---------------------------------------------------------------------------------------------------------
        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Welcome our newest member - " + user.Mention);
            embed.WithDescription("Remember to read our rules and have a nice stay, " + user.Mention);
            embed.WithColor(new Color(0, 0, 255));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
        //---------------------------------------------------------------------------------------------------------
        [Command("kaffe")]
        public async Task Kaffe([Remainder]int x)
        {
            
            
            
            string json = File.ReadAllText("SystemLang/kaffe.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            x = data.ToObject<Dictionary<string, int>>();
            



        
            

            var embed = new EmbedBuilder();
            embed.WithTitle("Hvor mange kopper kaffe er der blevet drukket?");
            embed.WithDescription("Der er blevet drukket: " + x + " kopper kaffe!");
            embed.WithColor(new Color(139, 69, 19));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
        
    }
}
