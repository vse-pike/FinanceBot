using FinanceBot.Bot.Commands.CommandHelpers;
using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = FinanceBot.DbSettings.ORM.User;


namespace FinanceBot.Bot.Commands;

public class ChangeIncomeCommand : ICommand, IExecuteCommand
{
    private static string _pattern = @"^\d+ [A-Z]{3} \w+$";

    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        var message = update.Message!.Text!;
        ApplicationContext db = new();

        try
        {
            if (IncomeCommandHelper.IsValidFormat(message))
            {
                (long amount, string currency, string incomeName) = IncomeCommandHelper.ParseContent(message);
                await ChangeIncomeInDb(amount, currency, incomeName, update, db);
            }
            else
            {
                Logger.Log(LoggLevel.WARN, $"Wrong format message: {message}");
            }
        }
        catch (Exception e)
        {
            Logger.Log(LoggLevel.ERROR, e);
        }
    }

    private async Task ChangeIncomeInDb(long amount, string currency, string incomeName, Update update,
        ApplicationContext db)
    {
        var userId = update.Message!.From!.Id;

        await using (db)
        {
            User? user = db.Users.FirstOrDefault(u => u.UserId == userId);
            Income? income = db.Incomes.FirstOrDefault(i => i.IncomeId == new Guid(update.CallbackQuery!.Data!));

            if (user != null && income != null && income.UserId == user.UserId)
            {
                income.Currency = currency;
                income.Name = incomeName;
                income.Value = amount;
                income.ModifiedDate = DateTime.Now;

                db.Incomes.Update(income);
                await db.SaveChangesAsync();
                Logger.Log(LoggLevel.INFO, income);
            }
        }
    }
}