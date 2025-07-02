using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;

namespace money_api.DTOs.AccountDtos;

public class AccountResponseDto
{
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public ICollection<TransactionHistoryDto> TransactionHistories { get; set; } = new List<TransactionHistoryDto>();
}
