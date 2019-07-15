using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IHandlerAwareErrorInterceptor<TSkillRequest> : IAlexaErrorInterceptor<TSkillRequest> where TSkillRequest:SkillRequest
    {
        Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> information, IAlexaRequestHandler<TSkillRequest> handler, Exception ex, ErrorInterceptorCall<TSkillRequest> next);
    }
}