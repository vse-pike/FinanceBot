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
        private static readonly MenuManager MenuManager = new();
        private static readonly Dictionary<long, string> UserCurrentCommand = new();

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            Logger.Log(LoggLevel.INFO, update);

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
                    Logger.Log(LoggLevel.WARN, update);
                    break;
            }
        }

        private static async Task HandleCallbackAsync(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var chatId = update.CallbackQuery!.Message!.Chat.Id;

                //UserCurrentCommand[chatId] = "default";

                var callback = update.CallbackQuery!.Data!;

                if (callback.StartsWith("action"))
                {
                    Button? button = MenuManager.GetButton(callback);
                    if (button != null)
                    {
                        await button.ActionInstance.Action(update, botClient, UserCurrentCommand);
                    }
                }
                else if (UserCurrentCommand.TryGetValue(chatId, out string? userState))
                {
                    Command? command = CommandManager.GetCommand(userState);
                    if (command?.CommandInstance is IExecuteCommand instance)
                    {
                        await instance.Execute(update, botClient);
                        //Если команда подразумевает чтение и обработку сообщения, то должeн быть указан CommandPath для обработки
                        //этого сообщения в HandleMessageAsync
                        // if (command.Type == CommandType.Receive)
                        // {
                        //     UserCurrentCommand[chatId] = command.CallBack;
                        // }
                    }
                }
                else
                {
                    Logger.Log(LoggLevel.ERROR, "Command not found");
                }
            }
            catch (Exception e)
            {
                Logger.Log(LoggLevel.ERROR, e);
            }
        }

        private static async Task HandleMessageAsync(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var chatId = update.CallbackQuery?.Message!.Chat.Id ?? update.Message!.Chat.Id;

                if (UserCurrentCommand.TryGetValue(chatId, out string? userState))
                {
                    Command? command = CommandManager.GetCommand(userState);
                    if (command?.CommandInstance is IExecuteCommand instance)
                    {
                        await instance.Execute(update, botClient);
                        UserCurrentCommand[chatId] = "default";
                    }
                }
                else if (update.Message.Text == "/start")
                {
                    await new StartCommand().Execute(update, botClient);
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Id, "Command unknown OR Wrong format");
                }
            }
            catch (Exception e)
            {
                Logger.Log(LoggLevel.ERROR, e);
            }
        }

        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            Logger.Log(LoggLevel.ERROR, exception);
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