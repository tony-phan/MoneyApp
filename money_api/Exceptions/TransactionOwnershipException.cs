using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class TransactionOwnershipException : Exception
{
    public TransactionOwnershipException() : base("You do not have permission to access or modify this transaction.") { }
}
