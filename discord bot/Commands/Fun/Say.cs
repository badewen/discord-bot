using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Bot.Commands.Fun
{
    public class Say : ModuleBase<SocketCommandContext>
    {
        [RequireBotPermission(Discord.ChannelPermission.SendMessages)]
        [Command("say")]
        public Task SayAsync([Remainder] string message)
        {
            var arg = message;
            arg = arg.Trim();
            if (arg.Length == 0) return Task.CompletedTask;
            foreach (var mentionedMember in Context.Message.MentionedUsers)
            {
                arg = arg.Replace($"<@{mentionedMember.Id}>", $"@{mentionedMember.Username}");
            }
            foreach (var mentionedRole in Context.Message.MentionedRoles)
            {
                arg = arg.Replace($"<@&{mentionedRole.Id}>", $"@{mentionedRole.Name}");
            }
            if (Context.Message.MentionedEveryone) arg = arg.Replace("@everyone", "@‎everyone");
            ReplyAsync(arg.ToString());
            return Task.CompletedTask;
        }
        public Say()
        {
            CommandList.register(new CommandData(".say <message>", "make bot say what you want", "SayAsync", typeof(Say), Categories.Fun));
        }
    }
}