using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using money_api.DTOs.AccountDtos;
using money_api.DTOs.TransactionDtos;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;
using money_api.Services;

namespace money_api.Controllers;

public class TransactionHistoryController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITransactionHistoryService _transactionHistoryService;

    public TransactionHistoryController(UserManager<AppUser> userManager, ITransactionHistoryService transactionHistoryService)
    {
        _userManager = userManager;
        _transactionHistoryService = transactionHistoryService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<TransactionHistoryDto>> CreateTransactionHistory(TransactionHistoryCreateDto transactionHistoryCreateDto)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID claim not found in the token.");
        else if (!ModelState.IsValid)
            return BadRequest(transactionHistoryCreateDto);

        var newTransactionHistory = await _transactionHistoryService.Create(transactionHistoryCreateDto);
        return Ok(newTransactionHistory);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionHistoryDto>> GetTransactionHistoryById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "ID must be a positive number" });
        }

        var transactionHistory = await _transactionHistoryService.GetById(id);
        return Ok(transactionHistory);
    }

    [Authorize]
    [HttpGet("histories")]
    public async Task<ActionResult<IEnumerable<TransactionHistoryDto>>> GetAllTransactionHistories()
    {
        var transactionHistories = await _transactionHistoryService.GetAll();

        return Ok(transactionHistories);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTransactionHistoryById(int id)
    {
        var deleted = await _transactionHistoryService.Delete(id);
        if (!deleted)
        {
            return NotFound($"TransactionHistory with id {id} not found.");
        }
        return NoContent();
    }
}
