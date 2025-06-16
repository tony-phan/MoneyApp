using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;

namespace money_api.Mappings;

public class TransactionHistoryMappingProfile : Profile
{
    public TransactionHistoryMappingProfile()
    {
        // Mapping for TransactionHistory to TransactionHistoryDto
        CreateMap<TransactionHistory, TransactionHistoryDto>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));
    }
}
