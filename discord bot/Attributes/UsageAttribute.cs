using System;

namespace Bot.Attributes
{
    //
    // Summary:
    //     Attaches a summary to your command.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class UsageAttribute : Attribute
    {
        public UsageAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; } = "not provided";
    }
}