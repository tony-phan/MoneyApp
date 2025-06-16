using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.DTOs;
using money_api.DTOs.AccountDtos;
using money_api.Exceptions;
using money_api.Models;

namespace money_api.Services;

public interface IAccountService
{
    Task<AppUser> Create(AccountCreateDto accountCreateDto);
    Task<IEnumerable<AccountResponseDto>> GetAll();
    Task<bool> DeleteById(string id);
    Task<AppUser?> GetByUsername(string username);
}

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public AccountService(ApplicationDbContext dbContext, IAccountRepository accountRepository, ITransactionHistoryRepository transactionHistoryRepository, ITransactionRepository transactionRepository, IMapper mapper)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<AppUser> Create(AccountCreateDto accountCreateDto)
    {
        var newUser = new AppUser
        {
            UserName = accountCreateDto.UserName,
            Email = accountCreateDto.Email
        };
        var result = await _accountRepository.Create(newUser, accountCreateDto.Password);
        if (!result.Succeeded)
            throw new AccountCreateException(result);
        return newUser;
    }

    public async Task<IEnumerable<AccountResponseDto>> GetAll()
    {
        var users = await _accountRepository.GetAll();
        return users.Select(_mapper.Map<AccountResponseDto>).ToList();
    }

    public async Task<bool> DeleteById(string id)
    {
        var user = await _accountRepository.GetById(id);
        if (user == null)
            throw new AccountNotFoundException("ID", id);

        var result = await _accountRepository.Delete(user);
        await _dbContext.SaveChangesAsync();
        return result.Succeeded;
    }

    public Task<AppUser?> GetByUsername(string username)
    {
        var result = _accountRepository.GetByUsername(username);
        if (result == null)
            throw new AccountNotFoundException("Username", username);
        return result;
    }
}
