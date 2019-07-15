using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class AlwaysTrueRequestHandler:AlwaysTrueRequestHandler<SkillRequest>{
}

public abstract class AlwaysTrueRequestHandler<TSkillRequest> : IAlexaRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
		public bool CanHandle(AlexaRequestInformation<TSkillRequest> information) => true;

		public abstract Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information);
    }
}
