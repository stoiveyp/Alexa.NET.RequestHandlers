﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Interceptors
{
    internal class RequestHandlerInterceptor:IRequestHandlerInterceptor
    {
		public RequestHandlerInterceptor(LinkedListNode<IRequestHandlerInterceptor> node, IRequestHandler handler)
        {
            Node = node;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public LinkedListNode<IRequestHandlerInterceptor> Node { get; }
        public IRequestHandler Handler { get; }

        public Task<SkillResponse> Intercept(RequestInformation information)
        {
            if (Node == null)
            {
                return Handler.Handle(information);
            }

			return Node.Value.Intercept(information, new RequestHandlerInterceptor(Node.Next,Handler).Intercept);
        }

		public Task<SkillResponse> Intercept(RequestInformation request, RequestInterceptorCall next)
        {
            return Node.Value.Intercept(request, next);
        }
    }
}
