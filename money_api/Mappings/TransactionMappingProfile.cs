using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using money_api.DTOs.TransactionDtos;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;

namespace money_api.Mappings;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        // Mapping for Transaction to TransactionDto
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString()))
            .ForMember(dest => dest.IncomeCategory, opt => opt.MapFrom(src => src.IncomeCategory.HasValue ? src.IncomeCategory.ToString() : null))
            .ForMember(dest => dest.ExpenseCategory, opt => opt.MapFrom(src => src.ExpenseCategory.HasValue ? src.ExpenseCategory.ToString() : null));
    }
}
