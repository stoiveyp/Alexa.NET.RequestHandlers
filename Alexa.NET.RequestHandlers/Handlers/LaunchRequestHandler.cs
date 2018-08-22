using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class LaunchRequestHandler:IAlexaRequestHandler
    {
        public bool CanHandle(AlexaRequestInformation information)
        {
            return information.SkillRequest.Request is LaunchRequest;
        }

        public abstract Task<SkillResponse> Handle(AlexaRequestInformation information);
    }
}
