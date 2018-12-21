using System;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IHandlerAwareErrorInterceptor : IAlexaErrorInterceptor
    {
        Task<SkillResponse> Intercept(AlexaRequestInformation information, IAlexaRequestHandler handler, Exception ex, ErrorInterceptorCall next);
    }
}