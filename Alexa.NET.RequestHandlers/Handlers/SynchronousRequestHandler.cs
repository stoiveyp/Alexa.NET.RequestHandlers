using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousRequestHandler:IAlexaRequestHandler
    {
        public abstract bool CanHandle(AlexaRequestInformation information);

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation information);

        public Task<SkillResponse> Handle(AlexaRequestInformation information)
        {
            return Task.FromResult(HandleSyncRequest(information));
        }
    }
}
