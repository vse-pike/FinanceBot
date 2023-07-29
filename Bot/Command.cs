using FinanceBot.Bot.Commands;

namespace FinanceBot.Bot;

public class Command
{
    public string Name { get; set; } 
    public string Description { get; set; }
    public IBotCommand CommandInstance { get; set; }
}