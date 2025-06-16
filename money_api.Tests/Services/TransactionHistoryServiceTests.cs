using AutoMapper;
using Microsoft.EntityFrameworkCore;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Exceptions;
using money_api.Models;
using money_api.Services;
using Moq;

namespace money_api.Tests.Services;

public class TransactionHistoryServiceTests
{

    private readonly Mock<ITransactionHistoryRepository> _transactionHistoryRepoMock;
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TransactionHistoryService _service;

    public TransactionHistoryServiceTests()
    {
        _transactionHistoryRepoMock = new Mock<ITransactionHistoryRepository>();
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _mapperMock = new Mock<IMapper>();
        _accountRepoMock = new Mock<IAccountRepository>();

        _service = new TransactionHistoryService(
            new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>()).Object,
            _accountRepoMock.Object,
            _transactionHistoryRepoMock.Object,
            _transactionRepoMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async void GetTransactionHistoryById_ValidId_ReturnsDto()
    {
        var fakeAppUser = new AppUser
        {
            UserName = "username",
            Email = "email@yahoo.com"
        };

        _accountRepoMock
            .Setup(repo => repo.GetByUsername("username"))
            .ReturnsAsync(fakeAppUser);

        var fakeEntity = new TransactionHistory
        {
            Id = 1,
            UserId = "abc123",
            Month = 5,
            Year = 2025,
            TotalIncome = 1000,
            TotalExpenses = 300,
            User = fakeAppUser
        };

        var expectedResult = new TransactionHistoryDto
        {
            Id = 1,
            UserId = "abc123",
            Month = 5,
            Year = 2025,
            TotalIncome = 1000,
            TotalExpenses = 300,
        };

        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(1))
            .ReturnsAsync(fakeEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<TransactionHistoryDto>(fakeEntity))
            .Returns(expectedResult);

        var result = await _service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.Id, result.Id);
        Assert.Equal(expectedResult.UserId, result.UserId);
        Assert.Equal(expectedResult.Month, result.Month);
        Assert.Equal(expectedResult.Year, result.Year);
        Assert.Equal(expectedResult.TotalIncome, result.TotalIncome);
        Assert.Equal(expectedResult.TotalExpenses, result.TotalExpenses);
        Assert.Equal(expectedResult.TotalExpenses, result.TotalExpenses);
    }

    [Fact]
    public async void GetTransactionHistoryById_InvalidId_ReturnsException()
    {
        int invalidId = -10;
        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(invalidId))
            .ReturnsAsync((TransactionHistory)null!);

        await Assert.ThrowsAsync<TransactionHistoryNotFoundException>(() => _service.GetById(invalidId));
    }
}
