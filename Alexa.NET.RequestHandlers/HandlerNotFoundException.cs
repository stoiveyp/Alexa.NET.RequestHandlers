using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.RequestHandlers
{
    public abstract class HandlerNotFoundException:Exception
    {
        protected HandlerNotFoundException(string message) : base(message)
        {
        }

        protected HandlerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
