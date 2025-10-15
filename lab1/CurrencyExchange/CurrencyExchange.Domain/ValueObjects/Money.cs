using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public CurrencyCode Currency { get; }

    public Money(decimal amount, CurrencyCode currency)
    {
        if (currency is null) throw new ArgumentNullException(nameof(currency));
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be >= 0");
        Amount = amount;
        Currency = currency;
    }

    public override string ToString() => $"{Amount} {Currency.Value}";
}

