using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot.Commands;

public class AddIncomeCommand: IBotCommand
{
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        var message = update.Message;
        await bot.SendTextMessageAsync(message.Chat, "Добавить доход");
    }
}