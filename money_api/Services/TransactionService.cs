using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.DTOs.TransactionDtos;
using money_api.Exceptions;
using money_api.Models;
using money_api.Models.Enums;

namespace money_api.Services;

public interface ITransactionService
{
    Task<TransactionDto> Create(TransactionCreateDto transactionCreateDto);
    Task<IEnumerable<TransactionDto>> GetByTransactionHistoryId(int transactionHistoryId);
    Task<IEnumerable<TransactionDto>> GetByUserId(string userId);
    Task<bool> Delete(int id);
}

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly IMapper _mapper;

    public TransactionService(ApplicationDbContext dbContext, ITransactionRepository transactionRepository, ITransactionHistoryRepository transactionHistoryRepository, IMapper mapper)
    {
        _dbContext = dbContext;
        _transactionRepository = transactionRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Create(TransactionCreateDto transactionCreateDto)
    {
        // NEED TO VALIDATE THAT THE USER CREATING THE TRANSACTiON is also associated with the transaction history

        var transactionHistory = await _transactionHistoryRepository.GetById(transactionCreateDto.TransactionHistoryId);
        if (transactionHistory == null)
            throw new TransactionHistoryNotFoundException(transactionCreateDto.TransactionHistoryId);

        if (transactionCreateDto.Date.Month != transactionHistory.Month || transactionCreateDto.Date.Year != transactionHistory.Year)
        {
            throw new TransactionDateMismtachException();
        }

        TransactionType transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), transactionCreateDto.TransactionType);
        IncomeCategory incomeCategory = string.IsNullOrEmpty(transactionCreateDto.IncomeCategory) ? IncomeCategory.None : (IncomeCategory)Enum.Parse(typeof(IncomeCategory), transactionCreateDto.IncomeCategory);
        ExpenseCategory expenseCategory = string.IsNullOrEmpty(transactionCreateDto.ExpenseCategory) ? ExpenseCategory.None : (ExpenseCategory)Enum.Parse(typeof(ExpenseCategory), transactionCreateDto.ExpenseCategory);

        var transactionEntity = new Transaction
        {
            TransactionHistoryId = transactionHistory.Id,
            Amount = transactionCreateDto.Amount,
            TransactionType = transactionType,
            IncomeCategory = incomeCategory,
            ExpenseCategory = expenseCategory,
            Description = transactionCreateDto.Description,
            Date = transactionCreateDto.Date,
            TransactionHistory = transactionHistory
        };

        var result = await _transactionRepository.Create(transactionEntity);

        transactionHistory.TotalIncome += transactionEntity.TransactionType == TransactionType.Income ? transactionEntity.Amount : 0;
        transactionHistory.TotalExpenses += transactionEntity.TransactionType == TransactionType.Expense ? transactionEntity.Amount : 0;

        _dbContext.TransactionHistories.Update(transactionHistory);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TransactionDto>(result);
    }

    public async Task<bool> Delete(int id)
    {
        var transaction = await _transactionRepository.GetById(id);
        if (transaction == null)
            throw new TransactionNotFoundException(id);

        var transactionHistory = transaction.TransactionHistory;

        if (transaction.TransactionType == TransactionType.Income)
        {
            transactionHistory.TotalIncome -= transaction.Amount;
        }
        else
        {
            transactionHistory.TotalExpenses -= transaction.Amount;
        }

        _transactionRepository.Delete(transaction);
        await _transactionHistoryRepository.Update(transactionHistory);

        var changes = await _dbContext.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable<TransactionDto>> GetByTransactionHistoryId(int transactionHistoryId)
    {
        var t = await _transactionRepository.GetAllByTransactionHistoryId(transactionHistoryId);
        return t.Select(transaction => _mapper.Map<TransactionDto>(transaction));
    }

    public async Task<IEnumerable<TransactionDto>> GetByUserId(string userId)
    {
        var t = await _transactionRepository.GetAllByUserId(userId);
        return t.Select(transaction => _mapper.Map<TransactionDto>(transaction));
    }
}
