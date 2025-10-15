using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyExchange.Domain.Rates;
using CurrencyExchange.Domain.ValueObjects;

namespace CurrencyExchange.Domain.Services;

public sealed class CurrencyConverter
{
    private readonly IExchangeRateProvider _provider;

    public CurrencyConverter(IExchangeRateProvider provider) => _provider = provider;

    public Money Convert(Money amount, CurrencyCode target)
    {
        if (amount.Currency.Value == target.Value) return amount;
        if (amount.Amount == 0) return new Money(0m, target);

        var rateSrc = _provider.GetRate(amount.Currency); // PLN per 1 SRC
        var rateDst = _provider.GetRate(target);          // PLN per 1 DST
        var result = amount.Amount * rateSrc / rateDst;
        return new Money(decimal.Round(result, 2), target);
    }
}
