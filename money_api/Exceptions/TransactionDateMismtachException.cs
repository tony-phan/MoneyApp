using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class TransactionDateMismtachException : Exception
{
    public TransactionDateMismtachException() : base("Transaction month and year must match the associated TransactionHistory month and year") { }
}
