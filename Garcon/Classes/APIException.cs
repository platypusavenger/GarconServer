using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Garcon.Classes
{
    public class APIException : Exception
    {
        public string message { get; set; }
        public int code { get; set; }

        public APIException() { this.message = ""; this.code = 500; }
        public APIException(string message) : base(message) { this.message = message; this.code = 500; }
        public APIException(string message, Exception inner) : base(message, inner) { this.message = message; this.code = 500; }
        public APIException(string message, int code) : base(message) { this.message = message; this.code = code; }
        public APIException(string message, int code, Exception inner) : base(message, inner) { this.message = message; this.code = code; }

        protected APIException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
