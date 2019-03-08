using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace NewTestBot.Modules
{
    class status : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
        readonly string thumbnailURL = "https://i.gyazo.com/f67d7843f1e9e918fb85816ab4a34181.png";
        DiscordSocketClient _client;

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
