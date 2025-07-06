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

    public TransactionHistoryServiceTests()
    {
        _transactionHistoryRepoMock = new Mock<ITransactionHistoryRepository>();
        _transactionRepoMock = new Mock<ITransactionRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<TransactionHistoryMappingProfile>());
        _mapper = config.CreateMapper();

        _accountRepoMock = new Mock<IAccountRepository>();

        _service = new TransactionHistoryService(
            new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>()).Object,
            _accountRepoMock.Object,
            _transactionHistoryRepoMock.Object,
            _transactionRepoMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task GetTransactionHistoryById_ValidId_ReturnsDto()
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
    public async Task GetTransactionHistoryById_InvalidId_ReturnsException()
    {
        int invalidId = -10;
        _transactionHistoryRepoMock
            .Setup(repo => repo.GetById(invalidId))
            .ReturnsAsync((TransactionHistory)null!);

        await Assert.ThrowsAsync<TransactionHistoryNotFoundException>(() => _service.GetById(invalidId));
    }

    [Fact]
    public async Task GetTransactionHistoryByUserId_ValidUserId_ReturnsDtos()
    {
        var appUser1 = new AppUser
        {
            UserName = "joe_slow",
            Email = "joe_slow@yahoo.com",
            Id = "a08ddd8f-6a9c-4498-a31b-6e3b27fb84ed"
        };

        var appUser2 = new AppUser
        {
            UserName = "t0n3yp",
            Email = "t0n3yp@gmail.com",
            Id = "bad19994-3554-4012-8e4e-a3d3983c9b8c"
        };

        var transactionHistory1 = new TransactionHistory
        {
            Id = 1,
            UserId = appUser1.Id,
            Month = 5,
            Year = 2025,
            TotalIncome = 1000,
            TotalExpenses = 300,
            User = appUser1
        };

        var transactionHistory2 = new TransactionHistory
        {
            Id = 2,
            UserId = appUser2.Id,
            Month = 1,
            Year = 2025,
            TotalIncome = 300,
            TotalExpenses = 300,
            User = appUser2
        };

        var transactionHistory3 = new TransactionHistory
        {
            Id = 3,
            UserId = appUser1.Id,
            Month = 5,
            Year = 2024,
            TotalIncome = 250,
            TotalExpenses = 300,
            User = appUser1
        };

        var appUser1Histories = new List<TransactionHistory> { transactionHistory1, transactionHistory3 };
        var appUser2Histories = new List<TransactionHistory> { transactionHistory2 };

        var expectedResult1 = new List<TransactionHistoryResponseDto>
        {
            new TransactionHistoryResponseDto {
                Id = 1,
                Month = 5,
                Year = 2025,
                TotalIncome = 1000,
                TotalExpenses = 300,
            },
            new TransactionHistoryResponseDto {
                Id = 3,
                Month = 5,
                Year = 2024,
                TotalIncome = 250,
                TotalExpenses = 300,
            }
        };
        var expectedResult2 = new List<TransactionHistoryResponseDto>
        {
            new TransactionHistoryResponseDto {
                Id = 2,
                Month = 1,
                Year = 2025,
                TotalIncome = 300,
                TotalExpenses = 300,
            }
        };

        _transactionHistoryRepoMock
            .Setup(repo => repo.GetAllByUserId(appUser1.Id))
            .ReturnsAsync(appUser1Histories);

        _transactionHistoryRepoMock
            .Setup(repo => repo.GetAllByUserId(appUser2.Id))
            .ReturnsAsync(appUser2Histories);

        var result1 = (await _service.GetByUserId(appUser1.Id)).ToList();
        var result2 = (await _service.GetByUserId(appUser2.Id)).ToList();

        // Assert
        Assert.NotNull(result1);
        Assert.Equal(expectedResult1.Count, result1.Count);

        for (int i = 0; i < expectedResult1.Count; i++)
        {
            Assert.Equal(expectedResult1[i].Id, result1[i].Id);
            Assert.Equal(expectedResult1[i].Month, result1[i].Month);
            Assert.Equal(expectedResult1[i].Year, result1[i].Year);
            Assert.Equal(expectedResult1[i].TotalIncome, result1[i].TotalIncome);
            Assert.Equal(expectedResult1[i].TotalExpenses, result1[i].TotalExpenses);
            Assert.Equal(expectedResult1[i].NetBalance, result1[i].NetBalance);
        }

        Assert.NotNull(result2);
        Assert.Equal(expectedResult2.Count, result2.Count);

        for (int i = 0; i < expectedResult2.Count; i++)
        {
            Assert.Equal(expectedResult2[i].Id, result2[i].Id);
            Assert.Equal(expectedResult2[i].Month, result2[i].Month);
            Assert.Equal(expectedResult2[i].Year, result2[i].Year);
            Assert.Equal(expectedResult2[i].TotalIncome, result2[i].TotalIncome);
            Assert.Equal(expectedResult2[i].TotalExpenses, result2[i].TotalExpenses);
            Assert.Equal(expectedResult2[i].NetBalance, result2[i].NetBalance);
        }
    }
}
