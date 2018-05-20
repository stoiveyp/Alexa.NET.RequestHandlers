using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.StateManagement;

namespace Alexa.NET.RequestHandlers
{
	public class RequestInformation
    {
		public SkillRequest SkillRequest { get; }
		public object Context { get; }
		public Dictionary<string,object> Items { get; }
		public ISkillState State { get; }

        public RequestInformation(SkillRequest request, object context)
        {
			SkillRequest = request;
			Context = context;
			Items = new Dictionary<string, object>();
			State = request.StateManagement();
        }
    }
}
