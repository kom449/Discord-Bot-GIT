﻿using System.Threading.Tasks;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Linq;

namespace NewTestBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        //embed stuff
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnailURL = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";
        DiscordSocketClient _client;

        //---------------------------------------------------------------------------------------------------------
        //works as intended

        [Command("bab")]
        public async Task Bab(string message = "")
        {


            if(message == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("I NEED A FUCKING TARGET YOU RETARD");
                embed.WithColor(new Color(255, 0, 0));

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
        [Command("kick"),RequireUserPermission(Discord.GuildPermission.KickMembers)]
        [RequireBotPermission(Discord.GuildPermission.KickMembers)]

        public async Task KickUser(IGuildUser user, string reason = "")
        {
            if (reason == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No reason was provided");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
         
            else if (reason != "")
            {
                var invite = await Context.Guild.GetInvitesAsync();
                await user.SendMessageAsync(invite.Select(x => x.Url).FirstOrDefault());
                await user.KickAsync();


                var embed = new EmbedBuilder();
                embed.AddField("User kicked!",
                user + " Has been kicked from the server!" + "\n \n Reason:" + "\n \n" + "**"+reason+"**")
                .WithColor(new Color(255, 0, 0))
                .WithAuthor(author => { author
                .WithName("Birdie Bot nortification")
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
//Command override by bot owner - makes it possible for me to invoke through permissions
        [RequireOwner]
        [Command("kickoverride")]
        [RequireBotPermission(Discord.GuildPermission.KickMembers)]

        public async Task KickUserbyowner(IGuildUser user, string reason = "")
        {
            if (reason == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No reason was provided");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
         
            else if (reason != "")
            {
                var invite = await Context.Guild.GetInvitesAsync();
                await user.SendMessageAsync(invite.Select(x => x.Url).FirstOrDefault());
                await user.KickAsync();


                var embed = new EmbedBuilder();
                embed.AddField("User kicked!",
                user + " Has been kicked from the server!" + "\n \n Reason:" + "\n \n" + reason)
                .WithColor(new Color(255, 0, 0))
                .WithAuthor(author => { author
                .WithName("Birdie Bot nortification")
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
//not tested yet
//should be working though

        [RequireOwner]
        [Command("ban"),RequireUserPermission(Discord.GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]

        public async Task Banuser(IGuildUser user, string reason = "")   
        {
            if (reason == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No reason was provided");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
         
            else if (reason != "")
            {
                await Context.Guild.AddBanAsync(user,0,reason);

                var embed = new EmbedBuilder();
                embed.AddField("User banned!",
                user + " Has been banned from the server!" + "\n \n Reason:" + "\n \n" + reason)
                .WithColor(new Color(255, 0, 0))
                .WithAuthor(author => { author
                .WithName("Birdie Bot nortification")
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
            //_client.UserJoined += AnnounceJoinedUser;
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
            

            var embed = new EmbedBuilder();
            embed.AddField(Context.User.Username+" - the great kaffe bæller!",
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

            //get unique user id
            string userId = Context.User.Id.ToString();

            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/kaffe.json");

            JObject o = JObject.Parse(fileText);

            //increment total coffee
            o["Coffee"] = (int)o["Coffee"];

            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

            //if not then add the user
            if (!userExists)
            {
                o["Users"][userId] = o["UserTemplate"];
            }

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange kopper kaffe er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Coffee"] + " kopper kaffe i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Coffee"] + " Kopper Kaffe!";

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
            string prefix = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(prefix);
            string p = o["cmdPrefix"].ToString();

            var embed = new EmbedBuilder();
            embed.AddField("Here is a list of my commands",
            p+"bab\n"+p+"kick (Requires permissions)"+"\n"+p+"ban (Requires permissions)"+"\n"+p+"kaffe"+"\n"+p+"kaffetotal"+"\n"+p+"birb"+"\n"+p+"prefix")
            .WithAuthor(author => { author
            .WithName("Birdie Bot")
            .WithIconUrl(IconURL);
            })
            .WithThumbnailUrl(thumbnailURL)
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

//---------------------------------------------------------------------------------------------------------
//kinda works as intended
//it restarts the application when the prefix it changed
//since when you update the prefix, the new one still dont work unless the whole bot is reloaded
         
         [Command ("prefix"),RequireUserPermission(GuildPermission.Administrator),RequireOwner]
         public async Task Prefix(string input = "")
         {
            if (input == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No new prefix was provided");
                embed.WithColor(new Color(255, 0, 0));
            }

            else if (input != "")
            {
                 try
                 {
                    string prefix = File.ReadAllText("Resources/config.json");
                    JObject o = JObject.Parse(prefix);

                    string visual = o["cmdPrefix"].ToString();

                    o["cmdPrefix"] = (string)o["cmdPrefix"];
                    o["cmdPrefix"] = input;

                    string xinput = o.ToString();

                    File.WriteAllText("Resources/config.json", xinput);

                    var embed = new EmbedBuilder();
                    embed.AddField("Prefix has been changed to: " + "**"+input+"**",
                    "The old prefix was: " + visual)
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
 
                    Process.Start("NewTestBot.exe");
                    Process.GetCurrentProcess().Kill();
                 }
                 catch (System.Exception e)
                 {
                    Debug.WriteLine(e);
                 }
            }
          
        }

//---------------------------------------------------------------------------------------------------------
//working on it

        [Command("updateroles")]
        public async Task Updateroles()
        {

            //getting the JSON file and turning it into a JSON object
            string role = File.ReadAllText("SystemLang/users.json");
            JObject o = JObject.Parse(role);

            //string to save the userID and roleID
            string userId = Context.User.Id.ToString();
            
            string roleId = "";

            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

            //if not then add the user
            if (!userExists)
            {
                o["Users"][userId] = o["UserTemplate"];
            }

            //saving the roleID into the userID
            o["Users"][userId]["role"] = (string)o["Users"][userId][roleId];

            //writing all new stuff into the JSON file
            string gatheredroles = o.ToString();
            File.WriteAllText("SystemLang/users.json", gatheredroles);


            var embed = new EmbedBuilder();
                    embed.AddField("**"+"Role updater!"+"**",
                    "Updating user roles and saving them to archive!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);
                    })
                    .WithThumbnailUrl(thumbnailURL)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")

                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .WithCurrentTimestamp()
                    .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

//---------------------------------------------------------------------------------------------------------
//bot owner command for changing the game status
        [Command("status"),RequireOwner]
        public async Task Changestatus(string input = "")
        {
            if (input == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No game status written");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else if (input != "")
            {

                await _client.SetGameAsync(input);
                

                var embed = new EmbedBuilder();
                 embed.AddField("Game status has been changed!",
                 "game status has been changed to"+input)
                 .WithAuthor(author => { author
                 .WithName("Birdie Bot")
                 .WithIconUrl(IconURL);
                 })
                 .WithThumbnailUrl(thumbnailURL)
                 .WithColor(new Color(255, 83, 13))
                 .WithTitle("Birdie Bot nortification")
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
}
