using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;
using User = FinanceBot.DbSettings.ORM.User;


namespace FinanceBot.Bot.Commands;

public class AddIncomeCommand : ICommand, IExecuteCommand, IReceiveCommand
{

    private static string _pattern = @"^\d{4} [A-Z]{3} \w+$";
    
    
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        await bot.SendTextMessageAsync(update.CallbackQuery.Message.Chat, "Добавить доход");
    }

    public async Task Receive(Update update, ITelegramBotClient botClient)
    {
        Console.WriteLine("Запускаю команду обработки сообщения!!!!");
    }

    public bool IsValidFormat(string content)
    {
        return Regex.IsMatch(content, _pattern);
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