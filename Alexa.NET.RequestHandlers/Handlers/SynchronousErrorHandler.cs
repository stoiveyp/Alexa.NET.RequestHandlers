using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousErrorHandler:SynchronousErrorHandler<SkillRequest>{
}

public abstract class SynchronousErrorHandler<TSkillRequest> : IAlexaErrorHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
        public abstract bool CanHandle(AlexaRequestInformation<TSkillRequest> information, Exception exception);

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation<TSkillRequest> information, Exception exception);

        public Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information, Exception exception)
        {
            return Task.FromResult(HandleSyncRequest(information, exception));
        }
    }
}
