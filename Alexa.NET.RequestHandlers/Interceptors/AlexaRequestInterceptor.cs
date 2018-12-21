using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    internal class AlexaRequestInterceptor : IAlexaRequestInterceptor
    {
        public AlexaRequestInterceptor(LinkedListNode<IAlexaRequestInterceptor> node, IAlexaRequestHandler handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public LinkedListNode<IAlexaRequestInterceptor> Node { get; }
        public IAlexaRequestHandler Handler { get; }

        public Task<SkillResponse> Intercept(AlexaRequestInformation information)
        {
            if (Node == null)
            {
                return Handler.Handle(information);
            }

            var interceptor = Node.Value;
            if (interceptor is IHandlerAwareRequestInterceptor requestInterceptor)
            {
                return requestInterceptor.Intercept(information, Handler, new AlexaRequestInterceptor(Node.Next, Handler).Intercept);
            }

            return interceptor.Intercept(information, new AlexaRequestInterceptor(Node.Next, Handler).Intercept);
        }

        public Task<SkillResponse> Intercept(AlexaRequestInformation request, RequestInterceptorCall next)
        {
            return Node.Value.Intercept(request, next);
        }
    }
}
