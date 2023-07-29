using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot.Commands;

public interface IBotCommand
{
    Task Execute(Update update, ITelegramBotClient bot);
}