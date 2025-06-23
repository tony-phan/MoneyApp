using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.DTOs.TransactionDtos;

public class TransactionUpdateDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }
}
