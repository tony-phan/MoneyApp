using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using money_api.Data;
using money_api.DTOs;
using money_api.DTOs.AccountDtos;
using money_api.Models;
using money_api.Services;

namespace money_api.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, IAccountService accountService, ITokenService tokenService)
    {
        _userManager = userManager;
        _accountService = accountService;
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
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
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
        var result = await _accountService.Delete(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

