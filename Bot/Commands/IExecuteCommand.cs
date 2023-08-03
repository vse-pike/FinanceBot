using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot.Commands;

public interface IExecuteCommand
{
    Task Execute(Update update, ITelegramBotClient bot);
}