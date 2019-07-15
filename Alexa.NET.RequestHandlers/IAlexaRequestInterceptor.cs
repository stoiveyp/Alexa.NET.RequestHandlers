using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public delegate Task<SkillResponse> RequestInterceptorCall<TSkillRequest>(AlexaRequestInformation<TSkillRequest> information) where TSkillRequest:SkillRequest;   

    public interface IAlexaRequestInterceptor<TSkillRequest> where TSkillRequest : SkillRequest
    {
		Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> information, RequestInterceptorCall<TSkillRequest> next);
    }
}
