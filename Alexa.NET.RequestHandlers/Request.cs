using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public class Request
	{
	    public bool RequestHandlerTriggersErrorHandlers { get; set; } = true;

		public List<IRequestHandler> RequestHandlers { get; }
		public List<IErrorHandler> ErrorHandlers { get; }

	    public Request()
	    {
	        RequestHandlers = new List<IRequestHandler>();
	        ErrorHandlers = new List<IErrorHandler>();
	    }

	    public Request(IEnumerable<IRequestHandler> requestHandlers, IEnumerable<IErrorHandler> errorHandlers)
	    {
	        RequestHandlers = (requestHandlers ?? new IRequestHandler[]{}).ToList();
	        ErrorHandlers = (errorHandlers ?? new IErrorHandler[]{}).ToList();
	    }

	    public Task<SkillResponse> Process(SkillRequest input)
		{
		    if (input == null)
		    {
		        throw new ArgumentNullException(nameof(input), "Null Skill Request");
		    }

            try
			{
			    var candidate = RequestHandlers.FirstOrDefault(h => h?.CanHandle(input) ?? false);
			    if (candidate == null)
			    {
			        throw new RequestHandlerNotFoundException();
			    }

                return candidate.Handle(input);
			}
			catch (RequestHandlerNotFoundException) when (!RequestHandlerTriggersErrorHandlers)
			{
				throw;
			}
			catch (Exception ex)
			{
				var errorCandidate = ErrorHandlers.FirstOrDefault(eh => eh?.CanHandle(input, ex) ?? false);
				if (errorCandidate == null)
				{
					throw new ErrorHandlerNotFoundException(ex);
				}

				return errorCandidate.Handle(input, ex);
			}
		}
	}
}
