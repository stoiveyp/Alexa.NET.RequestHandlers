using System;

namespace Alexa.NET.RequestHandlers
{
    public class RequestHandlerNotFoundException : HandlerNotFoundException
    {
		private const string RequestHandlerExceptionMessage = "No matching request handler found";

		public RequestHandlerNotFoundException() : base(RequestHandlerExceptionMessage)
        {
        }

		public RequestHandlerNotFoundException(Exception innerException) : base(RequestHandlerExceptionMessage, innerException)
        {
        }
    }
}