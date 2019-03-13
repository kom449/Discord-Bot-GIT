using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using System;

namespace NewTestBot.Modules
{
    public class Drink: ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://cdn.discordapp.com/avatars/467437867065540620/083828453afa6811a853008993c51a45.png";

        [Command("drink")]
        public async Task Drinks(string drikkelse, int amount)
        {

            try
            {
                if (drikkelse == "")
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Syntax Error");
                    embed.WithDescription("No drink was specified");
                    embed.WithColor(new Color(255, 0, 0));
                    await Context.Channel.SendMessageAsync("", false, embed);
                    return;
                }

                if (amount == 0)
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Syntax Error");
                    embed.WithDescription("No amount was specified");
                    embed.WithColor(new Color(255, 0, 0));
                    await Context.Channel.SendMessageAsync("", false, embed);
                    return;
                }

                string userId = Context.User.Id.ToString();

                string data = File.ReadAllText("SystemLang/drinks.json");
                JObject o = JObject.Parse(data);

                //check if user exists in json
                bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

                //if not then add the user
                if (!userExists)
                {
                    o["Users"][userId] = o["UserTemplate"];
                }

                if (drikkelse == "kaffe")
                {
                    o["Coffee"] = (int)o["Coffee"] + amount;
                    o["Users"][userId]["Coffee"] = (int)o["Users"][userId]["Coffee"] + amount;
                    string serialziedJson = o.ToString();
                    File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                    string text = Context.User.Mention + " Har lige drukket noget kaffe";
                    text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Coffee"] + " kopper kaffe!";

                    var embed = new EmbedBuilder();
                    embed.AddField(Context.User.Username + " - the great kaffe bæller!",
                    text)
                    .WithColor(new Color(139, 69, 19))
                    .WithTitle("Kaffe Tracker :coffee: ")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }

                if (drikkelse == "the")
                {
                    o["Tea"] = (int)o["Tea"] + amount;
                    o["Users"][userId]["Tea"] = (int)o["Users"][userId]["Tea"] + amount;
                    string serialziedJson = o.ToString();
                    File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                    string text = Context.User.Mention + " har lige drukket noget the!";
                    text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Tea"] + " kopper the!";

                    var embed = new EmbedBuilder();
                    embed.AddField(Context.User.Username + " - the great the bæller!",
                    text)
                    .WithColor(new Color(139, 69, 19))
                    .WithTitle("the Tracker :tea: ")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }

                if (drikkelse == "sodavand")
                {
                    o["Soda"] = (int)o["Soda"] + amount;
                    o["Users"][userId]["Soda"] = (int)o["Users"][userId]["Soda"] + amount;
                    string serialziedJson = o.ToString();
                    File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                    string text = Context.User.Mention + " har lige drukket noget sodavand!";
                    text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Soda"] + " flasker sodavand!";

                    var embed = new EmbedBuilder();
                    embed.AddField(Context.User.Username + " - the great Soda bæller!",
                    text)
                    .WithColor(new Color(139, 69, 19))
                    .WithTitle("Soda Tracker :tropical_drink:")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }

                if (drikkelse == "vand")
                {
                    o["Water"] = (int)o["Water"] + amount;
                    o["Users"][userId]["Water"] = (int)o["Users"][userId]["Water"] + amount;
                    string serialziedJson = o.ToString();
                    File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                    string text = Context.User.Mention + " har lige drukket noget vand!";
                    text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Water"] + " kopper vand!";

                    var embed = new EmbedBuilder();
                    embed.AddField(Context.User.Username + " - the great vand bæller!",
                    text)
                    .WithColor(new Color(139, 69, 19))
                    .WithTitle("vand Tracker :potable_water:")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithText("Need help? Contact Birdie Zukira#3950")
                        .WithIconUrl(IconURL);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }

