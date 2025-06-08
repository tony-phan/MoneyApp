
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using money_api.Models;

namespace money_api.Data;

public class ApplicationDbContext(DbContextOptions dbContextOptions) : IdentityDbContext<AppUser>(dbContextOptions)
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<TransactionHistory> TransactionHistories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18, 2)");

        builder.Entity<Transaction>()
            .Property(t => t.TransactionType)
            .HasConversion<string>();

        builder.Entity<Transaction>()
            .Property(t => t.IncomeCategory)
            .HasConversion<string?>();

        builder.Entity<Transaction>()
            .Property(t => t.ExpenseCategory)
            .HasConversion<string?>();

        builder.Entity<TransactionHistory>()
            .Property(t => t.TotalExpenses)
            .HasColumnType("decimal(18, 2)");

        builder.Entity<TransactionHistory>()
            .Property(t => t.TotalIncome)
            .HasColumnType("decimal(18, 2)");

        builder.Entity<TransactionHistory>()
            .HasIndex(t => new { t.UserId, t.Month, t.Year })
            .IsUnique();
    }
}
