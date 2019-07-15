using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IAlexaErrorHandler:IAlexaErrorHandler<SkillRequest>{
}

public interface IAlexaErrorHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
		bool CanHandle(AlexaRequestInformation<TSkillRequest> information, Exception exception);
		Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information, Exception exception);      
    }
}
