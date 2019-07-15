using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IHandlerAwareRequestInterceptor : IHandlerAwareRequestInterceptor<SkillRequest>
    {
    }

    public interface IHandlerAwareRequestInterceptor<TSkillRequest> : IAlexaRequestInterceptor<TSkillRequest> where TSkillRequest : SkillRequest
    {
        Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> information, IAlexaRequestHandler<TSkillRequest> handler, RequestInterceptorCall<TSkillRequest> next);
    }
}
