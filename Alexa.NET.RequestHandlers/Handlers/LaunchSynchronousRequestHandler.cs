using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class LaunchSynchronousRequestHandler : LaunchRequestHandler
    {
        public override Task<SkillResponse> Handle(AlexaRequestInformation information)
        {
            return Task.FromResult(HandleSyncRequest(information));
        }

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation information);
    }
}
