using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;

namespace money_api.Data.Repositories;

public interface ITransactionHistoryRepository
{
    Task<TransactionHistory> Create(TransactionHistory newTransactionHistory);
    Task<TransactionHistory?> GetById(int id);
    Task<IEnumerable<TransactionHistory>> GetAllByUserId(string userId);
    Task<TransactionHistory?> Update(TransactionHistory updatedTransactionHistory);
    void Delete(TransactionHistory transactionHistory);
    Task<IEnumerable<TransactionHistory>> GetAll();
    Task<bool> ExistsByUserIdMonthYear(string userId, int month, int year);
    void DeleteRange(IEnumerable<TransactionHistory> transactionHistories);
}

public class TransactionHistoryRepository : ITransactionHistoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionHistoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TransactionHistory> Create(TransactionHistory newTransactionHistory)
    {
        await _dbContext.TransactionHistories.AddAsync(newTransactionHistory);
        return newTransactionHistory;
    }

    public void Delete(TransactionHistory transactionHistory)
    {
        _dbContext.TransactionHistories.Remove(transactionHistory);
    }

    public void DeleteRange(IEnumerable<TransactionHistory> transactionHistories)
    {
        _dbContext.TransactionHistories.RemoveRange(transactionHistories);
    }

    public async Task<bool> ExistsByUserIdMonthYear(string userId, int month, int year)
    {
        var exists = await _dbContext.TransactionHistories.AnyAsync(th =>
            th.UserId == userId &&
            th.Month == month &&
            th.Year == year);
        return exists;
    }

    public async Task<IEnumerable<TransactionHistory>> GetAll()
    {
        return await _dbContext.TransactionHistories
            .Include(th => th.Transactions)
            .ToListAsync();
    }

    public async Task<IEnumerable<TransactionHistory>> GetAllByUserId(string userId)
    {
        return await _dbContext.TransactionHistories
            .Where(tH => tH.UserId == userId)
            .Include(tH => tH.Transactions)
            .ToListAsync();
    }

    public async Task<TransactionHistory?> GetById(int id)
    {
        return await _dbContext.TransactionHistories
            .Include(th => th.User)
            .Include(th => th.Transactions)
            .FirstOrDefaultAsync(th => th.Id == id);
    }

    public async Task<TransactionHistory?> Update(TransactionHistory updatedTransactionHistory)
    {
        var existingTransactionHistory = await _dbContext.TransactionHistories.FindAsync(updatedTransactionHistory.Id);
        if (existingTransactionHistory == null) return null;

        _dbContext.Entry(existingTransactionHistory).CurrentValues.SetValues(updatedTransactionHistory);
        return existingTransactionHistory;
    }
}
