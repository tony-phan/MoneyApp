using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using money_api.DTOs.AccountDtos;
using money_api.Models;

namespace money_api.Data.Repositories;

public interface IAccountRepository
{
    Task<AppUser> Create(AppUser user, string password);
    Task<AppUser?> GetById(string id);
    Task<IEnumerable<AppUser>> GetAll();
    Task<AppUser?> Update(string id, AppUser updatedUser);
    Task<bool> Delete(string id);
}

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUser> Create(AppUser user, string password)
    {
        await _userManager.CreateAsync(user, password);
        return user;
    }

    public async Task<bool> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<IEnumerable<AppUser>> GetAll()
    {
        return await _userManager.Users.Include(u => u.TransactionHistories).ToListAsync();
    }

    public async Task<AppUser?> GetById(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<AppUser?> Update(string id, AppUser updatedUser)
    {
        var user = await _userManager.FindByIdAsync(updatedUser.Id);
        if (user == null) return null;

        // Update properties (manually copy changes you want to persist)
        user.Email = updatedUser.Email;
        user.UserName = updatedUser.UserName;

        await _userManager.UpdateAsync(user);
        return user;
    }
}
