using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace NewTestBot.Modules
{
    public class randomteam : ModuleBase<SocketCommandContext>
    {
        [Command("randomteam", RunMode = RunMode.Async)]
        //[Command("randomteam"),RequireUserPermission(GuildPermission.Administrator)]
        //[RequireBotPermission(GuildPermission.Administrator)]
        public async Task RandomizeTeam(string response = null, params string[] args)
        {
            try
            {

                string userMessage = Context.Message.ToString();
                string name = userMessage.Substring(userMessage.IndexOf(' ') + 1);

                string[] users = name.Split(' ');
                foreach (string item in users)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }





    }
}
