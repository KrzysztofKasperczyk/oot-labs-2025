using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CurrencyExchange.Domain.ValueObjects;

namespace CurrencyExchange.Domain.Rates;

public interface IExchangeRateProvider
{
    DateTime LastUpdatedUtc { get; }
    decimal GetRate(CurrencyCode code);                     // PLN per 1 unit (PLN => 1.0)
    IReadOnlyDictionary<CurrencyCode, decimal> GetAll();    // whole table (to PLN)
}

