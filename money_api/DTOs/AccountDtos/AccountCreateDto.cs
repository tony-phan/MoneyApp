using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.DTOs.AccountDtos;

public class AccountCreateDto
{
    public required string UserName { get; set; }
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public required string Email { get; set; }
    public required string Password { get; set; }
}
