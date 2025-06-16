using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace money_api.Exceptions;

public class AccountNotFoundException : Exception
{
    public string FieldName { get; set; }
    public string FieldValue { get; set; }
    public AccountNotFoundException(string fieldName, string fieldValue) : base($"Account with {fieldName} '{fieldValue}' does not exist.")
    {
        FieldName = fieldName;
        FieldValue = fieldValue;
    }
}
