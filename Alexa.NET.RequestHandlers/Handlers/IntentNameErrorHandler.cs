using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
	public abstract class IntentNameErrorHandler:IAlexaErrorHandler
	{
		public string IntentName { get; }

		public IntentNameErrorHandler(string intentName)
		{
			IntentName = intentName;
		}

		public abstract Task<SkillResponse> Handle(AlexaRequestInformation information, Exception exception);

		public bool CanHandle(AlexaRequestInformation information, Exception exception)
		{
			var intentRequest = information.SkillRequest.Request as IntentRequest;
            return intentRequest != null && string.Compare(IntentName, intentRequest.Intent.Name, true) == 0;
		}
	}
}
