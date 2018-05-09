using System;

namespace Alexa.NET.RequestHandlers
{
    public class ErrorHandlerNotFoundException : HandlerNotFoundException
    {
		private const string ErrorHandlerExceptionMessage = "No matching error handler found";

		public ErrorHandlerNotFoundException() : base(ErrorHandlerExceptionMessage)
        {
        }

		public ErrorHandlerNotFoundException(Exception innerException) : base(ErrorHandlerExceptionMessage, innerException)
        {
        }
    }
}