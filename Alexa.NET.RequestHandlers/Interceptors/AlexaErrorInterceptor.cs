using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    public class AlexaErrorInterceptor : AlexaErrorInterceptor<SkillRequest>
    {
        public AlexaErrorInterceptor(LinkedListNode<IAlexaErrorInterceptor<SkillRequest>> node, 
            IAlexaRequestHandler<SkillRequest> requestHandler, 
            IAlexaErrorHandler<SkillRequest> handler):
            base(node,requestHandler,handler)
        {

        }
    }

    public class AlexaErrorInterceptor<TSkillRequest> where TSkillRequest : SkillRequest
    {
        public AlexaErrorInterceptor(LinkedListNode<IAlexaErrorInterceptor<TSkillRequest>> node, IAlexaRequestHandler<TSkillRequest> requestHandler, IAlexaErrorHandler<TSkillRequest> handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            RequestHandler = requestHandler;
        }

        public IAlexaRequestHandler<TSkillRequest> RequestHandler { get; set; }

        public LinkedListNode<IAlexaErrorInterceptor<TSkillRequest>> Node { get; }
        public IAlexaErrorHandler<TSkillRequest> Handler { get; }

        public Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> request, Exception ex)
        {
            if (Node == null)
            {
                return Handler.Handle(request, ex);
            }

            var interceptor = Node.Value;
            if (RequestHandler != null && interceptor is IHandlerAwareErrorInterceptor<TSkillRequest> requestInterceptor)
            {
                requestInterceptor.Intercept(request, RequestHandler, ex, new AlexaErrorInterceptor<TSkillRequest>(Node.Next, RequestHandler, Handler).Intercept);
            }

            return interceptor.Intercept(request, ex, new AlexaErrorInterceptor<TSkillRequest>(Node.Next, RequestHandler, Handler).Intercept);
        }

        public Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> request, Exception ex, ErrorInterceptorCall<TSkillRequest> next)
        {
            return Node.Value.Intercept(request, ex, next);
        }
    }
}
