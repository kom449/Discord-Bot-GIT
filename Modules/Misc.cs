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
            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/kaffe.json");

            //dynamiclly Deserializes everything in the string to make it a readable object in c#
            dynamic results = JsonConvert.DeserializeObject<dynamic>(fileText);
            
            //adding 1 to the result
            results.Coffee.Value++;

            //turning it back into json elements
            string serialziedJson = JsonConvert.SerializeObject(results);

            //writing the result back into the json file with 1 added to it
            File.WriteAllText("SystemLang/kaffe.json", serialziedJson);

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Username + " har lige drukket en kop kaffe" + "\n Så der er nu blevet drukket " + results.Coffee.ToString() + " Kopper Kaffe!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper kaffe er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee: ")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Birdie Zukira")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//works as intended      
        [Command("kaffetotal")]
        public async Task kaffe()
        {

            string fileText = File.ReadAllText("SystemLang/kaffe.json");
            dynamic results = JsonConvert.DeserializeObject<dynamic>(fileText);
            string text = "Der er blevet drukket i alt " + results.Coffee.ToString() + " Kopper Kaffe " + Context.User.Username + "!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper kaffe er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Birdie Zukira")
            .WithIconUrl(IconURL);
            })
            .Build();

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
