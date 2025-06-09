using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace money_api.Exceptions;

public class AccountCreateException : Exception
{
    public IEnumerable<string> Errors { get; }
    public AccountCreateException(IdentityResult result) : base("Account creation failed")
    {
        this.Errors = result.Errors.Select(e => e.Description);
    }
}
