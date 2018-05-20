using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousRequestHandler:IRequestHandler
    {
        public abstract bool CanHandle(RequestInformation information);

        public abstract SkillResponse HandleSyncRequest(RequestInformation information);

        public Task<SkillResponse> Handle(RequestInformation information)
        {
            return Task.FromResult(HandleSyncRequest(information));
        }
    }
}
