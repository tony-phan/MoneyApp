using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string id) : base($"Account with id {id} does not exist.") { }
}
