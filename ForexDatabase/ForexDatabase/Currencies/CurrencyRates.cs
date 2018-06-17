using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForexDatabase.Currencies
{
    public class CurrencyRates
    {
        private Dictionary<int, double> exchangeRates;

        public CurrencyRates()
        {
            exchangeRates = new Dictionary<int, double>();
            exchangeRates.Add(1, 1.0);
            exchangeRates.Add(2, 4.23);
        }

        public double GetExchangeRate(int id)
        {
            return exchangeRates[id];
        }

    }
}