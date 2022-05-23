using System;

namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CooldownAttribute: Attribute
    {
        public UInt64 sec;
        public CooldownAttribute(UInt64 sec)
        {
            this.sec = sec;
        }
    }
}
