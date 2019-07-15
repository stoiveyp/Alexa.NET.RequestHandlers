using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class LaunchRequestHandler<TSkillRequest>:IAlexaRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
        public bool CanHandle(AlexaRequestInformation<TSkillRequest> information)
        {
            return information.SkillRequest.Request is LaunchRequest;
        }

        public abstract Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information);
    }
}
