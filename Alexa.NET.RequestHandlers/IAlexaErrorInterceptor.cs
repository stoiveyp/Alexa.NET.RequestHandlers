using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public delegate Task<SkillResponse> ErrorInterceptorCall<TSkillRequest>(AlexaRequestInformation<TSkillRequest> information, Exception ex) where TSkillRequest : SkillRequest;

    public interface IAlexaErrorInterceptor<TSkillRequest> where TSkillRequest : SkillRequest
    {
		Task<SkillResponse> Intercept(AlexaRequestInformation<TSkillRequest> information, Exception ex, ErrorInterceptorCall<TSkillRequest> next);
    }
}
