using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    public class AlexaErrorInterceptor
    {
		public AlexaErrorInterceptor(LinkedListNode<IAlexaErrorInterceptor> node, IAlexaErrorHandler handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

		public LinkedListNode<IAlexaErrorInterceptor> Node { get; }
        public IAlexaErrorHandler Handler { get; }

		public Task<SkillResponse> Intercept(AlexaRequestInformation request, Exception ex)
        {
            if (Node == null)
            {
                return Handler.Handle(request,ex);
            }

			return Node.Value.Intercept(request, ex,new AlexaErrorInterceptor(Node.Next, Handler).Intercept);
        }

		public Task<SkillResponse> Intercept(AlexaRequestInformation request, Exception ex,ErrorInterceptorCall next)
        {
            return Node.Value.Intercept(request, ex,next);
        }
    }
}
