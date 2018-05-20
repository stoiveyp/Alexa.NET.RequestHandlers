using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
	public abstract class IntentNameErrorHandler:IErrorHandler
	{
		public string IntentName { get; }

		public IntentNameErrorHandler(string intentName)
		{
			IntentName = intentName;
		}

		public abstract Task<SkillResponse> Handle(RequestInformation information, Exception exception);

		public bool CanHandle(RequestInformation information, Exception exception)
		{
			var intentRequest = information.SkillRequest.Request as IntentRequest;
            return intentRequest != null && string.Compare(IntentName, intentRequest.Intent.Name, true) == 0;
		}
	}
}
