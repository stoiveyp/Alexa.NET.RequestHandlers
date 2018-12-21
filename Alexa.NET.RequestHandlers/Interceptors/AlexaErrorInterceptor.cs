using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    public class AlexaErrorInterceptor
    {
        public AlexaErrorInterceptor(LinkedListNode<IAlexaErrorInterceptor> node, IAlexaRequestHandler requestHandler, IAlexaErrorHandler handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            RequestHandler = requestHandler;
        }

        public IAlexaRequestHandler RequestHandler { get; set; }

        public LinkedListNode<IAlexaErrorInterceptor> Node { get; }
        public IAlexaErrorHandler Handler { get; }

        public Task<SkillResponse> Intercept(AlexaRequestInformation request, Exception ex)
        {
            if (Node == null)
            {
                return Handler.Handle(request, ex);
            }

            var interceptor = Node.Value;
            if (interceptor is IHandlerAwareErrorInterceptor requestInterceptor)
            {
                requestInterceptor.Intercept(request, RequestHandler, ex, new AlexaErrorInterceptor(Node.Next, RequestHandler, Handler).Intercept);
            }

            return interceptor.Intercept(request, ex, new AlexaErrorInterceptor(Node.Next, RequestHandler, Handler).Intercept);
        }

        public Task<SkillResponse> Intercept(AlexaRequestInformation request, Exception ex, ErrorInterceptorCall next)
        {
            return Node.Value.Intercept(request, ex, next);
        }
    }
}
