using System;

namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UsageAttribute : Attribute
    {
        public string Text
        {
            get;
        }
        public UsageAttribute(string usage = "No Information is provided")
        {
            Text = usage;
        }
    }
}
