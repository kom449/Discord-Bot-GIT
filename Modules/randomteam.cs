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
                //Context.Message.Tags.
                string name = Context.Message.ToString().Substring(Context.Message.ToString().IndexOf(' ') + 1);
                string[] users = name.Split(' ');
                foreach(string item in users)
                {

                    //var username = Context.User;
                    //var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "role name");
                    //await (username as IGuildUser).AddRoleAsync(role);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }





    }
}
