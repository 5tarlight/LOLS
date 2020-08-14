using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LOLS.Commands
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("핑")]
        public async Task PingC()
        {
            EmbedBuilder builder = new EmbedBuilder();
            int ping = Program.client.Latency;

            builder.WithTitle("Pong!");
            builder.WithDescription($"**{ping} ms**");

            Color color;

            if (ping <= 50)
                color = Color.Green;
            else if (ping <= 150)
                color = Color.Orange;
            else
                color = Color.Red;

            builder.WithColor(color);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
