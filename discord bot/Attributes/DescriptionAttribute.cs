using System;

namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        public string Text
        {
            get;
        }
        public DescriptionAttribute(string description = "No description is provided")
        {
            Text = description;
        }
    }
}
