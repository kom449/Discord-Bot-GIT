using System.Data;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;

namespace NewTestBot.Modules
{
    public class Calc : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

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
                    .WithText(Global.Botcreatorname)
                    .WithIconUrl(IconURL);
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
