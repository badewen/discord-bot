using Bot.Attributes;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Commands.Misc
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        public static Dictionary<string, Embed> listofcommands = new();

        [Command("help")]
        [Usage(".help")]
        [Summary("display this message")]
        public Task HelpAsync(string a = "default")
        {
            a = a.ToLower();
            try
            {
                var zoe = listofcommands[a];
                ReplyAsync(embed: zoe);
            }
            catch
            {
                ReplyAsync(message: "Cant find taht command", messageReference: new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            }

            return Task.CompletedTask;
        }

        public static Task prepareHelpCommand()
        {
            var defaulthelp = new EmbedBuilder();
            defaulthelp
                .AddField("Fun", "Fun command")
                .AddField("Misc", "Misc command or command with no category")
                .AddField("Moderation", "Moderation command")
                .Color = Color.Red;
            listofcommands.Add("default", defaulthelp.Build());
            foreach (var zxc in CommandData.classes)
            {
                var alias = (AliasAttribute)Attribute.GetCustomAttribute(zxc.GetMethod(zxc.Name + "Async"), typeof(AliasAttribute));
                List<string> aliases = new();
                string stringaliases = "";
                var gagu = new EmbedBuilder();
                gagu.AddField("Command", zxc.Name)
                    .AddField("Description", ((SummaryAttribute)Attribute.GetCustomAttribute(zxc.GetMethod(zxc.Name + "Async"), typeof(SummaryAttribute))).Text)
                    .AddField("Usage", ((UsageAttribute)Attribute.GetCustomAttribute(zxc.GetMethod(zxc.Name + "Async"), typeof(UsageAttribute))).Text ?? "no description provided");
                if (alias == null) aliases.Add(" ");
                else
                {
                    foreach (var asa in alias.Aliases)
                    {
                        aliases.Add(asa);
                    }

                    foreach (var c in aliases)
                    {
                        stringaliases += " , ";
                        stringaliases += c;
                    }
                }
                gagu.AddField("Aliases", zxc.Name + stringaliases);
                gagu.Color = Color.Red;
                listofcommands.Add(zxc.Name.ToLower(), gagu.Build());
            }
            foreach (var zxcc in Enum.GetValues(typeof(Categories)))
            {
                var Commanddatacat = CommandData.Category[(Categories)zxcc];
                int conter = 0;
                string CommandCategoryString = "";
                foreach (var zbv in CommandData.Category[(Categories)zxcc])
                {
                    if (conter == 1)
                    {
                        CommandCategoryString += " , ";
                        conter--;
                    }
                    else conter++;
                    CommandCategoryString += zbv.Name;
                }
                if (CommandCategoryString == null || CommandCategoryString == "") CommandCategoryString = "None";
                var embed = new EmbedBuilder().AddField(zxcc.ToString(), CommandCategoryString);
                embed.Color = Color.Red;
                listofcommands.Add(zxcc.ToString().ToLower(), embed.Build());
            }

            return Task.CompletedTask;
        }

        public Help()
        {
            CommandData.register(typeof(Help), Categories.Misc);
        }
    }
}