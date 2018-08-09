using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public delegate Task<SkillResponse> ErrorInterceptorCall(AlexaRequestInformation information, Exception ex);

    public interface IAlexaErrorInterceptor
    {
		Task<SkillResponse> Intercept(AlexaRequestInformation information, Exception ex, ErrorInterceptorCall next);
    }
}
