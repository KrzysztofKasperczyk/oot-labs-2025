using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Domain.ValueObjects;

public sealed class CurrencyCode
{
    public string Value { get; }

    public CurrencyCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code is required.");

        var v = code.Trim().ToUpperInvariant();
        if (v.Length != 3 || !v.All(char.IsLetter))
            throw new ArgumentException("Currency code must be 3 letters, e.g., EUR.");

        Value = v;
    }

    public override string ToString() => Value;
}

