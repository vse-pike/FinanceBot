using FinanceBot.Bot.Commands.CommandHelpers;
using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = FinanceBot.DbSettings.ORM.User;


namespace FinanceBot.Bot.Commands;

public class DeleteIncomeCommand : ICommand, IExecuteCommand
{
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        ApplicationContext db = new();

        try
        {
            await DeleteIncomeInDb(update, db);
        }
        catch (Exception e)
        {
            Logger.Log(LoggLevel.ERROR, e);
        }
    }

    private async Task DeleteIncomeInDb(Update update, ApplicationContext db)
    {
        var chatId = update.CallbackQuery?.Message!.Chat.Id ?? update.Message!.Chat.Id;

        await using (db)
        {
            User? user = db.Users.FirstOrDefault(u => u.UserId == chatId);
            Income? income = db.Incomes.FirstOrDefault(i => i.IncomeId == new Guid(update.CallbackQuery!.Data!));

            if (user != null && income != null && income.UserId == user.UserId)
            {
                db.Incomes.Remove(income);
                await db.SaveChangesAsync();
                Logger.Log(LoggLevel.INFO, income);
            }
        }
    }
}