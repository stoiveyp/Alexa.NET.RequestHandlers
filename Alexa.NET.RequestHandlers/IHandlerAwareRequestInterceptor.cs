using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IHandlerAwareRequestInterceptor:IAlexaRequestInterceptor
    {
        Task<SkillResponse> Intercept(AlexaRequestInformation information, IAlexaRequestHandler handler, RequestInterceptorCall next);
    }
}
