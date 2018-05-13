using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public delegate Task<SkillResponse> ErrorInterceptorCall(RequestInformation request, Exception ex);

    public interface IErrorHandlerInterceptor
    {
		Task<SkillResponse> Intercept(RequestInformation request, Exception ex, ErrorInterceptorCall next);
    }
}
