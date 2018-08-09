using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IAlexaRequestHandler
    {
		bool CanHandle(AlexaRequestInformation information);
		Task<SkillResponse> Handle(AlexaRequestInformation information);
    }
}
