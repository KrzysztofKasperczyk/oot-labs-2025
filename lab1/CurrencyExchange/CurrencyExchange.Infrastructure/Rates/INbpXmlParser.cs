using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Xml.Linq;
using CurrencyExchange.Domain.ValueObjects;

namespace CurrencyExchange.Infrastructure.Rates;

public interface INbpXmlParser
{
    IDictionary<CurrencyCode, decimal> ParseRates(string xml);
    DateTime ParseLastUpdated(string xml);
}

public sealed class NbpXmlParser : INbpXmlParser
{
    public IDictionary<CurrencyCode, decimal> ParseRates(string xml)
    {
        var doc = XDocument.Parse(xml);

        // Zbieramy wszystkie <pozycja>
        var dict = new Dictionary<CurrencyCode, decimal>();

        foreach (var pos in doc.Descendants("pozycja"))
        {
            var code = pos.Element("kod_waluty")?.Value?.Trim();
            var midText = pos.Element("kurs_sredni")?.Value?.Trim();
            var unitText = pos.Element("przelicznik")?.Value?.Trim();

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(midText))
                continue;

            // NBP używa przecinka, zamieniamy na kropkę do parsowania
            var midNorm = midText.Replace(' ', '\0').Replace(',', '.');
            if (!decimal.TryParse(midNorm, NumberStyles.Any, CultureInfo.InvariantCulture, out var mid))
                continue;

            var unit = 1m;
            if (!string.IsNullOrWhiteSpace(unitText))
            {
                var unitNorm = unitText.Replace(',', '.');
                if (decimal.TryParse(unitNorm, NumberStyles.Any, CultureInfo.InvariantCulture, out var u) && u > 0)
                    unit = u;
            }

            // Kurs w słowniku trzymamy jako PLN za 1 jednostkę waluty (dzielimy przez przelicznik)
            var perOne = mid / unit;

            dict[new CurrencyCode(code)] = perOne;
        }

        // PLN zawsze 1.0
        dict[new CurrencyCode("PLN")] = 1.0m;
        return dict;
    }

    public DateTime ParseLastUpdated(string xml)
    {
        var doc = XDocument.Parse(xml);
        var date = doc.Descendants("data_publikacji").FirstOrDefault()?.Value?.Trim();
        if (DateTime.TryParse(date, out var d))
            return DateTime.SpecifyKind(d, DateTimeKind.Utc); // NBP podaje datę bez czasu – traktujmy jako UTC
        return DateTime.UtcNow;
    }
}