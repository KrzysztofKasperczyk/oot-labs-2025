using Microsoft.AspNetCore.Mvc;
using CurrencyExchange.WebMvc.Models;
using CurrencyExchange.Domain.Services;
using CurrencyExchange.Domain.Rates;
using CurrencyExchange.Domain.ValueObjects;
using System.Globalization;

namespace CurrencyExchange.WebMvc.Controllers;

public class CurrencyController : Controller
{
    private readonly CurrencyConverter _converter;
    private readonly IExchangeRateProvider _provider;

    public CurrencyController(CurrencyConverter converter, IExchangeRateProvider provider)
    {
        _converter = converter;
        _provider = provider;
    }

    [HttpGet]
    public IActionResult Index() => View(new ConvertViewModel());

    [HttpPost]
    public IActionResult Convert(ConvertViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("Index", vm);
        try
        {
            // walidacja prosta
            if (string.IsNullOrWhiteSpace(vm.SourceCode) || vm.SourceCode.Trim().Length != 3 ||
                string.IsNullOrWhiteSpace(vm.TargetCode) || vm.TargetCode.Trim().Length != 3)
                throw new Exception("Currency codes must be 3 letters (e.g., EUR).");

            var norm = (vm.Amount ?? "").Trim().Replace(',', '.');
            if (!decimal.TryParse(norm, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var amount) || amount < 0)
                throw new Exception("Amount must be a non-negative number (max 2 decimals).");

            var src = new CurrencyCode(vm.SourceCode!);
            var dst = new CurrencyCode(vm.TargetCode!);
            var money = new Money(amount, src);
            var result = _converter.Convert(money, dst);

            
            vm.Amount = amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

            vm.Result = $"{result.Amount:N2} {dst.Value}";
            vm.LastUpdated = $"Last table: {_provider.LastUpdatedUtc:yyyy-MM-dd HH:mm} UTC";
        }
        catch (Exception ex)
        {
            vm.Error = ex.Message;
        }
        return View("Index", vm);
    }
}
