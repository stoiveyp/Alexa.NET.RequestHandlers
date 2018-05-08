using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IErrorHandler
    {
		bool CanHandle(SkillRequest request, Exception exception);
		Task<SkillResponse> Handle(SkillRequest request, Exception exception);      
    }
}
