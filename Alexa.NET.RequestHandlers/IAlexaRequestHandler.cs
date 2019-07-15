using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IAlexaRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
		bool CanHandle(AlexaRequestInformation<TSkillRequest> information);
		Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information);
    }
}
