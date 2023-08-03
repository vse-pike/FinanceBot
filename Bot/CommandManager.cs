using System.Collections;
using FinanceBot.Bot.Commands;

namespace FinanceBot.Bot;

public class CommandManager : IEnumerable
{
    private List<Command> _commands = new();

    public CommandManager()
    {
        _commands.Add(new Command
        {
            Name = "Инициализация бота",
            Type = CommandType.Execute,
            CommandPath = "/start",
            CommandInstance = new StartCommand()
        });

        _commands.Add(new Command
        {
            Name = "Добавить доход",
            Type = CommandType.Receive,
            CommandPath = "/add_income",
            CommandInstance = new AddIncomeCommand()
        });
        
        // _commands.Add( new Command
        // {
        //     Name = "Вывести отчет о доходах",
        //     Type = new List<CommandType>
        //     {
        //         CommandType.Executor,
        //         CommandType.Sender
        //     },
        //     CommandPath = "/income_history",
        //     CommandInstance = new IncomeHistoryCommand()
        // });

        // _commands.Add(new Command
        // {
        //     Name = "Добавить инвестицию",
        //     CommandPath = "/add_investment",
        //     CommandInstance = new AddIncomeCommand()
        // });
        // commands =  [
        //     telebot.types.BotCommand('add_income', 'Добавить доход'),
        //     telebot.types.BotCommand('add_investment', 'Добавить инвестицию'),
        //     telebot.types.BotCommand('delete_income', 'Удалить доход'),
        //     telebot.types.BotCommand('delete_investment', 'Удалить инвестицию'),
        //     telebot.types.BotCommand('add_new_report', 'Добавить новый отчет'),
        //     telebot.types.BotCommand('change_income', 'Изменить доход'),
        //     telebot.types.BotCommand('income_history', 'Вывести отчет о доходах'),
        //     telebot.types.BotCommand('investment_history', 'Вывести отчет об инвестициях')
        // ]
    }

    public List<Command> GetAllCommands()
    {
        return _commands;
    }

    public ICommand? GetCommandInstance(string commandPath)
    {
        Command command = _commands.FirstOrDefault(c => c.CommandPath == commandPath);
        return command?.CommandInstance;
    }

    public Command? GetCommand(string commandPath)
    {
        return _commands.FirstOrDefault(c => c.CommandPath == commandPath);
    }

    public Command? GetCommandByName(string commandName)
    {
        return _commands.FirstOrDefault(c => c.Name == commandName);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _commands.GetEnumerator();
    }
}