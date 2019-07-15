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
    public class AlexaRequestPipeline : AlexaRequestPipeline<SkillRequest>
    {
        public AlexaRequestPipeline()
        {

        }

        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler<SkillRequest>> requestHandlers) : base(requestHandlers, null, null, null)
        {
        }

        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler<SkillRequest>> requestHandlers, IEnumerable<IAlexaErrorHandler<SkillRequest>> errorHandlers) : base(requestHandlers, errorHandlers, null, null) { }


        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler<SkillRequest>> requestHandlers,
            IEnumerable<IAlexaErrorHandler<SkillRequest>> errorHandlers,
            IEnumerable<IAlexaRequestInterceptor<SkillRequest>> requestInterceptors,
            IEnumerable<IAlexaErrorInterceptor<SkillRequest>> errorInterceptors)
        :base(requestHandlers,errorHandlers,requestInterceptors,errorInterceptors)
        {

        }
    }

    public class AlexaRequestPipeline<TSkillRequest> where TSkillRequest : SkillRequest
    {
        public bool RequestHandlerTriggersErrorHandlers { get; set; } = true;

        public List<IAlexaRequestHandler<TSkillRequest>> RequestHandlers { get; }
        public List<IAlexaErrorHandler<TSkillRequest>> ErrorHandlers { get; }
        public LinkedList<IAlexaRequestInterceptor<TSkillRequest>> RequestInterceptors { get; }
        public LinkedList<IAlexaErrorInterceptor<TSkillRequest>> ErrorInterceptors { get; }
        public IPersistenceStore StatePersistance { get; set; }

        public AlexaRequestPipeline()
        {
            RequestHandlers = new List<IAlexaRequestHandler<TSkillRequest>>();
            ErrorHandlers = new List<IAlexaErrorHandler<TSkillRequest>>();
            RequestInterceptors = new LinkedList<IAlexaRequestInterceptor<TSkillRequest>>();
            ErrorInterceptors = new LinkedList<IAlexaErrorInterceptor<TSkillRequest>>();
        }

        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler<TSkillRequest>> requestHandlers) : this(requestHandlers, null, null, null)
        {
        }

        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler<TSkillRequest>> requestHandlers, IEnumerable<IAlexaErrorHandler<TSkillRequest>> errorHandlers) : this(requestHandlers, errorHandlers, null, null) { }


        public AlexaRequestPipeline(IEnumerable<IAlexaRequestHandler<TSkillRequest>> requestHandlers,
            IEnumerable<IAlexaErrorHandler<TSkillRequest>> errorHandlers,
            IEnumerable<IAlexaRequestInterceptor<TSkillRequest>> requestInterceptors,
            IEnumerable<IAlexaErrorInterceptor<TSkillRequest>> errorInterceptors)
        {
            RequestHandlers = requestHandlers?.ToList() ?? new List<IAlexaRequestHandler<TSkillRequest>>();
            ErrorHandlers = errorHandlers?.ToList() ?? new List<IAlexaErrorHandler<TSkillRequest>>();
            RequestInterceptors = requestInterceptors == null ? new LinkedList<IAlexaRequestInterceptor<TSkillRequest>>() : new LinkedList<IAlexaRequestInterceptor<TSkillRequest>>(requestInterceptors);
            ErrorInterceptors = errorInterceptors == null ? new LinkedList<IAlexaErrorInterceptor<TSkillRequest>>() : new LinkedList<IAlexaErrorInterceptor<TSkillRequest>>(errorInterceptors);
        }

        public Task<SkillResponse> Process(TSkillRequest input, object context = null)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Null Skill Request");
            }

            if (StatePersistance == null)
            {
                return Process(new AlexaRequestInformation<TSkillRequest>(input, context));
            }

            return Process(new AlexaRequestInformation<TSkillRequest>(input, context, StatePersistance));

        }

        private async Task<SkillResponse> Process(AlexaRequestInformation<TSkillRequest> information)
        {
            IAlexaRequestHandler<TSkillRequest> candidate = null;
            try
            {
                candidate = RequestHandlers.FirstOrDefault(h => h?.CanHandle(information) ?? false);
                if (candidate == null)
                {
                    throw new AlexaRequestHandlerNotFoundException();
                }

                return await new AlexaRequestInterceptor<TSkillRequest>(RequestInterceptors.First, candidate).Intercept(information);
            }
            catch (AlexaRequestHandlerNotFoundException) when (!RequestHandlerTriggersErrorHandlers)
            {
                throw;
            }
            catch (Exception) when (!ErrorHandlers?.Any() ?? false)
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

                return await new AlexaErrorInterceptor<TSkillRequest>(ErrorInterceptors.First, candidate, errorCandidate).Intercept(information, ex);
            }
        }
    }
}
