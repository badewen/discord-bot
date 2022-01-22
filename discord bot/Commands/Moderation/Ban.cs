using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using Bot;
using System.Threading.Tasks;

namespace Bot.Commands.Moderation
{
    public class Ban : ModuleBase<SocketCommandContext>
    {
        [RequireBotPermission(Discord.GuildPermission.BanMembers)]
        [RequireUserPermission(Discord.GuildPermission.BanMembers)]
        [Command("ban")]
        public async Task BanAsync([Remainder] string arg)
        {
            if (arg.Length == 0)
            {
                await ReplyAsync("Invalid usage", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }

            var t = Task.Run(
                () =>
                {
                    Context.Guild.DownloadUsersAsync();
                    var author = Context.Guild.GetUser(Context.User.Id);
                    var bot = Context.Guild.GetUser(Program.client.CurrentUser.Id);
                    int notExists = 0;      //---|
                    int aboveAuthor = 0;    //   ----- failed counter
                    int aboveBot = 0;       //---|
                    string argCopy = arg;
                    List<string> ids = new(argCopy.Split(' '));
                    List<ulong> filteredIds = new();
                    // summary : add non NaN into filtered IDs
                    foreach (var id in ids)
                    {
                        if (!Utils.Isnan(id) && id.Length == 18) //mentioned user always nan because the raw string is '<@id>'
                        {
                            Console.WriteLine(id);
                            filteredIds.Add(Convert.ToUInt64(id));
                        }
                        else
                        {
                            Console.WriteLine($"not Valid : {id}");
                        }
                    }
                    // no need explanation
                    foreach (var mentioned in Context.Message.MentionedUsers)
                    {
                        filteredIds.Add(mentioned.Id);
                    }
                    // ban time
                    foreach (var id in filteredIds)
                    {
                        var target = Context.Guild.GetUser(id);
                        Console.WriteLine($"{Context.Guild.Name}, {Context.Guild.Id}");
                        if (target == null) { notExists++; continue; }
                        if (target.Hierarchy >= author.Hierarchy) { aboveAuthor++; continue; }
                        if (target.Hierarchy >= bot.Hierarchy) { aboveBot++; continue; }
                        Context.Guild.AddBanAsync(target);
                    }
                    ReplyAsync($"done banning member \n {aboveAuthor} cant ban user because above you\n {aboveBot} cant ban user because above me\n {notExists} users doesn't exist", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                }
            );
            return;
        }

        public Ban()
        {
            CommandList.register(new CommandData(
                ".ban <id / mention>", 
                "ban people", 
                "BanAsync", 
                typeof(Ban), 
                Categories.Moderation));            
        }
    }
}