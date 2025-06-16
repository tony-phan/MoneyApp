using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace money_api.DTOs.TransactionDtos;

public class TransactionCreateDto
{
    public int TransactionHistoryId { get; set; }
    public decimal Amount { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required string TransactionType { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public string? IncomeCategory { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public string? ExpenseCategory { get; set; }
    public string Description { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public required DateTime Date { get; set; }
}
