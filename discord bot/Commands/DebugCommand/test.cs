﻿using Discord.Commands;
using System.Threading.Tasks;
using Bot.Attributes;
using System.Collections.Generic;
using System;

namespace Bot.Commands.DebugCommand
{
    
    public class test : ModuleBase<SocketCommandContext>
    {
        [Cooldown(20)]
        [Command("test")]
        public Task testAsync()
        {
            return Task.CompletedTask;            
        }
        public static List<String> vas = new();
        public static Dictionary<String, List<String>> vcb = new();
    }
}
