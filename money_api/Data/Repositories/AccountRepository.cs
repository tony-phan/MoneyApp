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
    Task<IdentityResult> Create(AppUser user, string password);
    Task<AppUser?> GetById(string id);
    Task<IEnumerable<AppUser>> GetAll();
    Task<AppUser?> Update(string id, AppUser updatedUser);
    Task<IdentityResult> Delete(AppUser user);
}

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Create(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> Delete(AppUser user)
    {
        return await _userManager.DeleteAsync(user);
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
