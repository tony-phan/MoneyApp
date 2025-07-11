using System.ComponentModel;

namespace money_api.Models.Enums;

public enum IncomeCategory
{
    [Description("salary")]
    Salary,
    [Description("bonus")]
    Bonus,
    [Description("investments")]
    Investments,
    [Description("miscellaneous")]
    Miscellaneous,
    [Description("")]
    None
}
