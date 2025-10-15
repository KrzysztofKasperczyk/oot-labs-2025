using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CurrencyExchange.Domain.Rates;
using CurrencyExchange.Domain.ValueObjects;
using CurrencyExchange.Infrastructure.Cache;

namespace CurrencyExchange.Infrastructure.Rates;

public sealed class NbpExchangeRateProvider : IExchangeRateProvider
{
    private readonly INbpApiClient _client;
    private readonly INbpXmlParser _parser;
    private readonly RateCache _cache;

    public NbpExchangeRateProvider(INbpApiClient client, INbpXmlParser parser, RateCache cache)
    {
        _client = client;
        _parser = parser;
        _cache = cache;
    }

    public DateTime LastUpdatedUtc => _cache.LastUpdatedUtc;

    public decimal GetRate(CurrencyCode code)
    {
        if (code.Value == "PLN") return 1.0m;

        var table = EnsureTable();
        var kv = table.FirstOrDefault(k => k.Key.Value == code.Value);
        if (kv.Key is null) throw new InvalidOperationException("Unsupported currency.");
        return kv.Value;
    }

    public IReadOnlyDictionary<CurrencyCode, decimal> GetAll() => EnsureTable();

    private IReadOnlyDictionary<CurrencyCode, decimal> EnsureTable()
    {
        if (_cache.TryGetAll(out var cached))
            return cached;

        var xml = _client.GetLatestTableA();
        var rates = _parser.ParseRates(xml);
        var ts = _parser.ParseLastUpdated(xml);
        _cache.SetAll(rates, ts);
        _ = _cache.TryGetAll(out var now);
        return now;
    }
}
