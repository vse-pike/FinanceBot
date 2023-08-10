using System.Text.RegularExpressions;

namespace FinanceBot.Bot.Commands.CommandHelpers;

public static class IncomeCommandHelper
{
    private static string _pattern = @"^\d+ [A-Z]{3} \w+$";

    public static (long amount, string currency, string incomeName) ParseContent(string content)
    {
        var array = content.Split(" ");
        long amount = Convert.ToInt32(array[0]);
        string currency = array[1];
        string incomeName = array[2];
        
        return (amount, currency, incomeName);
    }
    
    public static bool IsValidFormat(string content)
    {
        return Regex.IsMatch(content, _pattern);
    }
}