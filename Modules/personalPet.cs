using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.IO;
using Discord.Rest;

/*
First i need to get the user ID from DB and see what things the user currently have.
If the user doesnt have a pet, ask them if they want to create one.
Create pet Parameters.
If the user already have a pet, tell the user that they already have one.
Create new commands - Status on pet , Feed pet , Interact with pet , ETC...
Maybe a Disown command to remove your pet.

    Bot Diagram
    https://drive.google.com/open?id=1F6ueYeS_tV_-LCxxFai1JRIMPLuii_xz

All the interactions has to be done with Reactions, which is done in the Main program
so that i dont create duplicate client instances (discord hate when bot programmers make several instances).
All stuff is stored in a remote DB so all data is kept safe and accessible for me.
*/

namespace DingoBot.Modules
{
    public class personalPet : ModuleBase<SocketCommandContext>
    {
        [Command("pet")]
        public async Task Mypet()
        {
            Global.Command = "pet";
            string Discord_ID = Context.User.Id.ToString();
            string GetId = "SELECT PET FROM PETDB WHERE Discord_Id like  '%" + Discord_ID + "%'; ";
            string Duplicate = "SELECT Discord_Id FROM PETDB WHERE Discord_Id like  '%" + Discord_ID + "%'; ";
            string Result;

            MySqlConnection myconn = new MySqlConnection(Global.connect);
            MySqlCommand DuplicateCommand = new MySqlCommand(Duplicate, myconn);

            MySqlDataReader myreader;
            myconn.Open();
            Result = (string)DuplicateCommand.ExecuteScalar();
            myreader = DuplicateCommand.ExecuteReader();
            if (myreader.Read())
                Result = myreader.GetString(myreader.GetOrdinal("Discord_Id"));      
            myconn.Close();

            if(Result == Discord_ID)
            {
                //user already have pet
                
            }
            else
            {
                //user doesnt have pet - Ask if they want to create one with reactions
                var embed = new EmbedBuilder();
                embed.AddField("No pet was found...",
                "Do you want to create one?")
                .WithAuthor(author =>{
                author.WithName("Birdie Bot")
                .WithIconUrl(Global.Birdieicon);})
                .WithThumbnailUrl(Global.Birdiethumbnail)
                .WithColor(new Color(255, 83, 13))
                .WithTitle("Birdie Bot notification")
                .WithFooter(footer =>{ footer
                .WithText("Need help? Contact Birdie Zukira#3950")
                .WithIconUrl(Global.Birdieicon);})
                .WithCurrentTimestamp()
                .Build();

                var emoji = new[] { "👌", "👌" };
                RestUserMessage msg = await Context.Channel.SendMessageAsync("", false, embed);
                for (int x = 0; x < emoji.GetLength(0);x++)
                {
                    var emote = new Emoji(emoji[x]);
                    await msg.AddReactionAsync(emote);
                }
                Global.MessageidToTrack = msg.Id;
                Global.message = msg;
  
            }


            await Context.Channel.SendMessageAsync("");
        }
    }
}
