using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace NewTestBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        //embed stuff
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        private DiscordSocketClient _client;

        //---------------------------------------------------------------------------------------------------------
        //works as intended

        [Command("bab")]
        public async Task Bab([Remainder]string message = "")
        {
            if(message == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("I NEED A FUCKING TARGET YOU RETARD");
                embed.WithColor(new Color(0, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }

            else if(message != "")
            {
                var embed = new EmbedBuilder();
                embed.AddField("Your options are:",
                "Go commit rope :thinking: \n BAB back :fist: \n Go cry in a corner :cry: ")
                .WithColor(new Color(253, 246, 56))
                .WithTitle("BABBER-BOT9000")
                .WithDescription(message + " Has been bab'd by " + Context.User.Mention)
                .WithCurrentTimestamp()
                .WithFooter(footer => { footer
                .WithText("Need help? Contact Birdie Zukira#3950")
                .WithIconUrl(IconURL);
                })
                .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }
        //---------------------------------------------------------------------------------------------------------
        //works as intended

        [Command("kick"), RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(Discord.GuildPermission.KickMembers)]

        public async Task KickUser(IGuildUser user, string reason = "")
        {
            if (reason == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No reason was provided");
                embed.WithColor(new Color(0, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
         
            else if (reason != "")
            {
                await user.KickAsync();
                var embed = new EmbedBuilder();
                embed.AddField("User kicked!",
                user + " Has been kicked from the server!" + "\n \n Reason:" + "\n \n" + reason)
                .WithColor(new Color(255, 0, 0))
                .WithAuthor(author => { author
                .WithName("Birdie Bot Nortification")
                .WithIconUrl(IconURL);
                })
                .WithCurrentTimestamp()
                .WithFooter(footer => { footer
                .WithText("Need help? Contact Birdie Zukira#3950")
                .WithIconUrl(IconURL);
                })
                .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }




        //---------------------------------------------------------------------------------------------------------
        //annouce message not working
        //idk why
        //might be something related to not detecting people joining

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            _client.UserJoined += AnnounceJoinedUser;
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
            //get unique user id
            string userId = Context.User.Id.ToString();

            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/kaffe.json");

            JObject o = JObject.Parse(fileText);

            //increment total coffee
            o["Coffee"] = (int)o["Coffee"] + 1;

            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

            //if not then add the user
            if (!userExists) {
                o["Users"][userId] = o["UserTemplate"];
            }

            //increment user coffee
            o["Users"][userId]["Coffee"] = (int)o["Users"][userId]["Coffee"] + 1;

            //turning it back into json elements
            string serialziedJson = o.ToString();

            //writing the result back into the json file with 1 added to it
            File.WriteAllText("SystemLang/kaffe.json", serialziedJson);

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + " har lige drukket en kop kaffe";
            text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Coffee"] + " kopper kaffe!";
            text += "\n\n Der er i alt blevet drukket " + o["Coffee"] + " Kopper Kaffe!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper kaffe er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee: ")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//works as intended

        [Command("kaffetotal")]
        public async Task Kaffetotal()
        {

            string fileText = File.ReadAllText("SystemLang/kaffe.json");
            dynamic results = JsonConvert.DeserializeObject<dynamic>(fileText);
            string text = "Der er blevet drukket i alt " + results.Coffee.ToString() + " Kopper Kaffe " + Context.User.Mention + "!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper kaffe er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//Works as intended

        [Command("birb")]
        public async Task Birb()
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
            .WithAuthor(author => { author
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);
            })
            .WithColor(new Color(13, 255, 107))
            .WithTitle("Enjoy your Birb :bird:")
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .WithCurrentTimestamp()
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
//---------------------------------------------------------------------------------------------------------
//works as intended

     [Command("help")]
     public async Task Help()
        {
            var embed = new EmbedBuilder();
            embed.AddField("Here is a list of my commands",
            "%bab\n %kick (Requires permissions)\n %kaffe\n %kaffetotal\n %birb\n ")
            .WithAuthor(author => { author
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);
            })
            .WithThumbnailUrl(IconURL)
            .WithColor(new Color(255, 83, 13))
            .WithTitle("Hello! my name is birdie bot and i'm here to have fun ^v^")
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .WithCurrentTimestamp()
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }


    }
}
