using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace NewTestBot.Modules
{
    public class purge : ModuleBase<SocketCommandContext>
    {
        [Command("purge", RunMode = RunMode.Async),RequireOwner]
        [Summary("Deletes the specified amount of messages.")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeChat(uint amount)
        {
            Console.WriteLine("Removing "+amount+" Messages!");
            var messages = await this.Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ReplyAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }
}
