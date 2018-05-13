using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IRequestHandler
    {
		bool CanHandle(RequestInformation request);
		Task<SkillResponse> Handle(RequestInformation request);
    }
}
