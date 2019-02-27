using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace NewTestBot.Modules
{

    public class Misc : ModuleBase<SocketCommandContext>
    {
        //depencies
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        //---------------------------------------------------------------------------------------------------------
        //command works fine

        [Command("bab")]
        public async Task Bab([Remainder]string message)
        {
            var embed = new EmbedBuilder();
                embed.AddField("Your options are:",
                "Go commit rope :thinking: \n BAB back :fist: \n Go cry in a corner :cry: ")
                .WithColor(new Color(253, 246, 56))
                .WithTitle("BABBER-BOT9000")
                .WithDescription(message + " Has been bab'd by " + Context.User.Username)
                .WithCurrentTimestamp()
                .WithFooter(footer => {footer
                .WithText("Birdie Zukira")
                .WithIconUrl(IconURL);
                })
                .Build();
                      
                await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//kinda works, it checks the user sending the command if he has permissions but doesnt kick the target
//console doesnt say anything when the command is issued correctly

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
//annouce message not working
//idk why

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Welcome our newest member - " + user.Mention);
            embed.WithDescription("Remember to read our rules and have a nice stay, " + user.Mention);
            embed.WithColor(new Color(0, 0, 255));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//works as intended

        [Command("kaffe")]
        public async Task Kaffe()
        {
            string fileText = File.ReadAllText("SystemLang/kaffe.json");
            Debug.WriteLine(fileText);

            dynamic results = JsonConvert.DeserializeObject<dynamic>(fileText);
            results.Coffee.Value++;

            string serialziedJson = JsonConvert.SerializeObject(results);
            File.WriteAllText("SystemLang/kaffe.json", serialziedJson);


            var embed = new EmbedBuilder();
            embed.WithTitle("Hvor mange kopper kaffe er der blevet drukket?");
            embed.WithDescription("Der er blevet drukket: " + results.Coffee.ToString() + " kopper kaffe!");
            embed.WithColor(new Color(139, 69, 19));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//Works as intended

        [Command("birb")]
        public async Task birb()
        {
            // get the JSON IMG file name
            WebClient c = new WebClient();
            var data = c.DownloadString("http://random.birb.pw/tweet.json/");
            JObject o = JObject.Parse(data);

            //Sets the string url to domain + file extension
            var url = "https://random.birb.pw/img/"+o["file"];


            var embed = new EmbedBuilder();
            embed.AddField("Your daily dose of random birbs",
            url)
            .WithImageUrl(url)
            .WithAuthor(Context.Client.CurrentUser)
            .WithColor(new Color(13, 255, 107))
            .WithTitle("Enjoy your Birb")
            .WithFooter(footer => {footer
            .WithText("Birdie Zukira")
            .WithIconUrl(IconURL);
            })
            .WithCurrentTimestamp()
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }



    }
}
