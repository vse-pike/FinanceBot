using FinanceBot.Bot.Commands;

namespace FinanceBot.Bot;

public class Command
{
    public string Name { get; set; }
    public CommandType Type { get; set; }
    public string CommandPath { get; set; }
    public ICommand CommandInstance { get; set; }
}