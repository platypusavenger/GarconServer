using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Garcon.Areas.HelpPage.Models
{
    public class PostmanCollection
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public long timestamp { get; set; }
        public Collection<PostmanRequest> requests { get; set; }
    }

    public class PostmanRequest
    {
        public Guid collectionId { get; set; }
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string method { get; set; }
        public string headers { get; set; }
        public string data { get; set; }
        public string dataMode { get; set; }
        public long timestamp { get; set; }
    }
}