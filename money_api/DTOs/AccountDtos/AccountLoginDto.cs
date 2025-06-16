using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.DTOs.AccountDtos;

public class AccountLoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
