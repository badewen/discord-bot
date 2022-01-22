using System;
using Bot.Commands;

namespace Bot
{
    public struct CommandData
    {
        public string Usage;
        public string Description;
        public string CommandMethodName;
        public Type CommandClass;//your command class type
        public Categories Category; 

        public CommandData(string Usage , string Description , string CommandMethodName, Type CommandClass, Categories Category)
        {
            this.Usage = Usage;
            this.Description = Description;
            this.CommandMethodName = CommandMethodName;
            this.CommandClass = CommandClass;
            this.Category = Category;
        }
    }
}
