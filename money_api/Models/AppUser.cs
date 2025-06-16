using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace money_api.Models;
public class AppUser : IdentityUser
{
    // Navigation Property
    public ICollection<TransactionHistory>? TransactionHistories { get; set; }

    public override string ToString()
    {
        return $"ID: {this.Id}, Username: {this.UserName}, Email: {this.Email}";
    }
}