using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.StateManagement;

namespace Alexa.NET.RequestHandlers
{
	public class AlexaRequestInformation
	{
		public SkillRequest SkillRequest { get; }
		public object Context { get; }
		public Dictionary<string, object> Items { get; }
		public ISkillState State { get; }

		public AlexaRequestInformation(SkillRequest request, object context)
		{
			SkillRequest = request;
			Context = context;
			Items = new Dictionary<string, object>();
			State = new SkillState(request.Session);
		}

		public AlexaRequestInformation(SkillRequest request, object context, IPersistenceStore persistenceStore)
		{
			SkillRequest = request;
			Context = context;
			Items = new Dictionary<string, object>();
			State = new SkillState(request.Session, persistenceStore);
		}
	}
}
