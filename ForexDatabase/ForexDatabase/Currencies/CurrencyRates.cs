﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ForexDatabase.Currencies
{
    public class CurrencyRates : ICurrencyRates
    {
        static HttpClient client = new HttpClient();

        private Dictionary<string, double> exchangeRates = new Dictionary<string, double>();

        public CurrencyRates()
        {
            Console.WriteLine("CurrencyRates :)");
            exchangeRates.Add("USD", 1.0);

            client.BaseAddress = new Uri("https://forex.1forge.com/1.0.3/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            List<string> currencies = new List<string>(new string[] { "ETHUSD", "BTCUSD", "LTCUSD", "XRPUSD", "DSHUSD", "BCHUSD" });
            //currencies.ForEach(currencyName =>
            //{
            //    Debug.WriteLine("Adding currency started");
            //    List<CurrencyRate> currencyRates = GetCurrencyAsync(client.BaseAddress.ToString(),
            //                "4GOlRVjlyHsah857qPCoy6C46leXoQqA", currencyName).;
            //    currencyRates.ForEach(currencyRate => AddToDictionary(currencyRate));
            //    Debug.WriteLine("Adding currency ended");
            //});
            Debug.WriteLine("Starting tasks currency");
            currencies.ForEach(currencyName => Task.Run(() => RunAsync(currencyName)));
            Debug.WriteLine("Started tasks currency");
        }

        private void AddToDictionary(CurrencyRate currencyRate)
        {
            exchangeRates[currencyRate.Symbol.Substring(0, 3)] = currencyRate.Price;
        }

        public double GetExchangeRate(string currencyName)
        {
            return exchangeRates[currencyName];
        }

        async Task<List<CurrencyRate>> GetCurrencyAsync(string path, string apiKey, string currenciesNames)
        {
            List<CurrencyRate> currencyRates = null;
            HttpResponseMessage response = await client.GetAsync("quotes/?pairs=" + currenciesNames + "&api_key=" + apiKey);
            if (response.IsSuccessStatusCode)
            {
                currencyRates = await response.Content.ReadAsAsync<List<CurrencyRate>>();
            }
            return currencyRates;
        }

        async Task RunAsync(string currencyName)
        {
            while (true)
            {
                try
                {
                    // Get the currency rate
                    List<CurrencyRate> currencyRates = await GetCurrencyAsync(client.BaseAddress.ToString(),
                        "KoepnX5QLsaKZD9TmGIrO67Idwaq5dns", currencyName);
                    currencyRates.ForEach(currencyRate => AddToDictionary(currencyRate));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                await Task.Delay(60000);
            }
        }
    }

    public class CurrencyRate
    {
        public string Symbol { get; set; }
        public float Price { get; set; }
    }
}