using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public class RequestHandler
	{
		public List<IRequestHandler> RequestHandlers { get; } = new List<IRequestHandler>();
		public List<IErrorHandler> ErrorHandlers { get; } = new List<IErrorHandler>();

		public static RequestHandler Default { get; set; }

		public Task<SkillResponse> Process(SkillRequest input)
		{
			try
			{
				var candidate = RequestHandlers.First(h => h.CanHandle(input));
				if (candidate == null)
				{
					throw new RequestHandlerMissingException();
				}
				return candidate.Handle(input);
			}
			catch (RequestHandlerMissingException)
			{
				throw;
			}
			catch (Exception ex)
			{
				var errorCandidate = ErrorHandlers.First(eh => eh.CanHandle(input, ex));
				if (errorCandidate == null)
				{
					throw new ErrorHandlerMissingException(ex);
				}

				return errorCandidate.Handle(input, ex);
			}
		}
	}
}
