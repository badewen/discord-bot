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
        public Task HelpAsync(string arg = "default")
        {
            // very proud of this code very "clean"
            arg = arg.ToLower();
            try
            {
                var command = listofcommands[arg];
                ReplyAsync(embed: command);
            }
            catch
            {
                ReplyAsync(message: "Cant find that command", messageReference: new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            }

            return Task.CompletedTask;
        }

        public static Task prepareHelpCommand()
        {   //going to rarely touch this spaghethet code
            {
                var commandCategory = new EmbedBuilder();
                commandCategory
                    .AddField("Fun", "Fun command")
                    .AddField("Misc", "Misc command or command with no category")
                    .AddField("Moderation", "Moderation command")
                    .AddField("Debug", "Debug")
                    .Color = Color.Red;
                listofcommands.Add("default", commandCategory.Build());// defualt peag cnotains categoirs
            }
            foreach (var command in CommandData.classes)
            {
                // get all the alias
                Console.WriteLine(command.Name);
                var aliases = (AliasAttribute)Attribute.GetCustomAttribute(command.GetMethod(command.Name + "Async"), typeof(AliasAttribute)); 
                // representable list of aliases
                string stringaliases = "";
                var commandEmbed = new EmbedBuilder();
                commandEmbed.AddField("Command", command.Name)
                             .AddField("Description", ((SummaryAttribute)Attribute.GetCustomAttribute(command.GetMethod(command.Name + "Async"), typeof(SummaryAttribute))).Text)
                             .AddField("Usage", ((UsageAttribute)Attribute.GetCustomAttribute(command.GetMethod(command.Name + "Async"), typeof(UsageAttribute))).Text ?? "not provided");
                if (aliases == null) { }
                else
                {
                    // appending or adding aliases to stringaliases
                    foreach (var alias in aliases.Aliases)
                    {
                        stringaliases += " , ";
                        stringaliases += alias;
                    }
                }
                commandEmbed.AddField("Aliases", command.Name + stringaliases);
                commandEmbed.Color = Color.Red;
                listofcommands.Add(command.Name.ToLower(), commandEmbed.Build());
            }
            // summary : .help <Category>
            // get and loop through all categories
            foreach (var enumCategory in Enum.GetValues(typeof(Categories)))
            {
                // get category from commanddata (ran out of names)
                // return list of command class type
                var commands = CommandData.Category[(Categories)enumCategory];
                int conter = 0;
                string commandCategoryString = "";
                // get and append command associated with the category
                foreach (var command in commands)
                {
                    conter++;
                    if (conter%2==0)
                    {
                        commandCategoryString += " , ";
                        conter = 0;
                    }
                    commandCategoryString += command.Name;
                }
                // if there are no commands associated with the category
                if (commandCategoryString == null || commandCategoryString == "") commandCategoryString = "None";
                var embed = new EmbedBuilder().AddField(enumCategory.ToString(), commandCategoryString);
                embed.Color = Color.Red;
                // get enumCategory name and adding embed
                listofcommands.Add(enumCategory.ToString().ToLower(), embed.Build());
            }

            return Task.CompletedTask;
        }

        public Help()
        {
            CommandData.register(typeof(Help), Categories.Misc);
        }
    }
}