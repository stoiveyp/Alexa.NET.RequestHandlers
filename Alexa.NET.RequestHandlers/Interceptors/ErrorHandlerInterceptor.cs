using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    public class ErrorHandlerInterceptor
    {
		public ErrorHandlerInterceptor(LinkedListNode<IErrorHandlerInterceptor> node, IErrorHandler handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

		public LinkedListNode<IErrorHandlerInterceptor> Node { get; }
        public IErrorHandler Handler { get; }

        public Task<SkillResponse> Intercept(SkillRequest request, Exception ex)
        {
            if (Node == null)
            {
                return Handler.Handle(request,ex);
            }

			return Node.Value.Intercept(request, ex,new ErrorHandlerInterceptor(Node.Next, Handler).Intercept);
        }

		public Task<SkillResponse> Intercept(SkillRequest request, Exception ex,ErrorInterceptorCall next)
        {
            return Node.Value.Intercept(request, ex,next);
        }
    }
}
