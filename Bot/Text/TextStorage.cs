namespace FinanceBot.Bot.Text;

public class TextStorage
{
    public static string StartText { get; } = @"
Инициализация бота

Для взаимодействия с ботом доступны следующие команды:
/add_income - Добавить доход
/add_investment - Добавить инвестицию
/add_new_report - Добавить новый отчет
/change_income - Изменить доход
/income_history - Вывести отчет о доходах
/investment_history - Вывести отчет об инвестициях
";
}