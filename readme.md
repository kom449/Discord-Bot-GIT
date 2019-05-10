[![Codacy Badge](https://api.codacy.com/project/badge/Grade/77b4cd256aab40d59c107a6daff1314c)](https://www.codacy.com/app/kom449/Discord-Bot-GIT?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=kom449/Discord-Bot-GIT&amp;utm_campaign=Badge_Grade)
# Commands
| Command | Description | Usage |
| ------ | ------ | ------ |
|Bab|Bab a user|{prefix}bab {@name}|
|Kick|Kicks a user from the server|{prefix}kick {@name} {reason}|
|Ban|Bans a user from the server|{prefix}ban {@name} {reason}|
|help|shows a list of all the commands|{prefix}help|
|drink|drinks a beverage|{prefix}drink {beverage} {amount}|
|drinktotal|Total amount of a beverage|{prefix}drinktotal {beverage}|
|drinkleader|shows a leaderboard of the beverage|{prefix}drinkleader {beverage}|
|drinklist|Shows the list of beverages|{prefix}drinklist|
|Birb|gives you a random bird picture|{prefix}birb|
|Prefix|changes the current prefix|{prefix}prefix {new prefix}|
|purge|deletes x amount of messages in a channel|{prefix}purge {amount}|
|status|changes the game status of the bot|{prefix}status {text}|
|connect|connect/link discord and lol|{prefix}connect {account name}|
|disconnect|remove account from DB|{prefix}disconnect|
|update|update rank in DB/discord|{prefix}update|
|create|Creates ranks from unranked to Challenger|{prefix}create|

# To do list
 - Automatic assign roles to users that have been kicked and reinvited
 - Add token support
 - create global update command
 - support multiple accounts
 - Room creation, so it creates a room for it
 - make it check if the roles already exists to avoid making duplicates
 - add rank command so a user can get the ranks of a player
 
# Known Issues
 - there is a slight chance that the bot will not send a message when connecting 
 
 
# Low priority
 - make embeds pretty
 - create global variables for the embeds, so it's easier to make them in the future
