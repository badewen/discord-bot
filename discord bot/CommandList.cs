using Bot.Attributes;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bot
{
    public class CommandList
    {
        internal static void RegisterCommandListCategories()
        {
            _initCommandCategoriesDic();
            foreach(var category in Enum.GetValues<Category>())
            {
                CategoryCommands.Add(category, (CommandCategoriesDic["_" + Enum.GetName<Category>(category).ToLower() + "Commands"]));
            }
        }


        internal static void RegisterCommandsCategories()
        {
            foreach(CommandData data in Commands)
            {
                String categoryName = Enum.GetName<Category>(data.Category);
                categoryName = categoryName.ToLower();
                CommandCategoriesDic["_" + categoryName + "Commands"].Add(data);
            }
        }

        internal static void RegisterAllCommands()
        {
            Type[] modules = Assembly.GetEntryAssembly().GetTypes();
            List<Module> commandModules = new();
            foreach (Type module in modules)
            {
                MethodInfo[] methods = module.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    UsageAttribute usageResult;
                    DescriptionAttribute descriptionResult;
                    CooldownAttribute cooldownResult;

                    if (method.GetCustomAttribute<CommandAttribute>() == null)
                    {
                        continue;
                    }

                    usageResult = method.GetCustomAttribute<UsageAttribute>();
                    descriptionResult = method.GetCustomAttribute<DescriptionAttribute>();
                    cooldownResult = method.GetCustomAttribute<CooldownAttribute>();

                    string usage;
                    string description;
                    Category category = Category.Misc;
                    UInt64 sec;

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
                    if (cooldownResult == null)
                    {
                        sec = 0;
                    }
                    else
                    {
                        sec = cooldownResult.sec;
                    }
                    
                    // get the category
                    string namespaceName = module.Namespace.ToLower();;
                    {
                        Int32 dotPos;
                        dotPos = (Int32)namespaceName.LastIndexOf('.');
                        
                        string categoryName = namespaceName[new System.Index(dotPos)..];
                        bool found = false;
                        foreach((string key, Category value) in CategoryTable.Table){
                            if (categoryName.Contains(key.ToLower()))
                            {
                                found = true;
                                category = value;
                            }
                        }
                        if(!found){
                            category = Category.Misc;
                        }
                    }
                    
                    CommandData data = new CommandData(usage, description, method.Name, module, category, sec);
                    Commands.Add(data);
                }
            }
        }

        internal static void BuildCommandsEmbed()
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
            foreach (CommandData command in Commands)
            {
                // get all the alias
                var aliases = (AliasAttribute)Attribute.GetCustomAttribute(command.CommandClass.GetMethod(command.CommandMethodName), typeof(AliasAttribute));
                
                StringBuilder stringaliasesbuilder = new();

                // representable list of aliases
                string stringaliases = "";
                var commandEmbed = new EmbedBuilder();
                commandEmbed.AddField("Command", command.CommandClass.Name)
                             .AddField("Description", command.Description)
                             .AddField("Usage", command.Usage)
                             .AddField("Cooldown", command.CooldownSec);
                if (aliases != null)
                {
                    // appending or adding aliases to stringaliases
                    foreach (var alias in aliases.Aliases)
                    {
                        stringaliasesbuilder.Append(" , ");
                        stringaliasesbuilder.Append(alias);
                        CommandsDic.Add(alias.ToLower(), command);
                    }
                }
                stringaliases = stringaliasesbuilder.ToString();
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
                        commandCategoryString += ", ";
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
        }

        private static void _initCommandCategoriesDic()
        {
            CommandCategoriesDic.Add(nameof(_funCommands), _funCommands);
            CommandCategoriesDic.Add(nameof(_miscCommands), _miscCommands);
            CommandCategoriesDic.Add(nameof(_moderationCommands), _moderationCommands);
            CommandCategoriesDic.Add(nameof(_debugCommands), _debugCommands);
        }

        public static Dictionary<string, Embed> listofcommands = new();
        public static Dictionary<string, List<CommandData>> CommandCategoriesDic = new();
        private static List<CommandData> _funCommands = new();
        private static List<CommandData> _miscCommands = new();
        private static List<CommandData> _moderationCommands = new();
        private static List<CommandData> _debugCommands = new();

        internal static List<CommandData> Commands = new();
        internal static Dictionary<string, CommandData> CommandsDic = new();
        internal static Dictionary<Category, List<CommandData>> CategoryCommands = new Dictionary<Category, List<CommandData>>();
    }
}