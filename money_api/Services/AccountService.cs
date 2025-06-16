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
    Task<bool> Delete(string id);
}

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
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

    public async Task<bool> Delete(string id)
    {
        var user = await _accountRepository.GetById(id);
        if (user == null)
            throw new AccountNotFoundException(id);

        var result = await _accountRepository.Delete(user);
        return result.Succeeded;
    }
}
