using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexDatabase.Currencies
{
    public interface ICurrencyRates
    {
        double GetExchangeRate(string currencyName);
    }
}
