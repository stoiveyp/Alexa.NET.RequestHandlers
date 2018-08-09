using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class AlwaysTrueErrorHandler:IAlexaErrorHandler
    {
		public bool CanHandle(AlexaRequestInformation information, Exception exception) => true;

		public abstract Task<SkillResponse> Handle(AlexaRequestInformation information, Exception exception);
    }
}
