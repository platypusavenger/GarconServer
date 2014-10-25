using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Garcon.Classes
{
    /// <summary>
    /// IHttpActionResult wrapper which includes a message for those that do not inherently support it.
    /// </summary>
    public class ActionResultWithText : IHttpActionResult
    {
        public ActionResultWithText(string message, HttpRequestMessage request)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Message = message;
            Request = request;
        }

        public string Message { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
            response.Content = new StringContent(Message); // Put the message in the response body (text/plain content).
            response.RequestMessage = Request;
            return response;
        }
    }

    /// <summary>
    /// Add extension methods here for each base type you would like to add messaging to.
    /// </summary>
    public static class ApiControllerExtensions
    {
        /// <summary>
        /// Standard NotFound response with added message
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ActionResultWithText NotFound(this ApiController controller, string message)
        {
            return new ActionResultWithText(message, controller.Request);
        }
    }
}