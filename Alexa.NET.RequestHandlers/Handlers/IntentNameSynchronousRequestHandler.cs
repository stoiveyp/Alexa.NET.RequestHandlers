using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class IntentNameSynchronousRequestHandler<TSkillRequest> : IntentNameRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
        protected IntentNameSynchronousRequestHandler(string intentName) : base(intentName)
        {
        }

        public override Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information)
        {
            return Task.FromResult(HandleSyncRequest(information));
        }

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation<TSkillRequest> information);
    }
}
