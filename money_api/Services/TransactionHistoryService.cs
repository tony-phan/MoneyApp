using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Exceptions;
using money_api.Models;

namespace money_api.Services;

public interface ITransactionHistoryService
{
    Task<TransactionHistoryDto> Create(TransactionHistoryCreateDto transactionHistoryCreateDto);
    Task<TransactionHistoryDto> GetById(int id);
    Task<IEnumerable<TransactionHistoryDto>> GetAll();
    Task<bool> Delete(int id);
}

public class TransactionHistoryService : ITransactionHistoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    public TransactionHistoryService(ApplicationDbContext dbContext, IAccountRepository accountRepository, ITransactionHistoryRepository transactionHistoryRepository, ITransactionRepository transactionRepository, IMapper mapper)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<TransactionHistoryDto> Create(TransactionHistoryCreateDto transactionHistoryCreateDto)
    {
        var exists = await _transactionHistoryRepository.ExistsByUserIdMonthYear(transactionHistoryCreateDto.UserId, transactionHistoryCreateDto.Month, transactionHistoryCreateDto.Year);

        if (exists)
            throw new DuplicateTransactionHistoryException();

        var user = await _accountRepository.GetById(transactionHistoryCreateDto.UserId);
        if (user == null)
            throw new AccountNotFoundException(transactionHistoryCreateDto.UserId);

        var transactionHistoryEntity = new TransactionHistory
        {
            UserId = transactionHistoryCreateDto.UserId,
            Month = transactionHistoryCreateDto.Month,
            Year = transactionHistoryCreateDto.Year,
            TotalIncome = 0,
            TotalExpenses = 0,
            User = user,
            Transactions = new List<Transaction>()
        };

        var tH = await _transactionHistoryRepository.Create(transactionHistoryEntity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TransactionHistoryDto>(tH);
    }

    public async Task<bool> Delete(int id)
    {
        var th = await _transactionHistoryRepository.GetById(id);

        if (th == null)
            throw new TransactionHistoryNotFoundException(id);

        if (th.Transactions != null && th.Transactions.Any())
        {
            _transactionRepository.DeleteRange(th.Transactions);
        }

        _transactionHistoryRepository.Delete(th);
        var changes = await _dbContext.SaveChangesAsync();
        return changes > 0;

    }

    public async Task<IEnumerable<TransactionHistoryDto>> GetAll()
    {
        var transactionHistories = await _transactionHistoryRepository.GetAll();
        return transactionHistories.Select(tH => _mapper.Map<TransactionHistoryDto>(tH));
    }

    public async Task<TransactionHistoryDto> GetById(int id)
    {
        var tH = await _transactionHistoryRepository.GetById(id);

        if (tH == null)
            throw new TransactionHistoryNotFoundException(id);

        return _mapper.Map<TransactionHistoryDto>(tH);
    }
}
