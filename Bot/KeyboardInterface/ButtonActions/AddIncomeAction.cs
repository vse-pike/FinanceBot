using FinanceBot.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FinanceBot.Bot.ButtonActions;

public class AddIncomeAction : IButtonAction
{
    public async Task Action(Update update, ITelegramBotClient bot, Dictionary<long, string> userCurrentCommand)
    {
        var chatId = update.CallbackQuery?.Message!.Chat.Id ?? update.Message!.Chat.Id;

        userCurrentCommand[chatId] = "add_income";
        
        //TODO: Добавить текст
        await bot.SendTextMessageAsync(update.CallbackQuery!.Message!.Chat, "Добавить доход");
    }
}