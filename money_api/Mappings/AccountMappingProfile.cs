using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using money_api.DTOs.AccountDtos;
using money_api.DTOs.TransactionHistoryDtos;
using money_api.Models;

namespace money_api.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        // Mapping for AppUser to AccountResponseDto
        CreateMap<AppUser, AccountResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TransactionHistories, opt => opt.MapFrom(src => src.TransactionHistories));
    }
}
