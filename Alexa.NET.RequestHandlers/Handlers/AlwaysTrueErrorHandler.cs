using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{

    public abstract class AlwaysTrueErrorHandler:AlwaysTrueErrorHandler<SkillRequest>{
}

public abstract class AlwaysTrueErrorHandler<TSkillRequest> : IAlexaErrorHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
		public bool CanHandle(AlexaRequestInformation<TSkillRequest> information, Exception exception) => true;

		public abstract Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information, Exception exception);
    }
}
