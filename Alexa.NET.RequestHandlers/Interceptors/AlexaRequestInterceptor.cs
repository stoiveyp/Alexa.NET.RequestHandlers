using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    internal class AlexaRequestInterceptor<TSkillRequest> : IAlexaRequestInterceptor<TSkillRequest> where TSkillRequest:SkillRequest
    {
        public AlexaRequestInterceptor(LinkedListNode<IAlexaRequestInterceptor<TSkillRequest>> node, IAlexaRequestHandler<TSkillRequest> handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public LinkedListNode<IAlexaRequestInterceptor<TSkillRequest>> Node { get; }
        public IAlexaRequestHandler<TSkillRequest> Handler { get; }

        public Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> information)
        {
            if (Node == null)
            {
                return Handler.Handle(information);
            }

            var interceptor = Node.Value;
            if (Handler != null && interceptor is IHandlerAwareRequestInterceptor<TSkillRequest> requestInterceptor)
            {
                return requestInterceptor.Intercept(information, Handler, new AlexaRequestInterceptor<TSkillRequest>(Node.Next, Handler).Intercept);
            }

            return interceptor.Intercept(information, new AlexaRequestInterceptor<TSkillRequest>(Node.Next, Handler).Intercept);
        }

        public Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> request, RequestInterceptorCall<TSkillRequest> next)
        {
            return Node.Value.Intercept(request, next);
        }
    }
}
