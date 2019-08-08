﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace NewTestBot.Modules
{
    public class Createroles : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("create", RunMode = RunMode.Async),RequireOwner]
        public async Task Rolecreation()
        {
            try
            { 
                    var allRanks = new[] { "challenger", "grandMaster", "master", "diamond", "platinum", "gold", "silver", "bronze", "iron","unranked", "new ones :)" };
                    var allrankcolors = new[] { new Color(240,140,15), new Color(253,7,7), new Color(192,7,146),new Color(32,102,148),new Color(46,204,113),new Color(241,196,15),new Color(151,156,159),new Color(187,121,68),new Color(255,255,255),new Color(124, 136, 120),new Color(188, 157, 154)};
                    string channelname = "Birdie-Connect";

                    //my int for the color array
                    int y = 0;                    

                    //running through all the different roles and create them
                    //somehow when it checks for roles, it returns null everytime. But it only does it here?
                    for (int x = 0; x < allRanks.GetLength(0); x++, y++)
                    {
                            //checking if the role from allranks array can be found on the server
                            var role = Context.Guild.Roles.FirstOrDefault(z => z.Name.ToLower() == allRanks[x]);                            
                            if (role == null)
                            {
                                GuildPermissions permissions = default;
                                bool ishoisted = true;
                                RequestOptions options = null;
                                Console.WriteLine("Creating Role " + allRanks[x] + " On the server " + Context.Guild.Name+"!");
                                await Context.Guild.CreateRoleAsync(allRanks[x], permissions, allrankcolors[y], ishoisted, options);
                                
                            }

                            //else if the rolecheck matches the current role then it must exist on server
                            else if (role != null)
                            {
                                Console.WriteLine(allRanks[x] + " Already exists on the the server " + Context.Guild.Name+"!");
                                continue;
                            }
                    }

                    //check if there is a channel with the name "channelname"
                    int totalchannels = Context.Guild.Channels.Count;
                    int counter = 0;
                    bool exist = false;
                    foreach (SocketGuildChannel channel in Context.Guild.Channels)
                    {
                        counter++;
                            //if channel is found - notify in console
                            if (channelname.ToLower() == channel.Name)
                            {
                                Console.WriteLine(channelname + " Already exists on the the server " + Context.Guild.Name+"!");
                                exist = true;
                            }

                            //if no channel was found - create it
                            if (counter == totalchannels && exist == false)
                            {
                                RequestOptions channeloptions = null;
                                await Context.Guild.CreateTextChannelAsync(channelname, channeloptions);
                                Console.WriteLine(channelname + " Has been created on the server " + Context.Guild.Name + "!");
                            }
                    }

                    var embed = new EmbedBuilder();
                    embed.AddField("Creating roles for you now...",
                    "please hold on while i create roles and channel for you!")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 0, 0))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();                

                    var message = await Context.Channel.SendMessageAsync("", false, embed);

                    await Task.Delay(5000);
                    await message.ModifyAsync(x =>
                    {
                        x.Embed = new EmbedBuilder()
                        .AddField("All roles and channels has been created!",
                        "Feel free to move the channel as you please!")
                        .WithAuthor(author => { author
                        .WithName("Birdie Bot")
                        .WithIconUrl(IconURL);})
                        .WithThumbnailUrl(thumbnail)
                        .WithColor(new Color(0, 255, 0))
                        .WithTitle("Birdie Bot notification")
                        .WithFooter(footer => { footer
                        .WithText("Need help? Contact Birdie Zukira#3950")
                        .WithIconUrl(IconURL);})
                        .WithCurrentTimestamp()
                        .Build();
                    });
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        } 
    }
}
