using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class TableModel
    {
        public int id { get; set; }
        public string beaconId { get; set; }
        public string description { get; set; }
        public bool available { get; set; }
    }
}