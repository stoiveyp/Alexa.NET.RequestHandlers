using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class IntentNameRequestHandler
    {
		public string IntentName { get; }

        public IntentNameRequestHandler(string intentName)
        {
            IntentName = intentName;
        }

        public bool CanHandle(SkillRequest request)
        {
            var intentRequest = request.Request as IntentRequest;
            return intentRequest != null && string.Compare(IntentName, intentRequest.Intent.Name, true) == 0;
        }

        public abstract Task<SkillResponse> Handle(SkillRequest request);
    }
}
