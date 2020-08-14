using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using LOLS.Modules;

namespace LOLS
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public static DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection()
                .AddSingleton(this)
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton<ConfigHandler>()
                .BuildServiceProvider();
            client.Log += Logger.Log;

            await services.GetService<ConfigHandler>().PopulateConfig();
            await RegisterCommandAsync();
            await client.LoginAsync(TokenType.Bot, services.GetService<ConfigHandler>().Token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task RegisterCommandAsync()
        {
            client.MessageReceived += handleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task handleCommandAsync(SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            SocketCommandContext context = new SocketCommandContext(client, message);

            if (message.Author.IsBot) return;

            int argPos = 0;
            if(message.HasStringPrefix("!", ref argPos))
            {
                await Logger.Log($"log {message.Author.Id} {message.Content.Split(' ')[0]}");
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess) await Logger.Log($"error {result.ErrorReason}");
            }
        }
    }
}
