﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Diagnostics;

namespace NewTestBot.Modules
{
    public class Babv2 : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        [Command("babv2"),RequireOwner]
        public async Task Bab(string user)
        {

            //foreach (SocketGuildUser user in Context.Guild.Users)
            var users = Context.Guild.Users;
            Debug.WriteLine(users);

            
                if (user == users.ToString())
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Your options are:",
                    "Go commit rope :thinking: \n BAB back :fist: \n Go cry in a corner :cry: ")
                    .WithColor(new Color(253, 246, 56))
                    .WithTitle("BABBER-BOT9000")
                    .WithDescription(user + " Has been bab'd by " + Context.User.Mention)
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText(Global.Botcreatorname)
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                     await Context.Channel.SendMessageAsync("", false, embed);
                }
                else if (user != Context.User.ToString())
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Syntax Error");
                    embed.WithDescription("I NEED A FUCKING TARGET YOU RETARD");
                    embed.WithColor(new Color(255, 0, 0));

                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            
        }
    }
}
