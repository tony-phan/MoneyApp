using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class TransactionHistoryNotFoundException : Exception
{
    public TransactionHistoryNotFoundException(int id) : base($"TransactionHistory with id={id} not found") { }
}
