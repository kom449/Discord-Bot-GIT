using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace NewTestBot.Modules
{
    public class Createroles : ModuleBase<SocketCommandContext>
    {

        [Command("create", RunMode = RunMode.Async), RequireOwner]
        public async Task Rolecreation()
        {
            var embed = new EmbedBuilder();
            embed.AddField("Creating roles for you now...",
            "please hold on while i create roles and channel for you!")
            .WithAuthor(author =>{author
            .WithName("Birdie Bot")
            .WithIconUrl(Global.Birdieicon);})
            .WithThumbnailUrl(Global.Birdiethumbnail)
            .WithColor(new Color(255, 0, 0))
            .WithTitle("Birdie Bot notification")
            .WithFooter(footer =>
            {footer
            .WithText(Global.Botcreatorname)
            .WithIconUrl(Global.Birdieicon);})
            .WithCurrentTimestamp()
            .Build();
            var message = await Context.Channel.SendMessageAsync("", false, embed);


            try
            {
                var allRanks = new[] { "Challenger", "GrandMaster", "Master", "Diamond", "Platinum", "Gold", "Silver", "Bronze", "Iron", "Unranked", "New Ones :)" };
                var TFTRanks = new[] { "TFT-Challenger", "TFT-Grandmaster", "TFT-Master", "TFT-Diamond", "TFT-Platinum", "TFT-Gold", "TFT-Silver", "TFT-Bronze", "TFT-Iron", "TFT-Unranked" };
                var allrankcolors = new[] { new Color(240, 140, 15), new Color(253, 7, 7), new Color(192, 7, 146), new Color(32, 102, 148), new Color(46, 204, 113), new Color(241, 196, 15), new Color(151, 156, 159), new Color(187, 121, 68), new Color(255, 255, 255), new Color(124, 136, 120), new Color(188, 157, 154) };
                var TFTcolors = new[] { new Color(240, 140, 15), new Color(253, 7, 7), new Color(192, 7, 146), new Color(32, 102, 148), new Color(46, 204, 113), new Color(241, 196, 15), new Color(151, 156, 159), new Color(187, 121, 68), new Color(255, 255, 255), new Color(124, 136, 120) };

                string channelname = "Birdie-Connect";

                //my int for the color array (normal queue)
                int y = 0;

                //int for color array TFT
                int yy = 0;

                //running through all the different roles and create them
                //somehow when it checks for roles, it returns null everytime. But it only does it here?
                for (int x = 0; x < allRanks.GetLength(0); x++, y++)
                {
                    //checking if the role from allranks array can be found on the server
                    var role = Context.Guild.Roles.FirstOrDefault(z => z.Name.ToLower() == allRanks[x].ToLower());
                    if (role == null)
                    {
                        GuildPermissions permissions = default;
                        bool ishoisted = true;
                        RequestOptions options = null;
                        Console.WriteLine("Creating Role " + allRanks[x] + " On the server " + Context.Guild.Name + "!");
                        await Context.Guild.CreateRoleAsync(allRanks[x], permissions, allrankcolors[y], ishoisted, options);

                    }

                    //else if the rolecheck matches the current role then it must exist on server
                    else if (role != null)
                        Console.WriteLine(allRanks[x] + " Already exists on the the server " + Context.Guild.Name + "!");
                }

                for (int xx = 0; xx < TFTRanks.GetLength(0); xx++, yy++)
                {
                    var TFTRole = Context.Guild.Roles.FirstOrDefault(zz => zz.Name.ToLower() == TFTRanks[xx].ToLower());
                    if (TFTRole == null)
                    {
                        GuildPermissions permissions = default;
                        bool ishoisted = false;
                        RequestOptions options = null;
                        Console.WriteLine("Creating Role " + TFTRanks[xx] + " On the server " + Context.Guild.Name + "!");
                        await Context.Guild.CreateRoleAsync(TFTRanks[xx], permissions, allrankcolors[yy], ishoisted, options);
                    }
                    else if (TFTRole != null)
                        Console.WriteLine(TFTRanks[xx] + " Already exists on the the server " + Context.Guild.Name + "!");

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
                        Console.WriteLine(channelname + " Already exists on the the server " + Context.Guild.Name + "!");
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

                await Task.Delay(3000);
                await message.ModifyAsync(x =>
                {
                    x.Embed = new EmbedBuilder()
                    .AddField("All roles and channels has been created!",
                    "Feel free to move the channel as you please!")
                    .WithAuthor(author =>{author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);})
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(0, 255, 0))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>
                    {footer
                    .WithText(Global.Botcreatorname)
                    .WithIconUrl(Global.Birdieicon);})
                    .WithCurrentTimestamp()
                    .Build();});
                }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
