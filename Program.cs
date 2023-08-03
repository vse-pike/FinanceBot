using FinanceBot.Bot;
using FinanceBot.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace FinanceBot
{
    class Program
    {
        private static readonly string Token = Environment.GetEnvironmentVariable("BOT_TOKEN")!;
        private static readonly ITelegramBotClient Bot = new TelegramBotClient(Token);
        private static readonly CommandManager CommandManager = new();
        private static readonly Dictionary<long, string> UserCurrentCommand = new();

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    await HandleCallbackAsync(botClient, update);
                    break;
                case UpdateType.Message:
                    await HandleMessageAsync(botClient, update);
                    break;
                default:
                    await botClient.SendTextMessageAsync(update.Id, "Command unknown");
                    break;
            }
        }

        private static async Task HandleCallbackAsync(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.CallbackQuery!.Message!.Chat.Id;
            UserCurrentCommand[chatId] = "default";

            Command? command = CommandManager.GetCommand(update.CallbackQuery!.Data!);

            if (command?.CommandInstance is IExecuteCommand instance)
            {
                await instance.Execute(update, botClient);
                //Если команда подразумевает чтение и обработку сообщения, то должeн быть указан CommandPath для обработки
                // этого сообщения в HandleMessageAsync
                if (command.Type == CommandType.Receive)
                {
                    UserCurrentCommand[chatId] = command.CommandPath;
                }
            }
        }

        private static async Task HandleMessageAsync(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.Message!.Chat.Id;

            if (UserCurrentCommand.TryGetValue(chatId, out string? commandPath))
            {
                Command? command = CommandManager.GetCommand(commandPath);
                if (command?.CommandInstance is IReceiveCommand instance)
                {
                    await instance.Receive(update, botClient);
                    UserCurrentCommand[chatId] = "default";
                }
            }
            else if (update.Message.Text == "/start")
            {
                Command? command = CommandManager.GetCommand("/start");
                if (command?.CommandInstance is IExecuteCommand instance)
                {
                    await instance.Execute(update, botClient);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(update.Id, "Command unknown OR Wrong format");
            }
        }

        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine(Token);
            Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}