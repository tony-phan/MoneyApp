using System.ComponentModel;

namespace money_api.Models.Enums;

public enum IncomeCategory
{
    [Description("Salary")]
    Salary,
    [Description("Bonus")]
    Bonus,
    [Description("Investments")]
    Investments,
    [Description("")]
    None
}
