using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class AlwaysTrueRequestHandler:IAlexaRequestHandler
    {
		public bool CanHandle(AlexaRequestInformation information) => true;

		public abstract Task<SkillResponse> Handle(AlexaRequestInformation information);
    }
}
