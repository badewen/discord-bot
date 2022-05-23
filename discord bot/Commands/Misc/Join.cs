/*
using Bot.Attributes;
using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bot.Commands.Misc
{
    public class Join : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".play <youtube link>";
        private const string Description = "play music. will join your channel";

        [RequireBotPermission(ChannelPermission.Connect | ChannelPermission.Speak)]
        //[Command("play",RunMode = RunMode.Async)]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Misc)]
        public async Task PlayAsync(string link)
        {
            IVoiceChannel userchannel = Context.Guild.GetUser(Context.Message.Author.Id).VoiceChannel;
            if (userchannel == null)
            {
                await ReplyAsync("join a voice channel duh.", messageReference: new MessageReference(Context.Message.Id));
                return;
            }
            //regex the link so less expensive process
            if (link.Length == 11)
            {
                Regex re = new Regex(@"[A-Za-z0-9_-]{11}");
                if (!re.IsMatch(link))
                {
                    await ReplyAsync("not a valid id.", messageReference: new MessageReference(Context.Message.Id));
                    return;
                }
            }

            Stream output = Stream.Null;
            ProcessStartInfo youtubedlInfo = Utils.PrepareYoutubedl("-g " + link);
            Process youtubedlExec = Process.Start(youtubedlInfo);
            output = youtubedlExec.StandardOutput.BaseStream;

            // amount of dot for waiting message
            int dot = 1;
            var message = await Context.Message.Channel.SendMessageAsync($"finding video {String.Concat(Enumerable.Repeat(".", dot))}");
            while (output.Length == 0)
            {
                // youtubedl runs for about 2.5 seconds until 4 seconds on my machine
                // this delay shan't hurt isn't it?
                await Task.Delay(250);

                await message.ModifyAsync(m =>
                {
                    if (dot != 3)
                    {
                        dot++;
                    }
                    else
                    {
                        dot = 1;
                    }

                    var strDot = String.Concat(Enumerable.Repeat(".", dot).ToArray());
                    m.Content = $"finding video {strDot}";
                });
            }

            if (output.Length == 24 || output.Length == 25) // 25 here is just incase
            {
                await ReplyAsync("Video not found", messageReference: new MessageReference(Context.Message.Id));
                await output.FlushAsync();
                return;
            }
            bool livestreamlnk = false;
            if (Utils.IsLivestreamLink(link))
            {
                await ReplyAsync("getting livestream video, please wait...", messageReference: new MessageReference());
                livestreamlnk = true;
                youtubedlInfo = Utils.PrepareYoutubedl(link);
                youtubedlExec = Process.Start(youtubedlInfo);
                output = youtubedlExec.StandardOutput.BaseStream;
                while (output.Length == 0) { await Task.Delay(250); }
            }
            var client = await userchannel.ConnectAsync();
            var channel = userchannel as Discord.WebSocket.SocketVoiceChannel;
            if (!livestreamlnk)
            {
                await SendAsync(channel, client, output.ToString().Split(' ')[1]); //the spliet[1] thing is to get the audio only
            }
            else
            {
                do
                {
                    await SendAsync(channel, client, " ", output); // sends the byte
                } while (channel.Users.Count > 1);
            }
            output.Flush();
            return;
        }

        private async Task SendAsync(Discord.WebSocket.SocketVoiceChannel channel, IAudioClient client, string link, Stream input = null)
        {
            ProcessStartInfo ffmpeg = null;
            Process ffmpegExec = null;
            UInt64 lastSec = 0;
            do
            {
                // livestream video "stream"
                // or this link is a livestream link
                if (input != null)
                {
                    ffmpeg = Utils.PrepareFFmpeg(stdin: true);
                    ffmpegExec = Process.Start(ffmpeg);
                    await input.CopyToAsync(ffmpegExec.StandardInput.BaseStream);
                }
                else
                {
                    ffmpeg = Utils.PrepareFFmpeg(link, lastSec, lastSec += 15, stdin: false);
                    ffmpegExec = Process.Start(ffmpeg);
                }
                Console.WriteLine(link);
                Console.WriteLine(ffmpeg.Arguments);
                while (ffmpegExec.StandardOutput.BaseStream.Length == 0) { await Task.Delay(1); }
                using (var result = ffmpegExec.StandardOutput.BaseStream)
                using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
                {
                    try { await result.CopyToAsync(discord); }
                    finally
                    {
                        await discord.FlushAsync();
                    }
                }
            } while (ffmpegExec.StandardOutput.BaseStream.Length != 0 || channel.Users.Count > 1);
            return;
        }

        private string patchLink(string link)
        {
            if (link == null) return String.Empty;
            link = link.Trim();
            // it is an id
            if (!link.StartsWith("https://www.youtube.com/watch?v="))
            {
                return "https://www.youtube.com/watch?v=" + link;
            }
            // it is a "lazy" link
            if (!link.StartsWith("https://"))
            {
                return "https://" + link;
            }
            return String.Empty;
        }
    }
}
*/