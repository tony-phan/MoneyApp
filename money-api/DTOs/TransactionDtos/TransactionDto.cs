using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.DTOs.TransactionDtos;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string? IncomeCategory { get; set; }
    public string? ExpenseCategory { get; set; }
    public string Description { get; set; } = string.Empty;
    [RegularExpression(@"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\d{4}$", ErrorMessage = "Date must be in MM/DD/YYYY format.")]
    public required string Date { get; set; }
}
