using FinanceBot.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FinanceBot.Bot.ButtonActions;

public class ChangeIncomeAction : IButtonAction
{
    public async Task Action(Update update, ITelegramBotClient bot, Dictionary<long, string> userCurrentCommand)
    {
        var chatId = update.CallbackQuery?.Message!.Chat.Id ?? update.Message!.Chat.Id;
        
        userCurrentCommand[chatId] = "change_income";

        var incomes = await new GetIncomeCommand().Receive(update, bot);

        if (incomes != null)
        {
            InlineKeyboardMarkup keyboard = Keyboard.GetKeyboardMarkup(incomes, income => income.Name,
                income => Convert.ToString(income.IncomeId)!);

            //TODO: Написать норм текст для меню
            await bot.SendTextMessageAsync(chatId, "Список доходов:", replyMarkup: keyboard);
            Logger.Log(LoggLevel.INFO, keyboard);
        }
        else
        {
            await bot.SendTextMessageAsync(chatId, "Не найдено ни одной записи для изменения");
        }
    }
}