using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using money_api.Models.Enums;

namespace money_api.Models;

public class Transaction
{
    public int Id { get; set; }
    public int TransactionHistoryId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public IncomeCategory? IncomeCategory { get; set; }
    public ExpenseCategory? ExpenseCategory { get; set; }
    public required string Description { get; set; }
    public DateTime Date { get; set; } // mm/dd/yyyy

    // Navigation Property
    public required TransactionHistory TransactionHistory { get; set; }
}
