using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using money_api.DTOs.TransactionDtos;
using money_api.Models.Enums;
using money_api.Services;

namespace money_api.Controllers;

public class TransactionController : BaseApiController
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<TransactionDto>> CreateTransaction(TransactionCreateDto transactionCreateDto)
    {
        var result = await _transactionService.Create(transactionCreateDto);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{transactionHistoryId}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByHistoryId(int transactionHistoryId)
    {
        var result = await _transactionService.GetByTransactionHistoryId(transactionHistoryId);
        if (result == null || result.Count() == 0)
            return NotFound(new { message = "No transactions found" });

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTransactionById(int id)
    {
        var result = await _transactionService.Delete(id);
        if (!result)
            return NotFound($"Delete transaction with id {id} failed.");

        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<TransactionDto>> UpdateTransaction(int id, TransactionUpdateDto transactionUpdateDto)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID" });

        var result = await _transactionService.Update(id, transactionUpdateDto);
        return Ok(result);
    }
}