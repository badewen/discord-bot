using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Discord.Commands;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode;
using Discord.Audio;
using Discord;
using CliWrap;
using Bot.Attributes;
using System.Threading.Tasks;

namespace Bot.Commands.Misc
{
    public class Join : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".join <channel (optional)>";
        private const string Description = "join voice channel. If no channel is provided, it will join your voice channel";
        [Command("join",RunMode = RunMode.Async)]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Misc)]
        public async Task PlayAsync(IVoiceChannel channel = null)
        {
            var client = await channel.ConnectAsync();
            await SendAsync(client);
            return;
        }
        
        private async Task SendAsync(IAudioClient client)
        {
            
            YoutubeClient youtube = new YoutubeClient();
            var streammanifest = await youtube.Videos.Streams.GetManifestAsync("YykjpeuMNEk");
            
            var streamInfo = streammanifest
                .GetAudioOnlyStreams()
                .TryGetWithHighestBitrate();

            ProcessStartInfo ffmpeg = null;
            var exitcode = 0;
            
            ffmpeg = Utils.PrepareFFmpeg(streamInfo.Url);
            Console.WriteLine(streamInfo.Url);
            Console.WriteLine(ffmpeg.Arguments);
            
            using (var a = Process.Start(ffmpeg))
            using (var result = a.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try { await result.CopyToAsync(discord); }
                finally { 
                    await discord.FlushAsync();
                }
            }
            return;
        }
    }
}
