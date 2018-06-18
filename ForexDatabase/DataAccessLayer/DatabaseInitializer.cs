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
            var users = new List<User>
            {
                new User { Username = "Bzdeco", Password = "abcdef" }
            };
            users.ForEach(user => context.Users.Add(user));
            context.SaveChanges();

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

            var wallets = new List<Wallet>
            {
                new Wallet { UserId = users[0].Id, CurrencyId = currencies[0].Id, Amount = 10000.0 }
            };
            wallets.ForEach(wallet => context.Wallets.Add(wallet));
            context.SaveChanges();
        }
    }
}