﻿using System;
using System.Collections;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;
using Discord.Rest;

namespace NewTestBot.Modules
{
    public class Verify : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        string Token = null;
        [Command("verify", RunMode = RunMode.Async)]
        public async Task Verifyaccounts()
        {
            

            string data = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(data);
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);

            string UserID = Context.User.Id.ToString();

            string Get_Token = "SELECT TOKEN FROM users_testing WHERE Discord_Id like  '%" + UserID + "%'; ";
            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand Get_Token_Command = new MySqlCommand(Get_Token, myconn);
            MySqlDataReader myreader;

            myconn.Open();
            myreader = Get_Token_Command.ExecuteReader();
            while (myreader.Read())
            {
                Token = myreader.GetString(0);
            }
            myconn.Close();
            Console.WriteLine(Token);
            //Breaking if token/DBData already exists
            if (Token != null)
            {
                    var embed = new EmbedBuilder();
                    embed.AddField("Verification of your account...",
                    "To verify your League of Legends account \nPlease take this Token: "+Token+"\n Use it in the league client under the settings page")
                    .WithAuthor(author =>
                    {author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>
                    {footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    //var emoji = new Emoji(":ok_hand:");
                    //await Context.Message.AddReactionAsync(emoji);
                    RestUserMessage msg = await Context.Channel.SendMessageAsync("",false,embed);
                    Global.MessageidToTrack = msg.Id;
                    return;
            }
            else
            {

                //string of chars to use in token generation
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                //array of chars with given length [length of token]
                var stringChars = new char[8];
                Random random = new Random();
                //for-loop, running for .length of the char[], adding a new char every time, picked from the string of chars.
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                //finally adding the array of chars to the final string token
                Token = new String(stringChars);

                string Send_Token = "UPDATE users_testing SET TOKEN = '" + Token + "' WHERE Discord_Id like '%" + UserID + "%';";

                MySqlCommand Send_Token_Command = new MySqlCommand(Send_Token, myconn);

                myconn.Open();
                myreader = Send_Token_Command.ExecuteReader();
                myconn.Close();

                    var embed = new EmbedBuilder();
                    embed.AddField("Verification of your account...",
                    "To verify your League of Legends account \nPlease take this Token: "+"***"+""+Token+""+"***"+"\n Use it in the league client under the settings page")
                    .WithAuthor(author =>
                    {author
                    .WithName("Birdie Bot")
                    .WithIconUrl(IconURL);})
                    .WithThumbnailUrl(thumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot notification")
                    .WithFooter(footer =>
                    {footer
                    .WithText("Need help? Contact Birdie Zukira#3950")
                    .WithIconUrl(IconURL);})
                    .WithCurrentTimestamp()
                    .Build();
                    //var emoji = new Emoji(":ok_hand:");
                    //await Context.Message.AddReactionAsync(emoji);
                    RestUserMessage msg = await Context.Channel.SendMessageAsync("", false, embed);
                    Global.MessageidToTrack = msg.Id;
            }
            

                  


        }
    }
}
