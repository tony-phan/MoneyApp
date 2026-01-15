using AutoMapper;
using Microsoft.EntityFrameworkCore;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Exceptions;
using money_api.Mappings;
using money_api.Models;
using money_api.Services;
using Moq;

namespace money_api.Tests.Services;

public class TransactionHistoryServiceTests
{
    private readonly Mock<ITransactionHistoryRepository> _transactionHistoryRepoMock;
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly IMapper _mapper;
    private readonly TransactionHistoryService _service;
    private readonly Mock<ApplicationDbContext> _dbContextMock;

    public TransactionHistoryServiceTests()
    {
        _transactionHistoryRepoMock = new Mock<ITransactionHistoryRepository>();
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _accountRepoMock = new Mock<IAccountRepository>();
        _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

        var config = new MapperConfiguration(cfg => cfg.AddProfile<TransactionHistoryMappingProfile>());
        _mapper = config.CreateMapper();

        _service = new TransactionHistoryService(
            _dbContextMock.Object,
            _accountRepoMock.Object,
            _transactionHistoryRepoMock.Object,
            _transactionRepoMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task GetById_ValidId_ReturnsCorrectTransactionHistory()
    {
        // Arrange
        var appUser = new AppUser
        {
            UserName = "username",
            Email = "email@yahoo.com"
        };

        var transactionHistory = new TransactionHistory
        {
            Id = 1,
            UserId = "abc123",
            Month = 5,
            Year = 2025,
            TotalIncome = 1000,
            TotalExpenses = 300,
            User = appUser
        };

        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(1))
            .ReturnsAsync(transactionHistory);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("abc123", result.UserId);
        Assert.Equal(5, result.Month);
        Assert.Equal(2025, result.Year);
        Assert.Equal(1000, result.TotalIncome);
        Assert.Equal(300, result.TotalExpenses);
    }

    [Fact]
    public async Task GetById_InvalidId_ThrowsNotFoundException()
    {
        int invalidId = -10;
        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(invalidId))
            .ReturnsAsync((TransactionHistory)null!);

        await Assert.ThrowsAsync<TransactionHistoryNotFoundException>(() => _service.GetById(invalidId));
    }

    [Fact]
    public async Task GetByUserId_ValidUserId_ReturnsAllUserHistories()
    {
        // Arrange
        var userId = "a08ddd8f-6a9c-4498-a31b-6e3b27fb84ed";
        var appUser = new AppUser
        {
            UserName = "joe_slow",
            Email = "joe_slow@yahoo.com",
            Id = userId
        };

        var transactionHistories = new List<TransactionHistory>
        {
            new TransactionHistory
            {
                Id = 1,
                UserId = userId,
                Month = 5,
                Year = 2025,
                TotalIncome = 1000,
                TotalExpenses = 300,
                User = appUser
            },
            new TransactionHistory
            {
                Id = 3,
                UserId = userId,
                Month = 5,
                Year = 2024,
                TotalIncome = 250,
                TotalExpenses = 300,
                User = appUser
            }
        };

        _transactionHistoryRepoMock
            .Setup(repo => repo.GetAllByUserId(userId))
            .ReturnsAsync(transactionHistories);

        // Act
        var result = await _service.GetByUserId(userId);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);

        // Verify first history
        Assert.Equal(1, resultList[0].Id);
        Assert.Equal(5, resultList[0].Month);
        Assert.Equal(2025, resultList[0].Year);
        Assert.Equal(1000, resultList[0].TotalIncome);
        Assert.Equal(300, resultList[0].TotalExpenses);
        Assert.Equal(700, resultList[0].NetBalance); // Calculated: 1000 - 300

        // Verify second history
        Assert.Equal(3, resultList[1].Id);
        Assert.Equal(5, resultList[1].Month);
        Assert.Equal(2024, resultList[1].Year);
        Assert.Equal(250, resultList[1].TotalIncome);
        Assert.Equal(300, resultList[1].TotalExpenses);
        Assert.Equal(-50, resultList[1].NetBalance); // Calculated: 250 - 300
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsTrue()
    {
        // Arrange
        var appUser = new AppUser
        {
            UserName = "joe_slow",
            Email = "joe_slow@yahoo.com",
            Id = "a08ddd8f-6a9c-4498-a31b-6e3b27fb84ed"
        };

        var transactionHistory = new TransactionHistory
        {
            Id = 15,
            UserId = appUser.Id,
            Month = 5,
            Year = 2025,
            TotalIncome = 1000,
            TotalExpenses = 300,
            User = appUser,
        };

        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(15))
            .ReturnsAsync(transactionHistory);

        _dbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.Delete(15);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteTransactionHistory_InvalidId_ReturnsException()
    {
        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(-10))
            .ReturnsAsync((TransactionHistory?)null);

        await Assert.ThrowsAsync<TransactionHistoryNotFoundException>(() => _service.Delete(-10));
    }
}
