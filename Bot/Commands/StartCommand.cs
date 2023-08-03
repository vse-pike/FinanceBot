using FinanceBot.DbSettings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = FinanceBot.DbSettings.ORM.User;
using Telegram.Bot.Types.ReplyMarkups;

namespace FinanceBot.Bot.Commands;

public class StartCommand : ICommand, IExecuteCommand
{
    public async Task Execute(Update update, ITelegramBotClient bot)
    {
        ApplicationContext db = new();
        try
        {
            await AddNewUserToDb(update, db);
            await CreateKeyboard(update, bot);
        }
        catch (Exception e)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(e));
        }
    }

    private async Task AddNewUserToDb(Update update, ApplicationContext db)
    {
        await using (db)
        {
            User user = new User
            {
                UserId = update.Message!.From!.Id,
                Name = update.Message.From!.Username
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user));
        }
    }

    private async Task CreateKeyboard(Update update, ITelegramBotClient bot)
    {
        CommandManager commandManager = new();
        List<Command> commands = commandManager.GetAllCommands();
        InlineKeyboardMarkup keyboard = Keyboard.GetKeyboardMarkup(commands);
        
        await bot.SendTextMessageAsync(update.Message!.Chat, "Привет, это пример с кнопкой!", replyMarkup: keyboard);
    }
}