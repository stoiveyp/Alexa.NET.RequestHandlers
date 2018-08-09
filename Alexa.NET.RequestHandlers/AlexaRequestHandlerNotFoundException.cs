using System;

namespace Alexa.NET.RequestHandlers
{
    public class AlexaRequestHandlerNotFoundException : Exception
    {
		private const string RequestHandlerExceptionMessage = "No matching request handler found";

		public AlexaRequestHandlerNotFoundException() : base(RequestHandlerExceptionMessage)
        {
        }

		public AlexaRequestHandlerNotFoundException(Exception innerException) : base(RequestHandlerExceptionMessage, innerException)
        {
        }
    }
}