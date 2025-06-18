using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using money_api.Data;
using money_api.DTOs;
using money_api.DTOs.AccountDtos;
using money_api.DTOs.TransactionDtos;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;
using money_api.Services;

namespace money_api.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAccountService _accountService;
    private readonly ITransactionHistoryService _transactionHistoryService;
    private readonly ITransactionService _transactionService;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, IAccountService accountService, ITransactionHistoryService transactionHistoryService, ITransactionService transactionService, ITokenService tokenService)
    {
        _userManager = userManager;
        _accountService = accountService;
        _transactionHistoryService = transactionHistoryService;
        _transactionService = transactionService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AccountDto>> Register(AccountCreateDto accountCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _accountService.Create(accountCreateDto);
        var token = _tokenService.CreateToken(result);

        return Ok(new AccountDto { UserId = result.Id, Username = result.UserName, Token = token });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccountDto>> Login(AccountLoginDto loginDto)
    {
        var user = await _accountService.GetByUsername(loginDto.UserName);
        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return BadRequest("Invalid user credentials");
        }

        var token = _tokenService.CreateToken(user);

        return Ok(new AccountDto { UserId = user.Id, Username = user.UserName, Token = token });
    }

    [Authorize]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAllUsers()
    {
        var users = await _accountService.GetAll();

        return Ok(users);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(string id)
    {
        var result = await _accountService.DeleteById(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [Authorize]
    [HttpGet("{userId}/transactionHistories")]
    public async Task<ActionResult<IEnumerable<TransactionHistoryDto>>> GetTransactionHistoriesByUserId(string userId)
    {
        string regexPattern = "^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$";
        if (!Regex.IsMatch(userId, regexPattern, RegexOptions.IgnoreCase))
        {
            return BadRequest(new { message = $"Invalid UserId '{userId}'" });
        }

        var transactionHistories = await _transactionHistoryService.GetByUserId(userId);
        if (transactionHistories == null || transactionHistories.Count() == 0)
        {
            return NotFound(new { message = $"No transaction histories found for user ID '{userId}'" });
        }
        return Ok(transactionHistories);
    }

    [Authorize]
    [HttpGet("{userId}/transactions")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByUserId(string userId)
    {
        var response = await _transactionService.GetByUserId(userId);
        return Ok(response);
    }
}

