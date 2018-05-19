using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousRequestHandler:IRequestHandler
    {
        public abstract bool CanHandle(RequestInformation request);

        public abstract SkillResponse HandleSyncRequest(RequestInformation request);

        public Task<SkillResponse> Handle(RequestInformation request)
        {
            return Task.FromResult(HandleSyncRequest(request));
        }
    }
}
