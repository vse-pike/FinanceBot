using FinanceBot.DbSettings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = FinanceBot.DbSettings.ORM.User;
using FinanceBot.Bot.Text;

namespace FinanceBot.Bot.Commands;

public class StartCommand : IBotCommand
{
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        ApplicationContext db = new();
        var message = update.Message!;

        try
        {
            await CreateNewUser(message, db);
            await bot.SendTextMessageAsync(message.Chat, TextStorage.StartText);
        }
        catch (Exception e)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(e));
        }
    }

    private async Task CreateNewUser(Message message, ApplicationContext db)
    {
        await using (db)
        {
            User user = new User
            {
                UserId = message.From!.Id,
                Name = message.From!.Username
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user));
        }
    }
}