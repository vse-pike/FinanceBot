using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot.Commands;

public interface IReceiveCommand<T>
{
    Task<T> Receive(Update update, ITelegramBotClient botClient);
}