using System;

namespace Bot
{
    public record CommandData
    {
        public string Usage;
        public string Description;
        public string CommandMethodName;
        public Type CommandClass;//your command class type
        public Category Category;
        public UInt64 CooldownSec;

        public CommandData(string Usage, string Description, string CommandMethodName, Type CommandClass, Category Category, UInt64 CooldownSec = 0)
        {
            this.Usage = Usage;
            this.Description = Description;
            this.CommandMethodName = CommandMethodName;
            this.CommandClass = CommandClass;
            this.Category = Category;
            this.CooldownSec = CooldownSec;
        }
    }
}