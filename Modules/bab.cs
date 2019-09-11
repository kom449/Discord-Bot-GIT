using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace DingoBot.Modules
{
    public class bab : ModuleBase<SocketCommandContext>
    {
        [Command("bab"),RequireOwner]
        public async Task Bab(string message = "")
        {


            if (message == "")
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("I NEED A FUCKING TARGET YOU RETARD");
                embed.WithColor(new Color(255, 0, 0));

                await Context.Channel.SendMessageAsync("", false, embed);
            }

            else if (message != "")
            {
                var embed = new EmbedBuilder();
                embed.AddField("Your options are:",
                "Go commit rope :thinking: \n BAB back :fist: \n Go cry in a corner :cry: ")
                .WithColor(new Color(253, 246, 56))
                .WithTitle("BABBER-BOT9000")
                .WithDescription(message + " Has been bab'd by " + Context.User.Mention)
                .WithCurrentTimestamp()
                .WithFooter(footer => {
                    footer
                .WithText("Need help? Contact Birdie Zukira#3950")
                .WithIconUrl(Global.Birdieicon);
                })
                .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }
    }
}