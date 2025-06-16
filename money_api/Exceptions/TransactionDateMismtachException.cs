using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class TransactionDateMismtachException : Exception
{
    public TransactionDateMismtachException() : base("A transactions month and year must match the month and year of it's associated TransactionHistory ") { }
}
