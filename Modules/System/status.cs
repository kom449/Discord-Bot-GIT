﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MySql.Data.MySqlClient;


namespace NewTestBot.Modules
{
    public class Status : ModuleBase<SocketCommandContext>
    {
        [Command("status"),RequireUserPermission(GuildPermission.Administrator),RequireOwner]
        public async Task Changestatus(string input){
            DiscordSocketClient _client = Context.Client;

            if (input == null){
                var embed = new EmbedBuilder();
                embed.WithTitle("Syntax Error");
                embed.WithDescription("No game status written");
                embed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, embed);
            }

            if(input == "count"){
                try{
                    await _client.SetGameAsync("with " + Returncount() + " Players ^v^");
                    Console.WriteLine("Game changed!");
                    await Task.CompletedTask;
                }
                catch (Exception ex){
                    Console.WriteLine(ex);
                }
            }

            else if (input != null){
                try{
                    await _client.SetGameAsync(input);
                    await Task.CompletedTask;
                    var embed = new EmbedBuilder();
                    embed.AddField("Game status has been changed!",
                    "game status has been changed to: "+"**"+input+"**")
                    .WithAuthor(author => { author
                    .WithName("Birdie Bot")
                    .WithIconUrl(Global.Birdieicon);
                    })
                    .WithThumbnailUrl(Global.Birdiethumbnail)
                    .WithColor(new Color(255, 83, 13))
                    .WithTitle("Birdie Bot nortification")
                    .WithFooter(footer => { footer
                    .WithText(Global.Botcreatorname)
                    .WithIconUrl(Global.Birdieicon);
                    })
                    .WithCurrentTimestamp()
                    .Build();
                     await Context.Channel.SendMessageAsync("", false, embed);
                }
                catch(Exception ex){
                    Console.WriteLine(ex);
                }
            }
        }

        string Returncount(){
            MySqlConnection myconn = new MySqlConnection(Global.connect);
            string sqlquery = "SELECT COUNT(*) FROM users_testing";
            string returnedcount = null;
            MySqlCommand GetCount = new MySqlCommand(sqlquery, myconn);
            myconn.Open();
            Int64 result = (Int64)GetCount.ExecuteScalar();
            returnedcount = result.ToString();
            myconn.Close();
            return returnedcount;
        }
    }
}
