using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class AlwaysTrueErrorHandler:IErrorHandler
    {
        public bool CanHandle(SkillRequest request, Exception exception) => true;

        public abstract Task<SkillResponse> Handle(SkillRequest request, Exception exception);
    }
}
