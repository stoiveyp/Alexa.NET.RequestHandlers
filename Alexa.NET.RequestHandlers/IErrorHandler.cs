using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IErrorHandler
    {
		bool CanHandle(RequestInformation information, Exception exception);
		Task<SkillResponse> Handle(RequestInformation information, Exception exception);      
    }
}
