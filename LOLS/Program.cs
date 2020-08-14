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

        private DiscordSocketClient client;
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
            client.Log += _client_Log;

            await services.GetService<ConfigHandler>().PopulateConfig();
            await RegisterCommandAsync();
            await client.LoginAsync(TokenType.Bot, services.GetService<ConfigHandler>().Token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
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
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
