using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.WebMvc.Models;

public class ConvertViewModel
{
    [Required]
    [RegularExpression(@"^(\d{1,10}([.,]\d{0,2})?|[.,]\d{1,2})$",
    ErrorMessage = "Do 10 cyfr, opcjonalny separator i max 2 cyfry po nim.")]
    public string? Amount { get; set; } = "100";

    [Required, StringLength(3, MinimumLength = 3)]
    [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "Code must be 3 letters.")]
    public string? SourceCode { get; set; } = "EUR";

    [Required, StringLength(3, MinimumLength = 3)]
    [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "Code must be 3 letters.")]
    public string? TargetCode { get; set; } = "USD";
    public string? Result { get; set; }
    public string? LastUpdated { get; set; }
    public string? Error { get; set; }
}
