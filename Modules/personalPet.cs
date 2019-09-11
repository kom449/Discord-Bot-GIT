using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

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
            
           
            string Discord_ID = Context.User.Id.ToString();
            string GetId = "SELECT PET FROM PETDB WHERE Discord_Id like  '%" + Discord_ID + "%'; ";


            await Context.Channel.SendMessageAsync("");
        }
    }
}
