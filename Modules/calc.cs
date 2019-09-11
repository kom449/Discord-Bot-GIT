using System.Data;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;

namespace DingoBot.Modules
{
    public class Calc : ModuleBase<SocketCommandContext>
    {
        [Command("calc")]
        public async Task Calculator(string input)
        {
            if (input.Contains("42"))
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Discord Calculator!");
                embed.WithDescription("I do not know the meaning of life.");
                embed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, embed);
            }

            else
            {
                try
                {
                    var result = new DataTable().Compute(input, "") + "";


                    var embed = new EmbedBuilder();
                    embed.AddField("Made for easy math on discord",
                    "now with :cookie:")
                    .WithColor(new Color(0, 255, 0))
                    .WithTitle("Discord Calculator!")
                    .WithDescription("*" + result + "*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(Global.Birdieicon);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }
                catch(Exception)
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Discord Calculator!");
                    embed.WithDescription("invalid Operator was used!");
                    embed.WithColor(new Color(255, 0, 0));
                    await Context.Channel.SendMessageAsync("", false, embed);
                }

            }

        }
    }
}
