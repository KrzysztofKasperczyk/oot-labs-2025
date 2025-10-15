using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CurrencyExchange.Domain.ValueObjects;

namespace CurrencyExchange.Infrastructure.Cache;

public sealed class RateCache
{
    private static readonly Lazy<RateCache> _inst = new(() => new RateCache());
    public static RateCache Instance() => _inst.Value;

    private Dictionary<CurrencyCode, decimal>? _rates;
    private DateTime _lastUpdatedUtc;

    private RateCache() { }

    public bool TryGetAll(out IReadOnlyDictionary<CurrencyCode, decimal> rates)
    {
        if (_rates is null) { rates = default!; return false; }
        rates = new Dictionary<CurrencyCode, decimal>(_rates);
        return true;
    }

    public void SetAll(IDictionary<CurrencyCode, decimal> rates, DateTime lastUpdatedUtc)
    {
        _rates = new Dictionary<CurrencyCode, decimal>(rates);
        _lastUpdatedUtc = lastUpdatedUtc;
    }

    public DateTime LastUpdatedUtc => _lastUpdatedUtc;
}
