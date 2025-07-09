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
using System.Security.Claims;

namespace money_api.Services;

public interface ITransactionService
{
    Task<TransactionDto> Create(TransactionCreateDto transactionCreateDto);
    Task<IEnumerable<TransactionDto>> GetByTransactionHistoryId(int transactionHistoryId);
    Task<IEnumerable<TransactionDto>> GetByUserId(string userId);
    Task<bool> Delete(int id);
    Task<TransactionDto> Update(int id, TransactionUpdateDto transactionUpdateDto);
}

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionService(ApplicationDbContext dbContext, IAccountRepository accountRepository, ITransactionRepository transactionRepository, ITransactionHistoryRepository transactionHistoryRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TransactionDto> Create(TransactionCreateDto transactionCreateDto)
    {
        var user = await _accountRepository.GetById(transactionCreateDto.UserId);
        if (user == null)
            throw new AccountNotFoundException("ID", transactionCreateDto.UserId);

        var transactionHistory = await GetOrCreateTransactionHistory(user, transactionCreateDto.Date.Month, transactionCreateDto.Date.Year);

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

        await _transactionHistoryRepository.Update(transactionHistory);
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

    public async Task<TransactionDto> Update(int id, TransactionUpdateDto transactionUpdateDto)
    {
        var transaction = await _transactionRepository.GetById(id);
        if (transaction == null)
            throw new TransactionNotFoundException(id);

        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
        if (userId != transaction.TransactionHistory.UserId)
            throw new TransactionOwnershipException();

        var transactionHistory = transaction.TransactionHistory;

        if (transactionUpdateDto.Date.Month != transactionHistory.Month || transactionUpdateDto.Date.Year != transactionHistory.Year)
            throw new TransactionDateMismtachException();

        var originalAmount = transaction.Amount;

        transaction.Amount = transactionUpdateDto.Amount;
        transaction.Description = transactionUpdateDto.Description;
        transaction.Date = transactionUpdateDto.Date;

        var difference = transactionUpdateDto.Amount - originalAmount;

        if (difference != 0)
        {
            if (transaction.TransactionType == TransactionType.Income)
                transactionHistory.TotalIncome += difference;
            else if (transaction.TransactionType == TransactionType.Expense)
                transactionHistory.TotalExpenses += difference;
        }
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TransactionDto>(transaction);
    }

    private async Task<TransactionHistory> GetOrCreateTransactionHistory(AppUser user, int month, int year)
    {
        var history = await _transactionHistoryRepository.GetByUserIdMonthYear(user.Id, month, year);
        if (history != null)
            return history;

        var newHistory = new TransactionHistory
        {
            UserId = user.Id,
            Month = month,
            Year = year,
            TotalIncome = 0,
            TotalExpenses = 0,
            User = user
        };

        var created = await _transactionHistoryRepository.Create(newHistory);
        await _dbContext.SaveChangesAsync();
        return created;
    }
}
