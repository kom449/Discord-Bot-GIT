using System;
using System.Collections;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net;

namespace NewTestBot.Modules
{
    public class Verify : ModuleBase<SocketCommandContext>
    {
        readonly string IconURL = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        readonly string thumbnail = "https://i.gyazo.com/e05bec8ae83bbd60f5ff55f48c3c30f1.png";
        string Token = "";
        [Command("verify", RunMode = RunMode.Async)]

        /*
            Generate a password

            Requirements for password:
            random 8 chars
            letters and numbers
            upper and lower case letters
        */
        public async Task Verifyaccounts()
        {
            try
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            string data = File.ReadAllText("Resources/config.json");
            JObject o = JObject.Parse(data);
            string connect = string.Format("server={0};user={1};database={2};port={3};password={4}",
            (string)o["database"]["dbhost"], (string)o["database"]["dbuser"], (string)o["database"]["dbname"], (string)o["database"]["dbport"], (string)o["database"]["dbpass"]);


            string UserID = Context.User.Id.ToString();
            string Query = "UPDATE users_testing SET TOKEN = '" + Token + "' WHERE Discord_Id like '%" + UserID + "%';";


            MySqlConnection myconn = new MySqlConnection(connect);
            MySqlCommand command = new MySqlCommand(Query, myconn);
            MySqlDataReader myreader;

            myconn.Open();
            myreader = command.ExecuteReader();
            myconn.Close();


        }
    }
}
