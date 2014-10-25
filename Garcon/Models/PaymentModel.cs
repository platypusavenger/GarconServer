using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class PaymentModel
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public int userCardId { get; set; }
        public decimal amount { get; set; }
        public decimal tipAmount { get; set; }
    }
}