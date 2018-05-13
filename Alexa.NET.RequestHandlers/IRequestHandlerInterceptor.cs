using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public delegate Task<SkillResponse> RequestInterceptorCall(RequestInformation request);   

    public interface IRequestHandlerInterceptor
    {
		Task<SkillResponse> Intercept(RequestInformation request, RequestInterceptorCall next);
    }
}
