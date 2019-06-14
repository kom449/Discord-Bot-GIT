using Discord.WebSocket;
using Discord;
using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord.Commands;
using System.Net;



namespace NewTestBot
{

    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
            _client.Log += Log;
            _client.ReactionAdded += OnReactionAdded;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            await _client.SetGameAsync("Doing a fancy ^v^");
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);

        }

        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction) 
        {
            try
            {


                if (reaction.MessageId == Global.MessageidToTrack)
                {
                    if (reaction.Emote.Name == "👌" && reaction.UserId != _client.CurrentUser.Id)
                    {
                        
                        string data = File.ReadAllText("Resources/config.json");

                        //using c for webclient connections
                        WebClient c = new WebClient();

                        //getting DB information
                        JObject o = JObject.Parse(data);
                        string apikey = (string)o["lolapi"]["apikey"];
                        string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
                        (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);
                        string querytoken = "SELECT TOKEN FROM users_testing WHERE Discord_Id like  '%" + reaction.UserId + "%'; ";
                        string returnedtoken = null;
                        string queryid = "SELECT League_Id FROM users_testing WHERE Discord_Id like  '%" + reaction.UserId + "%'; ";
                        string returnedid = null;
                        Console.WriteLine(reaction.UserId);
                        

                        //sql connection for League Id
                        MySqlConnection myconn = new MySqlConnection(connect);
                        MySqlCommand GetId = new MySqlCommand(queryid, myconn);
                        MySqlCommand GetToken = new MySqlCommand(querytoken, myconn);

                        //getting League Id
                        myconn.Open();
                        string id = (string)GetId.ExecuteScalar();
                        string token = (string)GetToken.ExecuteScalar();
                        returnedid = id;
                        Console.WriteLine(returnedid);
                        returnedtoken = token;
                        myconn.Close();

                        //getting their token in the League Client
                        string Emptyreponse;
                        try
                        {
                            string ResponseToken = c.DownloadString("https://euw1.api.riotgames.com/lol/platform/v4/third-party-code/by-summoner/" + returnedid + "?api_key=" + apikey + "");
                            ResponseToken = ResponseToken.Trim('"');
                            Emptyreponse = ResponseToken;

                        }
                        catch (Exception ex)
                        {
                            string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
                            string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
                            var embed = new EmbedBuilder();
                            embed.AddField("Verifying your account...",
                            "No token was detected!")
                            .WithAuthor(author =>{ author
                            .WithName("Birdie Bot")
                            .WithIconUrl(IconURL);})
                            .WithThumbnailUrl(thumbnail)
                            .WithColor(new Color(255, 83, 13))
                            .WithTitle("Birdie Bot notification")
                            .WithFooter(footer =>{ footer
                            .WithText("Need help? Contact Birdie Zukira#3950")
                            .WithIconUrl(IconURL);})
                            .WithCurrentTimestamp()
                            .Build();

                            Console.WriteLine(ex);
                            await channel.SendMessageAsync("", false, embed);
                            await Task.Delay(2000);
                            var messages = await channel.GetMessagesAsync(3).Flatten();
                            await channel.DeleteMessagesAsync(messages);
                            return;
                        }

                        if (returnedtoken == Emptyreponse)
                        {
                            //add their account to DB
                            //getting league rank from ID
                            //using "r" for rank
                            string responserank = c.DownloadString("https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/" + returnedid + "?api_key=" + apikey + "");
                            JArray r = JArray.Parse(responserank);
                            string ranksolo = null;
                            string rankflex5 = null;
                            string rankflex3 = null;
                            string usedtiersolo = null;

                            //using a for loop to check all the bodies of the json
                            //since each queue type is in another body
                            for (int x = 0; x < r.Count; x++)
                            {
                                if (((string)r[x]["queueType"] == "RANKED_SOLO_5x5"))
                                {
                                    var tiersolo = (string)r[x]["tier"];
                                    var divisionsolo = (string)r[x]["rank"];
                                    string soloq = tiersolo + " " + divisionsolo;
                                    ranksolo = soloq;
                                    usedtiersolo = tiersolo.ToLower();
                                }
                            }

                            //using the same loop to get the Flex 5v5 rank
                            for (int y = 0; y < r.Count; y++)
                            {
                                if (((string)r[y]["queueType"] == "RANKED_FLEX_SR"))
                                {
                                    var tierflex5v5 = (string)r[y]["tier"];
                                    var divisionflex5v5 = (string)r[y]["rank"];
                                    string flex5v5 = tierflex5v5 + " " + divisionflex5v5;
                                    rankflex5 = flex5v5;
                                }

                            }

                            //again using the same loop to find the Flex 3v3 rank
                            for (int z = 0; z < r.Count; z++)
                            {
                                if (((string)r[z]["queueType"] == "RANKED_FLEX_TT"))
                                {
                                    var tierflex3v3 = (string)r[z]["tier"];
                                    var divisionflex3v3 = (string)r[z]["rank"];
                                    string flex3v3 = tierflex3v3 + " " + divisionflex3v3;
                                    rankflex3 = flex3v3;
                                }

                            }


                            string Query = "UPDATE users_testing SET SOLO_QUEUE = '" + ranksolo + "',FLEX_3V3 ='" + rankflex3 + "',FLEX_5V5 = '" + rankflex5 + "' WHERE Discord_Id like  '%" + reaction.UserId + "%';";
                            //sql connection and command
                            MySqlCommand postdata = new MySqlCommand(Query, myconn);
                            MySqlDataReader myreader;
                            myconn.Open();
                            myreader = postdata.ExecuteReader();
                            myconn.Close();

                            string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
                            string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
                            var embed = new EmbedBuilder();
                            embed.AddField("Verifying your account...",
                            "Your Account has been Verified!")
                            .WithAuthor(author =>{ author
                            .WithName("Birdie Bot")
                            .WithIconUrl(IconURL);})
                            .WithThumbnailUrl(thumbnail)
                            .WithColor(new Color(255, 83, 13))
                            .WithTitle("Birdie Bot notification")
                            .WithFooter(footer =>{ footer
                            .WithText("Need help? Contact Birdie Zukira#3950")
                            .WithIconUrl(IconURL);})
                            .WithCurrentTimestamp()
                            .Build();


                            await channel.SendMessageAsync("", false, embed);
                            Console.WriteLine(reaction.User +" Just verified their account!");
                            await Task.Delay(5000);
                            var messages = await channel.GetMessagesAsync(3).Flatten();
                            await channel.DeleteMessagesAsync(messages);
                        }
                        else
                        {
                            await channel.SendMessageAsync("Something went wrong - Contact Birdie Zukira#3950 ASAP");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private async Task Log(LogMessage msg)
        {
            await Task.Delay(100);
            Console.WriteLine(msg.Message);         
        }
    }
}