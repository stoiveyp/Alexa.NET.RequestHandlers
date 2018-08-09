using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousErrorHandler:IAlexaErrorHandler
    {
        public abstract bool CanHandle(AlexaRequestInformation information, Exception exception);

        public abstract SkillResponse HandleSyncRequest(AlexaRequestInformation information, Exception exception);

        public Task<SkillResponse> Handle(AlexaRequestInformation information, Exception exception)
        {
            return Task.FromResult(HandleSyncRequest(information, exception));
        }
    }
}
