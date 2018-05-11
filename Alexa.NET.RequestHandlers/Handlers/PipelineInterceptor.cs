using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    internal class PipelineInterceptor:IRequestHandlerInterceptor
    {
        public PipelineInterceptor(LinkedListNode<IRequestHandlerInterceptor> node, IRequestHandler handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public LinkedListNode<IRequestHandlerInterceptor> Node { get; }
        public IRequestHandler Handler { get; }

        public Task<SkillResponse> Intercept(SkillRequest request)
        {
            if (Node == null)
            {
                return Handler.Handle(request);
            }

            return Node.Value.Intercept(request, new PipelineInterceptor(Node.Next,Handler).Intercept);
        }

        public Task<SkillResponse> Intercept(SkillRequest request, RequestInterceptorCall next)
        {
            return Node.Value.Intercept(request, next);
        }
    }
}
