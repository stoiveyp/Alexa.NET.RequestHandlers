using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousRequestHandler<TSkillRequest> : IAlexaRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
        public abstract bool CanHandle(AlexaRequestInformation<TSkillRequest> information);

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation<TSkillRequest> information);

        public Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information)
        {
            return Task.FromResult(HandleSyncRequest(information));
        }
    }
}
