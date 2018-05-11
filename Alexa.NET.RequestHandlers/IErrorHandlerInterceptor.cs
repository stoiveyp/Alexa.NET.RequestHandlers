using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IErrorHandlerInterceptor
    {
        Task<SkillResponse> Intercept(SkillRequest request, Exception ex, IErrorHandlerInterceptor next);
    }
}
