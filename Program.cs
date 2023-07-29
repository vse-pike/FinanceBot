using FinanceBot.Bot;
using FinanceBot.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;

namespace FinanceBot
{
    class Program
    {
        static string _token = Environment.GetEnvironmentVariable("BOT_TOKEN");
        static ITelegramBotClient _bot = new TelegramBotClient(_token);
        static BotCommandManager _commands = new();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text != null)
                {
                    IBotCommand? botCommand = _commands.GetCommandInstance(message.Text);

                    if (botCommand != null)
                    {
                        await botCommand.Execute(update, botClient);
                    }
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static async Task SetBotCommandsAsync()
        {
            var commands = new List<BotCommand>();

            foreach (Command command in _commands)
            {
                commands.Add(new BotCommand
                {
                    Command = command.Name,
                    Description = command.Description
                });
            }

            await _bot.SetMyCommandsAsync(commands);
        }


        static async Task Main(string[] args)
        {
            Console.WriteLine(_token);
            Console.WriteLine("Запущен бот " + _bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            await SetBotCommandsAsync();

            _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}