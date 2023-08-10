using System.Collections;
using Telegram.Bot.Types.ReplyMarkups;

namespace FinanceBot.Bot;

public class Keyboard
{
    // public static InlineKeyboardMarkup GetKeyboardMarkup(List<Command> commands)
    // {
    //     List<InlineKeyboardButton[]> inlineKeyboardButtons = new List<InlineKeyboardButton[]>();
    //
    //     foreach (var command in commands)
    //     {
    //         var button = new[]
    //         {
    //             InlineKeyboardButton.WithCallbackData(command.Name, command.CallBack)
    //         };
    //         
    //         inlineKeyboardButtons.Add(button);
    //     }
    //
    //     InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);
    //
    //     return inlineKeyboardMarkup;
    // }
    
    public static InlineKeyboardMarkup GetKeyboardMarkup<T>(List<T> arr, Func<T, string> getName, Func<T, string> getCallBack)
    {
        List<InlineKeyboardButton[]> inlineKeyboardButtons = new List<InlineKeyboardButton[]>();

        foreach (var value in arr)
        {
            var button = new[]
            {
                InlineKeyboardButton.WithCallbackData(getName(value), getCallBack(value))
            };
            
            inlineKeyboardButtons.Add(button);
        }

        InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

        return inlineKeyboardMarkup;
    }
    
    
}

