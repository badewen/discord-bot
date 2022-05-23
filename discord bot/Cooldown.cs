using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bot;

namespace Bot
{
    internal class Cooldown
    {

        internal static void PrepareCooldown()
        {
            foreach (var (Key, _) in CommandList.CommandsDic)
            {
                Cooldowns.Add(Key, new());

                Console.WriteLine(Key);
            }

        }

        internal static void UpdateUserCooldown(string CommandName, UInt64 id)
        {
            if (!Cooldowns[CommandName].ContainsKey(id))
            {
                Cooldowns[CommandName].Add(id, (UInt64)(DateTime.UtcNow - new DateTime(2010, 1, 1)).TotalSeconds);
            } else  {
                Cooldowns[CommandName][id] = (UInt64)(DateTime.UtcNow - new DateTime(2010, 1, 1)).TotalSeconds;
            }
        }

        internal static (bool, UInt64) IsCooldownDone(UInt64 sec, string commandName, UInt64 id)
        {
            var secPassed = (UInt64)(DateTime.UtcNow - new DateTime(2010, 1, 1)).TotalSeconds - Cooldowns[commandName][id];
            return (secPassed >= sec, secPassed);
        }

        internal static void AddUser(string commandName, UInt64 id)
        {
            if (!Cooldowns[commandName].ContainsKey(id))
            {
                Cooldowns[commandName].Add(id, (UInt64)(DateTime.UtcNow - new DateTime(2010, 1, 1)).TotalSeconds);
            }
        }

        internal static bool IsUserExist(string commandName, UInt64 id)
        {
            if (!Cooldowns[commandName].ContainsKey(id))
            {
                return false;
            }
            return true;
        }

        /*
         * string = command name
         * Uint64 = user id
         * Uint64 = Last Cooldown Time in seocnsd
         * 
         */
        internal static Dictionary<string, Dictionary<UInt64,UInt64>> Cooldowns = new();
    }
}
