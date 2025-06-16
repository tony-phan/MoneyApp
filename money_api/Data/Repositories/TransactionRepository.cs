using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using money_api.DTOs.TransactionDtos;
using money_api.Models;

namespace money_api.Data.Repositories;


public interface ITransactionRepository
{
    Task<Transaction> Create(Transaction transaction);
    Task<Transaction?> GetById(int id);
    Task<IEnumerable<Transaction>> GetAllByTransactionHistoryId(int transactionHistoryId);
    Task<Transaction?> Update(Transaction transaction);
    void Delete(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAllByUserId(string userId);
    void DeleteRange(ICollection<Transaction> transactions);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Transaction> Create(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
        return transaction;
    }

    public void Delete(Transaction transaction)
    {
        _dbContext.Transactions.Remove(transaction);
    }

    public void DeleteRange(ICollection<Transaction> transactions)
    {
        _dbContext.Transactions.RemoveRange(transactions);
    }

    public async Task<IEnumerable<Transaction>> GetAllByTransactionHistoryId(int transactionHistoryId)
    {
        return await _dbContext.Transactions
            .Where(x => x.TransactionHistoryId == transactionHistoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllByUserId(string userId)
    {
        return await _dbContext.Transactions
            .Where(t => t.TransactionHistory.UserId == userId)
            .ToListAsync();
    }

    public async Task<Transaction?> GetById(int id)
    {
        return await _dbContext.Transactions
            .Include(t => t.TransactionHistory)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Transaction?> Update(Transaction transaction)
    {
        var existingTransaction = await _dbContext.Transactions.FindAsync(transaction.Id);
        if (existingTransaction == null) return null;

        _dbContext.Entry(existingTransaction).CurrentValues.SetValues(transaction);
        return existingTransaction;
    }
}
