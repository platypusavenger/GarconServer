using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class UserCardModel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string cardType { get; set; }
        public string description { get; set; }
        // Fake out storing of CC# and CVV2 -- we are abstracting authentication and payments
        public string fakeDigits { get; set; }
        public string fakeCVV2 { get; set; }
    }
}