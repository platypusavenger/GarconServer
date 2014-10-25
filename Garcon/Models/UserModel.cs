using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }

    public class UserHistoryModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }

        public List<OrderDetailModel> OrderHistory { get; set; }
    }

}