using FinanceBot.DbSettings;
using FinanceBot.DbSettings.ORM;
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
            await CreateMenuKeyboard(update, bot);
        }
        catch (Exception e)
        {
            Logger.Log(LoggLevel.ERROR, e);
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
            Logger.Log(LoggLevel.INFO, user);
        }
    }

    private async Task CreateMenuKeyboard(Update update, ITelegramBotClient bot)
    {
        var menuButtons = new MenuManager().GetAllButtons();
        InlineKeyboardMarkup keyboard = Keyboard.GetKeyboardMarkup(menuButtons, button => button.Name, button => button.CallBack);
        
        //TODO: Добавить текст
        await bot.SendTextMessageAsync(update.Message!.Chat, "Меню команд:", replyMarkup: keyboard);
        Logger.Log(LoggLevel.INFO, keyboard);
    }
}