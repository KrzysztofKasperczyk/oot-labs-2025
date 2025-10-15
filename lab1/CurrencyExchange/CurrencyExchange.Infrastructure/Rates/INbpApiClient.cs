using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace CurrencyExchange.Infrastructure.Rates;

public interface INbpApiClient
{
    string GetLatestTableA(); 
}

public sealed class NbpApiClient : INbpApiClient
{
    private static readonly HttpClient _http = new HttpClient();
    private readonly string _tableAUrl;

    public NbpApiClient(string tableAUrl)
    {
        _tableAUrl = tableAUrl ?? throw new ArgumentNullException(nameof(tableAUrl));
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("CurrencyExchangeApp/1.0");
    }

    public string GetLatestTableA()
    {
        var resp = _http.GetAsync(_tableAUrl).GetAwaiter().GetResult();
        resp.EnsureSuccessStatusCode();
        return resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    }
}
