using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot;

public interface IButtonAction
{
    Task Action(Update update, ITelegramBotClient bot, Dictionary<long, string> userCurrentCommand);
}