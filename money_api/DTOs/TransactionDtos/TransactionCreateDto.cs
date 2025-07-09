using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace money_api.DTOs.TransactionDtos;

public class TransactionCreateDto
{
    public required string UserId { get; set; }
    public decimal Amount { get; set; }
    public required string TransactionType { get; set; }
    public string? IncomeCategory { get; set; }
    public string? ExpenseCategory { get; set; }
    public string Description { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public required DateTime Date { get; set; }
}
