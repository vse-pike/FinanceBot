using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = FinanceBot.DbSettings.ORM.User;


namespace FinanceBot.Bot.Commands;

public class AddIncomeCommand : IBotCommand
{
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        ApplicationContext db = new();
        var message = update.Message!;

        try
        {
            await AddNewIncome(message, db);
            //await bot.SendTextMessageAsync(message.Chat, TextStorage.AddIncomeText);
        }
        catch (Exception e)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(e));
        }
        //await bot.SendTextMessageAsync(message.Chat, "Добавить доход");
    }

    private async Task AddNewIncome(Message message, ApplicationContext db)
    {
        await using (db)
        {
            User? user = db.Users.FirstOrDefault(u => u.UserId == message.From!.Id);

            if (user != null)
            {
                Income income = new Income
                {
                    
                };

                db.Incomes.Add(income);
                await db.SaveChangesAsync();
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(income));
            }
        }
    }
}