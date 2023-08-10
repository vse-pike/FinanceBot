using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = FinanceBot.DbSettings.ORM.User;


namespace FinanceBot.Bot.Commands;

public class GetIncomeCommand : ICommand, IReceiveCommand<List<Income>?>
{
    public async Task<List<Income>?> Receive(Update update, ITelegramBotClient bot)
    {
        ApplicationContext db = new();

        try
        {
            var incomes = await GetListIncomesFromDb(update, db);
            return incomes;
        }
        catch (Exception e)
        {
            Logger.Log(LoggLevel.ERROR, e);
        }

        return null;
    }

    private async Task<List<Income>?> GetListIncomesFromDb(Update update, ApplicationContext db)
    {
        var userId = update.CallbackQuery?.From.Id ?? update.Message?.From?.Id;

        await using (db)
        {
            User? user = db.Users.FirstOrDefault(u => u.UserId == userId);
            List<Income> incomes = db.Incomes.Where(income => income.UserId == userId).ToList();

            if (user != null && incomes[0].UserId == user.UserId)
            {
                Logger.Log(LoggLevel.INFO, incomes);
                return incomes;
            }
        }
        return null;
    }
}