using System;
using System.Globalization;
using System.Linq;

namespace KabyliaTaste.Services
{
    public static class CurrencyFormatting
    {
        public static string GetCurrencySymbol(string? currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return "";

            if (string.Equals(currencyCode, "DZD", StringComparison.OrdinalIgnoreCase))
                return "D.A";

            try
            {
                var region = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Select(c => new RegionInfo(c.Name))
                    .FirstOrDefault(r => string.Equals(r.ISOCurrencySymbol, currencyCode, StringComparison.OrdinalIgnoreCase));

                return region?.CurrencySymbol ?? currencyCode.ToUpperInvariant();
            }
            catch
            {
                return currencyCode.ToUpperInvariant();
            }
        }

        public static string FormatAmount(decimal amount, string? currencyCode)
        {
            var symbol = GetCurrencySymbol(currencyCode);
            return string.IsNullOrWhiteSpace(symbol)
                ? amount.ToString("F2", CultureInfo.InvariantCulture)
                : $"{amount.ToString("F2", CultureInfo.InvariantCulture)} {symbol}";
        }

        public static string FormatQuantity(decimal quantity)
        {
            return quantity.ToString("0.#", CultureInfo.InvariantCulture);
        }
    }
}
