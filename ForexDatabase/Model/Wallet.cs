using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Wallet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CurrencyId { get; set; }
        public double Amount { get; set; }

        public virtual Currency Currency { get; set; }
    }
}
