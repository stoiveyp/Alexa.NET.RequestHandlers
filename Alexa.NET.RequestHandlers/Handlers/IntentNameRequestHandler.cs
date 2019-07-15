using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
	public abstract class IntentNameRequestHandler<TSkillRequest> : IAlexaRequestHandler<TSkillRequest> where TSkillRequest:SkillRequest
    {
		public string IntentName { get; }

        protected IntentNameRequestHandler(string intentName)
        {
            IntentName = intentName;
        }

        public bool CanHandle(AlexaRequestInformation<TSkillRequest> information)
        {
            return information.SkillRequest.Request is IntentRequest intentRequest 
                   && String.Compare(IntentName, intentRequest.Intent.Name, StringComparison.OrdinalIgnoreCase) == 0;
        }
              
		public abstract Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information);
	}
}
