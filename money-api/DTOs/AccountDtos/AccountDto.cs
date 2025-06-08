using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.DTOs.AccountDtos;

public class AccountDto
{
    public required string UserId { get; set; }
    public required string Username { get; set; }
    public required string Token { get; set; }
}
