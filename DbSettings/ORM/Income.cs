namespace FinanceBot.DbSettings.ORM;

public class Income
{
    public Guid IncomeId { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public long Value { get; set; }
    public string Currency { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}