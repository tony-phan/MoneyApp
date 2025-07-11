using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace money_api.Models;

public class AppUser : IdentityUser
{
    // Navigation Property
    public IEnumerable<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();

    public override string ToString()
    {
        return $"ID: {this.Id}, Username: {this.UserName}, Email: {this.Email}";
    }
}