using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;

namespace NewTestBot
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;
        

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {          
            var msg = s as SocketUserMessage;
            if (msg == null)
            {
                return;
            }
            
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                if (context.IsPrivate == true)
                {
                    string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Birdie Bot nortification");
                    embed.WithDescription("I do not accept commands from Direct messages!");
                    embed.WithFooter(footer => { footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    });
                    embed.WithCurrentTimestamp();
                    embed.WithColor(new Color(255, 0, 0));
                    await context.User.SendMessageAsync("", false, embed);                 
                }
                else
                {
                    var result = await _service.ExecuteAsync(context, argPos);
                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }
            }
        }
    }
}
