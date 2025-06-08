using System.ComponentModel.DataAnnotations;

namespace money_api.DTOs.TransactionHistoryDtos;

public class TransactionHistoryCreateDto
{
    public required string UserId { get; set; }
    [Range(1, 12)]
    public int Month { get; set; }
    public int Year { get; set; }
}
