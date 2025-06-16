using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class DuplicateTransactionHistoryException : Exception
{
    public DuplicateTransactionHistoryException() : base("Transaction history already exists.") { }
}
