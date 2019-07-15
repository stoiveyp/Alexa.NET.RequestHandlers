using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.StateManagement;

namespace Alexa.NET.RequestHandlers
{
    public class AlexaRequestInformation : AlexaRequestInformation<SkillRequest>
    {
        public AlexaRequestInformation(SkillRequest request, object context) : base(request, context)
        {

        }

        public AlexaRequestInformation(SkillRequest request, object context, IPersistenceStore persistenceStore) : base(request, context, persistenceStore)
        {

        }
    }

    public class AlexaRequestInformation<TSkillRequest> where TSkillRequest : SkillRequest
    {
        public TSkillRequest SkillRequest { get; }
        public object Context { get; }
        public Dictionary<string, object> Items { get; }
        public ISkillState State { get; }

        protected AlexaRequestInformation(TSkillRequest request, object context, SkillState state)
        {
            SkillRequest = request;
            Context = context;
            Items = new Dictionary<string, object>();
            State = state;
        }

        public AlexaRequestInformation(TSkillRequest request, object context)
            : this(request, context, new SkillState(request))
        {

        }

        public AlexaRequestInformation(TSkillRequest request, object context, IPersistenceStore persistenceStore)
        : this(request, context, new SkillState(request, persistenceStore))
        {

        }
    }
}
