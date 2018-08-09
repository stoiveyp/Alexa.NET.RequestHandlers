using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IAlexaErrorHandler
    {
		bool CanHandle(AlexaRequestInformation information, Exception exception);
		Task<SkillResponse> Handle(AlexaRequestInformation information, Exception exception);      
    }
}
