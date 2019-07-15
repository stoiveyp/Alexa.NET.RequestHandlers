using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.Handlers
{
    public abstract class IntentNameErrorHandler : IntentNameErrorHandler<SkillRequest>
    {
        protected IntentNameErrorHandler(string intentName) : base(intentName)
        {

        }
    }

    public abstract class IntentNameErrorHandler<TSkillRequest> : IAlexaErrorHandler<TSkillRequest> where TSkillRequest : SkillRequest
    {
        public string IntentName { get; }

        protected IntentNameErrorHandler(string intentName)
        {
            IntentName = intentName;
        }

        public abstract Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information, Exception exception);

        public bool CanHandle(AlexaRequestInformation<TSkillRequest> information, Exception exception)
        {
            return information.SkillRequest.Request is IntentRequest intentRequest && string.Compare(IntentName, intentRequest.Intent.Name, true) == 0;
        }
    }
}
