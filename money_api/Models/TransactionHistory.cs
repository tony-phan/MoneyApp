using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Models;

public class TransactionHistory
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetBalance => TotalIncome - TotalExpenses;

    // Navigation Properties
    public required AppUser User { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
}
