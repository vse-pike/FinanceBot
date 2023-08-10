using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;
using FinanceBot.Bot.Commands.CommandHelpers;
using User = FinanceBot.DbSettings.ORM.User;


namespace FinanceBot.Bot.Commands;

public class AddIncomeCommand : ICommand, IExecuteCommand
{

    public async Task Execute(Update update, ITelegramBotClient botClient)
    {
        var message = update.Message!.Text!;
        ApplicationContext db = new();

        try
        {
            if (IncomeCommandHelper.IsValidFormat(message))
            {
                (long amount, string currency, string incomeName) = IncomeCommandHelper.ParseContent(message);
                await AddNewIncomeToDb(amount, currency, incomeName, update, db);
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

    private async Task AddNewIncomeToDb(long amount, string currency, string incomeName, Update update, ApplicationContext db)
    {
        var userId = update.Message!.From!.Id;
        
        await using (db)
        {
            User? user = db.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                Income income = new Income
                {
                    IncomeId = Guid.NewGuid(),
                    UserId = user.UserId,
                    Name = incomeName,
                    Value = amount,
                    Currency = currency,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                db.Incomes.Add(income);
                await db.SaveChangesAsync();
                Logger.Log(LoggLevel.INFO, income);
            }
        }
    }
}