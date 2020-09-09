using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NewTestBot.Modules
{
    public class Drink : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";

        [Command("drink"),RequireOwner]
        public async Task Drinks(string drikkelse, float amount)
        {
            // change first letter to uppercase, and the rest of the letters to lower case.
            drikkelse = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(drikkelse.ToLower());

            try
            {
                if (drikkelse == "")
                {
                    var errorEmbed = new EmbedBuilder();
                    errorEmbed.WithTitle("Syntax Error");
                    errorEmbed.WithDescription("No drink was specified");
                    errorEmbed.WithColor(new Color(255, 0, 0));
                    await Context.Channel.SendMessageAsync("", false, errorEmbed);
                    return;
                }

                if (amount <= 0)
                {
                    var errorEmbed = new EmbedBuilder();
                    errorEmbed.WithTitle("Syntax Error");
                    errorEmbed.WithDescription("No amount was specified");
                    errorEmbed.WithColor(new Color(255, 0, 0));
                    await Context.Channel.SendMessageAsync("", false, errorEmbed);
                    return;
                }

                string userId = Context.User.Id.ToString();

                //update user info
                UpdateUserInfo(Context.User.Id.ToString(), Context.User);

                //read json drinks file
                string data = File.ReadAllText("SystemLang/drinks.json");
                JObject o = JObject.Parse(data);

                if (!((JObject)o["DrinkBinds"]).ContainsKey(drikkelse))
                {
                    var errorEmbed = new EmbedBuilder();
                    errorEmbed.WithTitle("Fejlkode 42");
                    errorEmbed.WithDescription("Drikken findes ikke!");
                    errorEmbed.WithColor(new Color(255, 0, 0));
                    await Context.Channel.SendMessageAsync("", false, errorEmbed);
                    return;
                }

                string drink = (string)o["DrinkBinds"][drikkelse];

                //add drink to user if it doesn't exist
                if (!((JObject)o["Users"][userId]["Drinks"]).ContainsKey(drink))
                {
                    o["Users"][userId]["Drinks"][drink] = 0;
                }

                //increment drinks
                o["Drinks"][drink]["Total"] = (int)o["Drinks"][drink]["Total"] + amount;
                o["Users"][userId]["Drinks"][drink] = (int)o["Users"][userId]["Drinks"][drink] + amount;

                //save json file
                string serialziedJson = o.ToString();
                File.WriteAllText("SystemLang/drinks.json", serialziedJson);

                string drinkName = (string)o["Drinks"][drink]["Name"];
                string drinkNameLower = drinkName.ToLower();
                string drinkEmoji = (string)o["Drinks"][drink]["Emoji"];
                string drinkUnits = (string)o["Drinks"][drink]["Units"];

                string text = Context.User.Mention + " har lige drukket noget " + drinkNameLower + "!";
                text += "\nSå " + Context.User.Username + " har nu drukket " + o["Users"][userId]["Drinks"][drink] + " " + drinkUnits + " " + drinkNameLower + "!";

                var embed = new EmbedBuilder();
                embed.AddField(Context.User.Username + " - the great " + drinkNameLower + " bæller!",
                text)
                .WithColor(new Color(139, 69, 19))
                .WithTitle(drinkName + " Tracker " + drinkEmoji)
                .WithCurrentTimestamp()
                .WithFooter(footer => {
                    footer
                    .WithText(Global.Botcreatorname)
                    .WithIconUrl(IconURL);
                })
                .Build();

                await Context.Channel.SendMessageAsync("", false, embed);

            }
            catch (ArgumentException e)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription(e.ToString());
                embed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, embed);
                return;
            }
        }

        [Command("drinklist"),RequireOwner]
        public async Task DrinkList()
        {
            //update user info
            UpdateUserInfo(Context.User.Id.ToString(), Context.User);

            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/drinks.json");
            JObject o = JObject.Parse(fileText);

            JObject drinks = (JObject)o["Drinks"];

            string text = "";

            foreach (var drinkPair in drinks)
            {
                JObject drink = (JObject)drinkPair.Value;
                text += "**" + (string)drink["Name"] + "**\n";

                foreach (var drinkAlias in (JObject)o["DrinkBinds"])
                {
                    if (drinkAlias.Value.ToString() == drinkPair.Key)
                    {
                        text += " - " + drinkAlias.Key + "\n";
                    }
                }
            }

            var embed = new EmbedBuilder();
            embed.AddField("Her er en liste af drinks, og måder at skrive dem på.",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle("Drink List")
            .WithCurrentTimestamp()
            .WithFooter(footer => {
                footer
            .WithText(Global.Botcreatorname)
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("drinkleader"),RequireOwner]
        public async Task DrinkLeaderboard(string drikkelse)
        {
            // change first letter to uppercase, and the rest of the letters to lower case.
            drikkelse = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(drikkelse.ToLower());

            if (drikkelse == "")
            {
                var errorEmbed = new EmbedBuilder();
                errorEmbed.WithTitle("Syntax Error");
                errorEmbed.WithDescription("No drink was specified");
                errorEmbed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, errorEmbed);
                return;
            }

            //update user info
            UpdateUserInfo(Context.User.Id.ToString(), Context.User);

            //read json drinks file
            string data = File.ReadAllText("SystemLang/drinks.json");
            JObject o = JObject.Parse(data);

            //throw error if drink doesn't exist
            if (!((JObject)o["DrinkBinds"]).ContainsKey(drikkelse))
            {
                var errorEmbed = new EmbedBuilder();
                errorEmbed.WithTitle("Fejlkode 42");
                errorEmbed.WithDescription("Drikken findes ikke!");
                errorEmbed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, errorEmbed);
                return;
            }

            string drink = (string)o["DrinkBinds"][drikkelse];
            string text = "";

            JObject userObject = (JObject)o["Users"];
            List<JObject> users = new List<JObject>();

            foreach (var user in userObject)
            {
                if (((JObject)user.Value["Drinks"]).ContainsKey(drink))
                    users.Add((JObject)user.Value);
            }

            users.Sort(
                delegate (JObject pair1,
                JObject pair2)
                {
                    return ((int)pair2["Drinks"][drink]).CompareTo((int)pair1["Drinks"][drink]);
                }
            );

            string drinkName = (string)o["Drinks"][drink]["Name"];
            string drinkNameLower = drinkName.ToLower();
            string drinkEmoji = (string)o["Drinks"][drink]["Emoji"];
            string drinkUnits = (string)o["Drinks"][drink]["Units"];

            for (int i = 0; i < Math.Min(10, users.Count); i++)
            {
                JObject user = users[i];

                text += String.Format("{0}: {1} har drukket {2} {3} {4}!", i + 1, user["Mention"], user["Drinks"][drink], drinkUnits, drinkNameLower) + "!\n";
            }

            var embed = new EmbedBuilder();
            embed.AddField("Hvem har drukket mest " + drinkNameLower + "?",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle(drinkName + " Leaderboard " + drinkEmoji)
            .WithCurrentTimestamp()
            .WithFooter(footer => {
                footer
            .WithText(Global.Botcreatorname)
            .WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("drinktotal"),RequireOwner]
        public async Task DrinkTotal(string drikkelse)
        {
            if (drikkelse == "")
            {
                var errorEmbed = new EmbedBuilder();
                errorEmbed.WithTitle("Syntax Error");
                errorEmbed.WithDescription("No drink was specified");
                errorEmbed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, errorEmbed);
                return;
            }

            // change first letter to uppercase, and the rest of the letters to lower case.
            drikkelse = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(drikkelse.ToLower());

            //get unique user id
            string userId = Context.User.Id.ToString();

            //update user info
            UpdateUserInfo(Context.User.Id.ToString(), Context.User);

            //creating a string called filetext with everything from the json file
            string fileText = File.ReadAllText("SystemLang/drinks.json");
            JObject o = JObject.Parse(fileText);

            //throw error if drink doesn't exist
            if (!((JObject)o["DrinkBinds"]).ContainsKey(drikkelse))
            {
                var errorEmbed = new EmbedBuilder();
                errorEmbed.WithTitle("Fejlkode 42");
                errorEmbed.WithDescription("Drikken findes ikke!");
                errorEmbed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, errorEmbed);
                return;
            }

            //get drink json name
            string drink = (string)o["DrinkBinds"][drikkelse];

            //drink information
            string drinkName = (string)o["Drinks"][drink]["Name"];
            string drinkNameLower = drinkName.ToLower();
            string drinkEmoji = (string)o["Drinks"][drink]["Emoji"];
            string drinkUnits = (string)o["Drinks"][drink]["Units"];

            //initialize user amount variable
            int userAmount = 0;

            //get user amount if defined
            if (((JObject)o["Users"][userId]["Drinks"]).ContainsKey(drink))
                userAmount = (int)o["Users"][userId]["Drinks"][drink];

            //get total amount
            int totalAmount = (int)o["Drinks"][drink]["Total"];

            //turning the result and some text into a string for the embed builder
            string text = Context.User.Mention + " vil gerne vide hvor mange " + drinkUnits + " " + drinkNameLower + " der er blevet drukket.";
            text += "\n " + Context.User.Username + " har drukket " + userAmount + " " + drinkUnits + " " + drinkNameLower + " i alt!";
            text += "\n\n I alt er der blevet drukket " + totalAmount + " " + drinkUnits + " " + drinkNameLower + "!";

            var embed = new EmbedBuilder();
            embed.AddField("Hvor mange " + drinkUnits + " " + drinkNameLower + " er der blevet drukket? ",
            text)
            .WithColor(new Color(139, 69, 19))
            .WithTitle(drinkName + " Tracker " + drinkEmoji)
            .WithCurrentTimestamp()
            .WithFooter(footer => {
                footer
.WithText(Global.Botcreatorname)
.WithIconUrl(IconURL);
            })
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        public static void UpdateUserInfo(string userId, IUser user)
        {
            string data = File.ReadAllText("SystemLang/drinks.json");
            JObject o = JObject.Parse(data);

            //check if user exists in json
            bool userExists = ((JObject)o["Users"]).ContainsKey(userId);

            //if not then add the user
            if (!userExists)
            {
                o["Users"][userId] = o["UserTemplate"];
            }

            // add or update username
            if (!((JObject)o["Users"][userId]).ContainsKey("Username") || (string)o["Users"][userId]["Username"] != user.Username)
            {
                o["Users"][userId]["Username"] = user.Username;
            }

            // add or update mention
            if (!((JObject)o["Users"][userId]).ContainsKey("Mention") || (string)o["Users"][userId]["Mention"] != user.Mention)
            {
                o["Users"][userId]["Mention"] = user.Mention;
            }

            string serialziedJson = o.ToString();
            File.WriteAllText("SystemLang/drinks.json", serialziedJson);
        }

    }
}