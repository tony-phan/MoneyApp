using System.ComponentModel;

namespace money_api.Models.Enums;

public enum ExpenseCategory
{
    [Description("Rent")]
    Rent,
    [Description("Utilities")]
    Utilities,
    [Description("Groceries")]
    Groceries,
    [Description("Transportation")]
    Transportation,
    [Description("Entertainment")]
    Entertainment,
    [Description("EatingOut")]
    EatingOut,
    [Description("Insurance")]
    Insurance,
    [Description("Miscellaneous")]
    Miscellaneous,
    [Description("")]
    None
}
