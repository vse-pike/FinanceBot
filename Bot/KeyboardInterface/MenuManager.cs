using FinanceBot.Bot.ButtonActions;

namespace FinanceBot.Bot;

public class MenuManager
{
    private List<Button> _buttons = new();

    public MenuManager()
    {
        //Кнопки дохода
        _buttons.Add(new Button
        {
            Name = "Доход: добавить новую запись",
            CallBack = "action:add_income",
            ActionInstance = new AddIncomeAction()
        });
        _buttons.Add(new Button
        {
            Name = "Доход: изменить существующую запись",
            CallBack = "action:change_income",
            ActionInstance = new ChangeIncomeAction()
        });
        _buttons.Add(new Button
        {
            Name = "Доход: удалить существующую запись",
            CallBack = "action:delete_income",
            ActionInstance = new DeleteIncomeAction()
        });
        // _buttons.Add(new Button
        // {
        //     Name = "Доход: вывести отчет",
        //     CallBack = "action:income_history"
        // });

        //Кнопки инвестиций
        // _buttons.Add(new Button
        // {
        //     Name = "Инвестиции: добавить новую запись",
        //     CallBack = "action:add_investment"
        // });
        // _buttons.Add(new Button
        // {
        //     Name = "Инвестиции: добавить новый отчет",
        //     CallBack = "action:add_investment_report"
        // });
        // _buttons.Add(new Button
        // {
        //     Name = "Инвестиции: удалить запись",
        //     CallBack = "action:delete_investment"
        // });
        //
        // _buttons.Add(new Button
        // {
        //     Name = "Инвестиции: вывести общий отчет",
        //     CallBack = "action:investment_history"
        // });
    }

    public List<Button> GetAllButtons()
    {
        return _buttons;
    }

    public Button? GetButton(string callBack)
    {
        return _buttons.FirstOrDefault(b => b.CallBack == callBack);
    }
}

public class Button
{
    public string Name { get; set; }
    public string CallBack { get; set; }
    public IButtonAction ActionInstance { get; set; }
}