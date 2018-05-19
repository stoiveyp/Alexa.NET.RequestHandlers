using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class SynchronousErrorHandler:IErrorHandler
    {
        public abstract bool CanHandle(RequestInformation request, Exception exception);

        public abstract SkillResponse HandleSyncRequest(RequestInformation request, Exception exception);

        public Task<SkillResponse> Handle(RequestInformation request, Exception exception)
        {
            return Task.FromResult(HandleSyncRequest(request, exception));
        }
    }
}
