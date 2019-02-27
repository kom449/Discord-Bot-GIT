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
            {
                embed.WithTitle("BABBER-BOT9000");
                embed.WithDescription(message + " Has been bab'd by " + Context.User.Username);
            };
            embed.AddField("Your options are:",
                "Go commit rope :thinking: \n BAB back :fist: ")
                //.WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "Babber xd")
                .WithColor(new Color(253, 246, 56))
                .WithTitle("BABBER-BOT9000")
                .WithDescription(message + " Has been bab'd by " + Context.User.Username)
                .WithCurrentTimestamp()
                .Build();
            
                
                

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
//---------------------------------------------------------------------------------------------------------
        [Command("birb")]
        public async Task birb()
        {
            var url = "https://random.birb.pw/tweet/random";
            var embed = new EmbedBuilder();
            embed.AddField("Your daily dose of random birbs",
                url)
            .WithAuthor(Context.Client.CurrentUser)
            .WithColor(new Color(253, 246, 56))
            .WithTitle("Enjoy your Birb")
            .WithFooter(footer => footer.Text = "Babber xd")
            .WithCurrentTimestamp()
            .Build();



            await Context.Channel.SendMessageAsync("", false, embed);
        }



    }
}
