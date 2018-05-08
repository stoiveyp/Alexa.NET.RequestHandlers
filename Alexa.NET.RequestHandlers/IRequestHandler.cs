using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
    public interface IRequestHandler
    {
		bool CanHandle(SkillRequest request);
		Task<SkillResponse> Handle(SkillRequest request);
    }
}
