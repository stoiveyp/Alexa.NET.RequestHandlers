using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class LaunchSynchronousRequestHandler:LaunchSynchronousRequestHandler<SkillRequest>{
}

public abstract class LaunchSynchronousRequestHandler<TSkillRequest> : LaunchRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
        public override Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information)
        {
            return Task.FromResult(HandleSyncRequest(information));
        }

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation<TSkillRequest> information);
    }
}
