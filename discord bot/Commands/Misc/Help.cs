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
            foreach (var command in CommandList.Commands)
            {
                // get all the alias
                Console.WriteLine(command.CommandClass.Name);
                var aliases = (AliasAttribute)Attribute.GetCustomAttribute(command.CommandClass.GetMethod(command.CommandMethodName), typeof(AliasAttribute)); 
                // representable list of aliases
                string stringaliases = "";
                var commandEmbed = new EmbedBuilder();
                commandEmbed.AddField("Command", command.CommandClass.Name)
                             .AddField("Description", command.Description)
                             .AddField("Usage", command.Usage);
                if (aliases != null)
                {
                    // appending or adding aliases to stringaliases
                    foreach (var alias in aliases.Aliases)
                    {
                        stringaliases += " , ";
                        stringaliases += alias;
                    }
                }
                commandEmbed.AddField("Aliases", command.CommandClass.Name + stringaliases);
                commandEmbed.Color = Color.Red;
                listofcommands.Add(command.CommandClass.Name.ToLower(), commandEmbed.Build());
            }
            // summary : .help <Category>
            // get and loop through all categories
            foreach (var enumCategory in Enum.GetValues(typeof(Categories)))
            {
                // get commands associated with the category from commanddata (ran out of names)
                // return list of command class type
                var commands = CommandList.CategoryCommands[(Categories)enumCategory];
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
                    commandCategoryString += command.CommandClass.Name;
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
            CommandList.register(new CommandData(".help <category / commands>", "display this message", "HelpAsync", typeof(Help), Categories.Misc));
        }
    }
}