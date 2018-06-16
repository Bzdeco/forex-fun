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
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new CurrencyValue
            {
                Name = "Currency" + rng.Next(0, 100),
                Value = rng.NextDouble() * 5.0
            });
        }
    }

    public class CurrencyValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}