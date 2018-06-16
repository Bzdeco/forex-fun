using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
        /// <summary>
        /// Represents selling (+1) or buying (-1)
        /// </summary>
        public int Operation { get; set; }
        /// <summary>
        /// Operation * Amount * Exchange rate at a time is operation cost
        /// </summary>
        public double Amount { get; set; }

        public virtual User User { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
