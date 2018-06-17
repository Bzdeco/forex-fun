﻿using ForexDatabase.Currencies;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ForexDatabase.Controllers
{

    public class DashboardController : ApiController
    {
        // GET: Dashboard
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/dashboard")]
        [HttpGet]
        public IEnumerable<CurrencyValue> CurrenciesValues()
        {
            CurrencyRates currencyRates = new CurrencyRates();
            IQueryable < Currency > currencies = new CurrenciesController().GetCurrencies();
            List<CurrencyValue> currencyValues = new List<CurrencyValue>();
            currencies.ToList().ForEach(currency =>
                currencyValues.Add(
                    new CurrencyValue { CurrencyId = currency.Id, Name = currency.Name, Value = currencyRates.GetExchangeRate(currency.Id) }));
            return currencyValues;
        }
    }

    public class CurrencyValue
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}