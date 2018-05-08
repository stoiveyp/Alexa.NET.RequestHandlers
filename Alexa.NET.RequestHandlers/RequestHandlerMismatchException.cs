using System;

namespace Alexa.NET.RequestHandlers
{
    public class RequestHandlerMissingException : Exception
    {
		private const string RequestHandlerExceptionMessage = "No matching request handler found";

		public RequestHandlerMissingException() : base(RequestHandlerExceptionMessage)
        {
        }

		public RequestHandlerMissingException(Exception innerException) : base(RequestHandlerExceptionMessage, innerException)
        {
        }
    }
}