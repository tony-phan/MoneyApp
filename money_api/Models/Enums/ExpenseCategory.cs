using System.ComponentModel;

namespace money_api.Models.Enums;

public enum ExpenseCategory
{
    [Description("rent")]
    Rent,
    [Description("utilities")]
    Utilities,
    [Description("groceries")]
    Groceries,
    [Description("transportation")]
    Transportation,
    [Description("entertainment")]
    Entertainment,
    [Description("eatingOut")]
    EatingOut,
    [Description("insurance")]
    Insurance,
    [Description("miscellaneous")]
    Miscellaneous,
    [Description("")]
    None
}
