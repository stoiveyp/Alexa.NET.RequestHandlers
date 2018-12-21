using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.RequestHandlers.Interceptors;
using Alexa.NET.StateManagement;

namespace Alexa.NET.RequestHandlers
{
	public class AlexaRequestPipeline
	{
	    public bool RequestHandlerTriggersErrorHandlers { get; set; } = true;

		public List<IAlexaRequestHandler> RequestHandlers { get; }
		public List<IAlexaErrorHandler> ErrorHandlers { get; }
        public LinkedList<IAlexaRequestInterceptor> RequestInterceptors { get; }
        public LinkedList<IAlexaErrorInterceptor> ErrorInterceptors { get; }
		public IPersistenceStore StatePersistance { get; set; }

	    public AlexaRequestPipeline()
	    {
	        RequestHandlers = new List<IAlexaRequestHandler>();
	        ErrorHandlers = new List<IAlexaErrorHandler>();
            RequestInterceptors = new LinkedList<IAlexaRequestInterceptor>();
            ErrorInterceptors = new LinkedList<IAlexaErrorInterceptor>();
	    }

	    public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler> requestHandlers):this(requestHandlers,null,null,null)
	    {
	    }

	    public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler> requestHandlers, IEnumerable<IAlexaErrorHandler> errorHandlers) : this(requestHandlers, errorHandlers, null, null) { }


        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler> requestHandlers, 
            IEnumerable<IAlexaErrorHandler> errorHandlers, 
            IEnumerable<IAlexaRequestInterceptor> requestInterceptors, 
            IEnumerable<IAlexaErrorInterceptor> errorInterceptors)
	    {
	        RequestHandlers = requestHandlers?.ToList() ?? new List<IAlexaRequestHandler>();
	        ErrorHandlers = errorHandlers?.ToList() ?? new List<IAlexaErrorHandler>();
	        RequestInterceptors = requestInterceptors == null ? new LinkedList<IAlexaRequestInterceptor>() : new LinkedList<IAlexaRequestInterceptor>(requestInterceptors);
	        ErrorInterceptors = errorInterceptors == null ? new LinkedList<IAlexaErrorInterceptor>() : new LinkedList<IAlexaErrorInterceptor>(errorInterceptors);
        }

		public Task<SkillResponse> Process(SkillRequest input, object context = null)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Null Skill Request");
            }

			if(StatePersistance == null)
			{
				return Process(new AlexaRequestInformation(input, context));
			}

			return Process(new AlexaRequestInformation(input, context, StatePersistance));
            
        }

	    private Task<SkillResponse> Process(AlexaRequestInformation information)
        {
            IAlexaRequestHandler candidate = null;
            try
			{
				candidate = RequestHandlers.FirstOrDefault(h => h?.CanHandle(information) ?? false);
			    if (candidate == null)
			    {
			        throw new AlexaRequestHandlerNotFoundException();
			    }

				return new AlexaRequestInterceptor(RequestInterceptors.First,candidate).Intercept(information);
			}
			catch (AlexaRequestHandlerNotFoundException) when (!RequestHandlerTriggersErrorHandlers)
			{
				throw;
			}
			catch(Exception) when (!ErrorHandlers?.Any() ?? false)
			{
				throw;
			}
			catch (Exception ex)
			{
				var errorCandidate = ErrorHandlers.FirstOrDefault(eh => eh?.CanHandle(information, ex) ?? false);
				if (errorCandidate == null)
				{
					throw;
				}

				return new AlexaErrorInterceptor(ErrorInterceptors.First,candidate,errorCandidate).Intercept(information, ex);
			}
		}
    }
}
