using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.StateManagement;

namespace Alexa.NET.RequestHandlers
{
    public class AlexaRequestInformation<TSkillRequest> where TSkillRequest:SkillRequest
	{
		public TSkillRequest SkillRequest { get; }
		public object Context { get; }
		public Dictionary<string, object> Items { get; }
		public ISkillState State { get; }

		public AlexaRequestInformation(TSkillRequest request, object context)
		{
			SkillRequest = request;
			Context = context;
			Items = new Dictionary<string, object>();
			State = new SkillState(request);
		}

		public AlexaRequestInformation(TSkillRequest request, object context, IPersistenceStore persistenceStore)
		{
			SkillRequest = request;
			Context = context;
			Items = new Dictionary<string, object>();
			State = new SkillState(request, persistenceStore);
		}
	}
}
