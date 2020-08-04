# Alexa.NET.RequestHandlers

Alexa skills can get large and complicated and editing long switch statements can be error prone. This library allows you to isolate functionality into RequestHandlers. A request handler is an isolated piece of logic that you want your alexa skill to run based on a particular condition (it's a launch request, it's a specific type of intent, its the fallbackintent and the user has an account linked etc.).

# So what is a request handler?

From a code point of view it's any class that implements the following interface

```csharp
public interface IAlexaRequestHandler
{
	bool CanHandle(AlexaRequestInformation information);
	Task<SkillResponse> Handle(AlexaRequestInformation information);
}
```

The way this works is that when brought together in a pipeline and a request is processed, each of the request handlers has its `CanHandle` method executed in declaration order. The first handler that returns true is selected, and the handler logic in the `Handle` method is executed to generated the skill response.

Here's a few examples of a request handler

__Launch Request:__
```csharp
public class LaunchRequestHandler:LaunchRequestHandler
{
    public bool CanHandle(AlexaRequestInformation information)
    {
        return information.SkillRequest.Request is LaunchRequest;
    }

    public Task<SkillResponse> Handle(AlexaRequestInformation<TSkillRequest> information){
        return Task.FromResult(ResponseBuilder.Ask("hello, what should I call you?", null));
    }
}
```

__Synchronous intent request__
```csharp
public class AnswerIntentRequestHandler:IntentNameSynchronousRequestHandler
{
    public AnswerIntentRequestHandler():base("answer")

    public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
    {
        var intentRequest = information.SkillRequest.Request as IntentRequest;
        return ResponseBuilder.Ask($"hello {intentRequest.Intent.Slots["answer"].Value}", null);
    }
}
```

__Specific request criteria with a non-standard request object (from Alexa.NET.APL)__
```csharp
public class AnswerIntentRequestHandler:IAlexaRequestHandler<APLSkillRequest>
{
    public bool CanHandle(AlexaRequestInformation<APLSkillRequest> information)
    {
        return skillRequest.APLSupported();
    }

    public async Task<SkillResponse> Handle(AlexaRequestInformation<APLSkillRequest> information){
        var response = ResponseBuilder.Empty();
	var document = await GenerateDisplay(information.State.Session);
	AddToResponse(response, document);
	return response;
    }
}
```

# Executing your request handlers

To execute your request handlers you build an AlexaRequestPipeline and register each of your RequestHanders. As we've said order here is important - it will allow you to make handlers that deal with subtle differences in functionality and you can register the most specific first (such as the two buy handlers in the example below).

```csharp
var pipeline = new AlexaRequestPipeline(
    new[]{
        new LaunchHandler(),
        new BuySpecificProductHandler(),
        new BuyRequestHandler(),
        new IntentHandler(),
        new FallbackHandler()
    }
)

return await pipeline.Process(skillRequest, context);
```

Side note - another advantage of having handlers perform logic is that your executing environment doesn't need to know about the logic its executing, functionality can be tweaked and reordered by the order of the handlers without any alterations to the project that handles the Alexa requests.

# Pre-packaged handlers
Although you can create handlers for yourself if you wish, there are several types of handler already available as base classes.

*    LaunchRequestHandler - CanHandle set up to look for launch requests
*    IntentNameRequestHandler - takes a constructor parameter of the intent name and returns true on an exact match
*    AlwaysTrueRequestHandler - Good as a final item in the list, a catch all that always returns true to ensure you never have requests fail without some handled response

(all these have a synchronous version that doesn't require await/async functionality)

# What is the AlexaRequestInformation object in the interface?

The AlexaRequestInformation object is several pieces of information about the execution.

*    SkillRequest - the request being processed
*    Context - the context object passed in as part of the process call, useful for information from the executing request
*    Items - a dictionary of objects specific to this request, useful for passing information from interceptors (see below)

# Custom Skill requests objects

It may be that you're using a sub-class of the SkillRequest object, for example skills that are APL enabled will be using the APLSkillRequest to get display specific info. In those cases there is a version of both the pipeline and each of the base classes that allows a version of the skill request to be specific, such as `LaunchRequestHandler<APLSkillRequest>`

# Using error handlers

As well as handlers for requests, you can also register error handlers. These work in a similar way to RequestHandlers, allowing for specific request/exception combinations to be handled regardless of which of your handlers generated the exception in question. These are registered as an optional second argument in the pipeline constructor.

```csharp
public interface IAlexaErrorHandler
{
    bool CanHandle(AlexaRequestInformation information, Exception exception);
    Task<SkillResponse> Handle(AlexaRequestInformation information, Exception exception);      
}
```

# Creating request interceptors

There are times where you want to write logic that is run regardless of which handler is selected. Potentially you could wrap this logic around the pipeline, but that would require alteration of the execution environment. So instead you have request and error interceptors.

Interceptors have a single method, which are aware of the AlexaSkillInformation object. As multiple interceptors can be registered it passes in one more argument - the next item in the chain.

```csharp
public interface IAlexaRequestInterceptor
{
    Task<SkillResponse> Intercept(AlexaRequestInformation information, RequestInterceptorCall next);
}
```

So for example you could have a request interceptor that executes a stopwatch, and stops just before returning the SkillResponse object to the user to see how long handlers run for:

```csharp
public class TimingHandlerInterceptor : IAlexaRequestInterceptor<SkillRequest>
{
    public async Task<SkillResponse> Intercept(AlexaRequestInformation<SkillRequest> information, RequestInterceptorCall<SkillRequest> next)
    {
        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            return await next(information);
       }
        finally
        {
            stopwatch.Stop();
        }
    }
}
```

# Handler aware interceptors

You also have situations where you want logic to occur under specific conditions - dependent on the handler selected, but ideally this should be kept isolated from the handlers themselves. To allow for this there is a handler aware request interceptor, which implements a second method which should be used instead.

```csharp
Task<SkillResponse> Intercept(AlexaRequestInformation information, IAlexaRequestHandler handler, RequestInterceptorCall next);
```

The handler is passed in to help with logic **you do not need to execute the handler yourself** All the interceptor has to do is execute the `next` argument with the parameters - when the full chain is executed the `next` parameter will execute the handler as the last step in the chain.

If you want to pass information outside of the interceptor without altering the handler directly (as that would tightly couple your handler to your interceptor) then you can use the `AlexaSkillInformation.Items` dictionary.

```csharp
public Task<SkillResponse> Intercept(AlexaRequestInformation<SkillRequest> information, IAlexaRequestHandler<SkillRequest> handler, RequestInterceptorCall<SkillRequest> next)
{
    if (handler is PurchaseHandlerBaseClass)
    {
        var productCatalog = GetProductData();
        information.Items.Add("productData", productCatalog);
    }

    return next(information);
}
```
