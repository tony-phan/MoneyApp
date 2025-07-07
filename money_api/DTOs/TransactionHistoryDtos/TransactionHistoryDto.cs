using System.ComponentModel.DataAnnotations;
using money_api.DTOs.TransactionDtos;

namespace money_api.DTOs.TransactionHistoryDtos;

public class TransactionHistoryDto
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    [Range(1, 12)]
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetBalance => TotalIncome - TotalExpenses;
    public IEnumerable<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
}
