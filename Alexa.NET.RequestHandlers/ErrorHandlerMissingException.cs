using System;

namespace Alexa.NET.RequestHandlers
{
    internal class ErrorHandlerMissingException : Exception
    {
		private const string ErrorHandlerExceptionMessage = "No matching error handler found";

		public ErrorHandlerMissingException() : base(ErrorHandlerExceptionMessage)
        {
        }

		public ErrorHandlerMissingException(Exception innerException) : base(ErrorHandlerExceptionMessage, innerException)
        {
        }
    }
}