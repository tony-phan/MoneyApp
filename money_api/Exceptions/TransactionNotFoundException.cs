using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class TransactionNotFoundException : Exception
{
    public TransactionNotFoundException(int id) : base($"Transaction with id {id} not found") { }
}