                if (drikkelse == "energidrik")
                {
                    o["EnergyDrink"] = (int)o["EnergyDrink"] + amount;
                    o["Users"][userId]["EnergyDrink"] = (int)o["Users"][userId]["EnergyDrink"] + amount;
                    string serialziedJson = o.ToString();
                    File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                    string text = Context.User.Mention + " har lige drukket noget energidrik!";
                    text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["EnergyDrink"] + " dåser energidrik!";

                    var embed = new EmbedBuilder();
                    embed.AddField(Context.User.Username + " - the great Monster bæller!",
                    text)
                    .WithColor(new Color(139, 69, 19))
                    .WithTitle("Energidrik Tracker :rocket:")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }

                if (drikkelse == "mtndew")
                {
                    o["Mtndew"] = (int)o["Mtndew"] + amount;
                    o["Users"][userId]["Mtndew"] = (int)o["Users"][userId]["Mtndew"] + amount;
                    string serialziedJson = o.ToString();
                    File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                    string text = Context.User.Mention + " har lige drukket noget Mountain Dew!";
                    text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Mtndew"] + " liter Mountain Dew!";

                    var embed = new EmbedBuilder();
                    embed.AddField(Context.User.Username + " - the great Dew bæller!",
                    text)
                    .WithColor(new Color(139, 69, 19))
                    .WithTitle("Mountain Dew tracker :tropical_drink:")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);
                    })
                    .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }

            }
            catch(ArgumentException e)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription(e.ToString());
                embed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, embed);
                return;
            }
        }
         
//---------------------------------------------------------------------------------------------------------

              [Command("drinktotal")]
        public async Task Kaffetotal(string drikkelse)
        {
                string userId = Context.User.Id.ToString();
                string data = File.ReadAllText("SystemLang/drinks.json");
                JObject o = JObject.Parse(data);

                //check if user exists in json
                bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

                //if not then add the user
                if (!userExists)
                {
                    o["Users"][userId] = o["UserTemplate"];
                }
        
            if (drikkelse == "kaffe")
            {

            o["Coffee"] = (int)o["Coffee"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange kopper kaffe er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Coffee"] + " kopper kaffe i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Coffee"] + " Kopper Kaffe!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper kaffe er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Kaffe Tracker :coffee:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
            }

            if (drikkelse == "the")
            {
             o["Tea"] = (int)o["Tea"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange kopper the er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Tea"] + " kopper the i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Tea"] + " Kopper the!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange Kopper the er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("the Tracker :tea:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
            }

            if (drikkelse == "sodavand")
            {
             o["Soda"] = (int)o["Soda"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange flasker sodavand er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Soda"] + " flasker sodavand i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Soda"] + " flasker sodavand!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange flasker sodavand er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Soda Tracker :tropical_drink:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
            }

            if (drikkelse == "vand")
            {
            o["Water"] = (int)o["Water"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange kopper vand er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Water"] + " kopper vand i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Water"] + " kopper vand!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange flasker sodavand er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("vand Tracker :potable_water:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
            }
            
            if (drikkelse == "energidrik")
            {
            o["EnergyDrink"] = (int)o["EnergyDrink"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange dåser energidrik er der blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["EnergyDrink"] + " dåser energidrik i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["EnergyDrink"] + " dåser energidrik!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange dåser energidrik er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Energidrik Tracker :rocket:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
            }

            if (drikkelse == "mtndew")
            {
            o["Mtndew"] = (int)o["Mtndew"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + "Vil gerne vide hvor mange liter Mountain Dew der er blevet drukket.";
            text += "\n " + Context.User.Mention + " har drukket " + o["Users"][userId]["Mtndew"] + " liter Mountain Dew i alt!";
            text += "\n\n til sammen er der blevet drukket " + o["Mtndew"] + " liter Mountain Dew!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange dåser energidrik er der blevet drukket?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Mountain Dew tracker :tropical_drink:")
            .WithCurrentTimestamp()
            .WithFooter(footer => { footer
            .WithText("Need help? Contact Birdie Zukira#3950")
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
            }

            else if (drikkelse == "")
            {
                //en liste med total for alle drikke
                //i'm lazy okay?
            }
        }
        
    }
}
