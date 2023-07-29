using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot.Commands;

public class StartCommand : IBotCommand
{
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        var message = update.Message;
        await bot.SendTextMessageAsync(message.Chat, "Инициализация бота");
    }
}