using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers
{
	public class Request
	{
	    public bool RequestHandlerTriggersErrorHandlers { get; set; } = true;

		public List<IRequestHandler> RequestHandlers { get; }
		public List<IErrorHandler> ErrorHandlers { get; }
        public LinkedList<IRequestHandlerInterceptor> RequestInterceptors { get; }
        public LinkedList<IErrorHandlerInterceptor> ErrorInterceptors { get; }

	    public Request()
	    {
	        RequestHandlers = new List<IRequestHandler>();
	        ErrorHandlers = new List<IErrorHandler>();
            RequestInterceptors = new LinkedList<IRequestHandlerInterceptor>();
            ErrorInterceptors = new LinkedList<IErrorHandlerInterceptor>();
	    }

	    public Request(IEnumerable<IRequestHandler> requestHandlers):this(requestHandlers,null,null,null)
	    {
	    }

	    public Request(IEnumerable<IRequestHandler> requestHandlers, IEnumerable<IErrorHandler> errorHandlers) : this(requestHandlers, errorHandlers, null, null) { }


        public Request(IEnumerable<IRequestHandler> requestHandlers, 
            IEnumerable<IErrorHandler> errorHandlers, 
            IEnumerable<IRequestHandlerInterceptor> requestInterceptors, 
            IEnumerable<IErrorHandlerInterceptor> errorInterceptors)
	    {
	        RequestHandlers = requestHandlers?.ToList() ?? new List<IRequestHandler>();
	        ErrorHandlers = errorHandlers?.ToList() ?? new List<IErrorHandler>();
	        RequestInterceptors = requestInterceptors == null ? new LinkedList<IRequestHandlerInterceptor>() : new LinkedList<IRequestHandlerInterceptor>(requestInterceptors);
	        ErrorInterceptors = errorInterceptors == null ? new LinkedList<IErrorHandlerInterceptor>() : new LinkedList<IErrorHandlerInterceptor>(errorInterceptors);
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

			    if (!RequestInterceptors.Any()) return candidate.Handle(input);

			    return new PipelineInterceptor(RequestInterceptors.First,candidate).Intercept(input);
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

        public Task<SkillResponse> GetNext(SkillRequest input, LinkedListNode<Func<SkillRequest, Func<SkillRequest, Task<SkillResponse>>, Task<SkillResponse>>> node)
	    {
	        return node?.Value(input, node.Next == null ? null : GetFunc(node.Next));
	    }

	    public Func<SkillRequest, Task<SkillResponse>> GetFunc(LinkedListNode<Func<SkillRequest, Func<SkillRequest, Task<SkillResponse>>, Task<SkillResponse>>> node)
	    {
	        return skillRequest => GetNext(skillRequest, node);
	    }
    }
}
