using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.DTOs.TransactionDtos;

public class TransactionUpdateDto
{
    public decimal Amount { get; set; }
    public required string TransactionType { get; set; }
    public string? IncomeCategory { get; set; }
    public string? ExpenseCategory { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } // Format expected: MM/DD/YYYY
}
