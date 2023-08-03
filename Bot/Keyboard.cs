using System.Collections;
using Telegram.Bot.Types.ReplyMarkups;

namespace FinanceBot.Bot;

public class Keyboard
{
    public static InlineKeyboardMarkup GetKeyboardMarkup(List<Command> commands)
    {
        List<InlineKeyboardButton[]> inlineKeyboardButtons = new List<InlineKeyboardButton[]>();

        foreach (var command in commands)
        {
            var button = new[]
            {
                InlineKeyboardButton.WithCallbackData(command.Name, command.CommandPath)
            };
            
            inlineKeyboardButtons.Add(button);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(button));

        }

        InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

        return inlineKeyboardMarkup;
    }
}