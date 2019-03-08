# Commands
| Command | Description | Usage |
| ------ | ------ | ------ |
|Bab|Bab a user|{prefix}bab {@name}|
|Kick|Kicks a user from the server|{prefix}kick {@name} {reason}|
|Ban|Bans a user from the server|{prefix}ban {@name} {reason}|
|Kaffe|Drink a cup of coffee|{prefix}kaffe|
|Kaffetotal|Shows the total amount of coffee|{prefix}kaffetotal|
|Birb|gives you a random bird picture|{prefix}birb|
|Prefix|changes the current prefix|{prefix}prefix {new prefix}|

# To do list
 - Add commands to individual drinks
 - Automatic assign roles to users that have been kicked and reinvited
 - Optimize commands and put each command into individual files 
 - Add MYSQL support and store user information remotely
 
# Known Issues
 - Users can DM the bot with the commands, which should not be possible
 - There is currently no way to get specific roles of users
 - Using more than 1 word in a reason will result in a "Using too many parameters" will be fixed soon
 
# Low priority
 - make embeds pretty
 - create global variables for the embeds, so it's easier to make them in the future
