using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class AlwaysTrueRequestHandler:IRequestHandler
    {
        public bool CanHandle(SkillRequest request) => true;

        public abstract Task<SkillResponse> Handle(SkillRequest request);
    }
}
