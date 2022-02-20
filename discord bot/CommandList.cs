using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Bot.Attributes;
using Discord.Commands;

namespace Bot
{
    public class CommandList
    {
        internal static Task PrepareCategories()
        {
            CategoryCommands.Add(Category.Fun, FunCommands);
            CategoryCommands.Add(Category.Misc, MiscCommands);
            CategoryCommands.Add(Category.Moderation, ModerationCommands);
            CategoryCommands.Add(Category.Debug, DebugCommands);
            return Task.CompletedTask;
        }

        internal static Task RegisterAllCommands()
        {
            Type[] modules = Assembly.GetEntryAssembly().GetTypes();
            List<Module> commandModules = new();
            foreach(Type module in modules)
            {
                MethodInfo[] methods = module.GetMethods();
                foreach(MethodInfo method in methods)
                {
                    if (method.GetCustomAttribute<CommandAttribute>() == null)
                    {
                        continue;
                    }

                    UsageAttribute usageResult = method.GetCustomAttribute<UsageAttribute>();
                    DescriptionAttribute descriptionResult = method.GetCustomAttribute<DescriptionAttribute>();
                    CategoryAttribute categoryResult = method.GetCustomAttribute<CategoryAttribute>();

                    string usage ;
                    string description;
                    Category category ;

                    if (usageResult == null)
                    {
                        usage = "No Information Is Provided";
                    }
                    else
                    {
                        usage = usageResult.Text;
                    }
                    if (descriptionResult == null)
                    {
                        description = "No Description Is Provided";
                    }
                    else
                    {
                        description = descriptionResult.Text;
                    }
                    if (categoryResult == null)
                    {
                        category = Category.Misc;
                    }
                    else
                    {
                        category = categoryResult.category;
                    }
                    CommandData data = new CommandData(usage, description, method.Name, module, category);
                    switch (category)
                    {
                        case Category.Misc:
                            MiscCommands.Add(data);
                            break;
                        case Category.Debug:
                            DebugCommands.Add(data);
                            break;
                        case Category.Fun:
                            FunCommands.Add(data);
                            break;
                        case Category.Moderation:
                            ModerationCommands.Add(data);
                            break;
                    }

                    Commands.Add(data);
                }
            }
            return Task.CompletedTask;
        }

        internal static Task BuildCommandsEmbed()
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
            foreach (var command in Commands)
            {
                // get all the alias
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
                        CommandsDic.Add(alias.ToLower(), command);
                    }
                }
                commandEmbed.AddField("Aliases", command.CommandClass.Name + stringaliases);
                commandEmbed.Color = Color.Red;
                listofcommands.Add(command.CommandClass.Name.ToLower(), commandEmbed.Build());
                CommandsDic.Add(command.CommandClass.Name.ToLower(), command);

            }
            // summary : .help <Category>
            // get and loop through all categories
            foreach (var enumCategory in Enum.GetValues(typeof(Category)))
            {
                // get commands associated with the category from commanddata (ran out of names)
                // return list of command class type
                var commands = CategoryCommands[(Category)enumCategory];
                string commandCategoryString = "";
                int conter = 0;
                // get and append command associated with the category
                foreach (var command in commands)
                {
                    conter++;
                    if (conter % 2 == 0)
                    {
                        commandCategoryString += ",";
                        conter = 1;
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

        public static Dictionary<string, Embed> listofcommands = new();
        private static List<CommandData> FunCommands = new();
        private static List<CommandData> MiscCommands = new();
        private static List<CommandData> ModerationCommands = new();
        private static List<CommandData> DebugCommands = new();
        
        internal static List<CommandData> Commands = new();
        internal static Dictionary<string, CommandData> CommandsDic = new();
        internal static Dictionary<Category, List<CommandData>> CategoryCommands = new Dictionary<Category, List<CommandData>>();
    }
}
