using System.Collections;
using FinanceBot.Bot.Commands;

namespace FinanceBot.Bot;

public class BotCommandManager: IEnumerable
{
    private List<Command> _commands = new();

    public BotCommandManager()
    {
        _commands.Add(new Command
        {
            Name = "/start",
            Description = "Инициализация бота",
            CommandInstance = new StartCommand()
        });
        _commands.Add(new Command
        {
            Name = "/add_income",
            Description = "Добавить доход",
            CommandInstance = new AddIncomeCommand()
        });
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

    public IBotCommand? GetCommandInstance(string commandName)
    {
        Command command = _commands.FirstOrDefault(c => c.Name == commandName);
        return command?.CommandInstance;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _commands.GetEnumerator();
    }
}