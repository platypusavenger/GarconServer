using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class OrderModel
    {
        public int id { get; set; }
        public int tableId { get; set; }
        public DateTime openDateTime { get; set; }
        public Nullable<DateTime> closeDateTime { get; set; }
        public decimal taxAmount { get; set; }
        public decimal totalAmount { get; set; }
    }
}