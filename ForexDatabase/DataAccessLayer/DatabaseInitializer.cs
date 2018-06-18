using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ForexDatabase.DAL
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            var currencies = new List<Currency>
            {
                new Currency { Name = "USD" },
                new Currency { Name = "BTC" },
                new Currency { Name = "ETH" },
                new Currency { Name = "LTC" },
                new Currency { Name = "XRP" },
                new Currency { Name = "DSH" },
                new Currency { Name = "BCH" }

            };
            currencies.ForEach(currency => context.Currencies.Add(currency));
            context.SaveChanges();
        }
    }
}