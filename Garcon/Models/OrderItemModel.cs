using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class OrderItemModel
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
    }
}