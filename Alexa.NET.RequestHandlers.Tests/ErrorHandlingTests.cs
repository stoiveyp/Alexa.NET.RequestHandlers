using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using Xunit;

namespace Alexa.NET.RequestHandlers.Tests
{
    public class ErrorHandlingTests
    {
        [Fact]
        public async Task LLTest()
        {
            var log = Substitute.For<IRequestHandlerInterceptor>();
            log.Intercept(Arg.Any<SkillRequest>(),Arg.Any<RequestInterceptorCall>()).Returns(async c =>
             {
                 Console.WriteLine("before");
                 var nextResult = await c.Arg<RequestInterceptorCall>().Invoke(c.Arg<SkillRequest>());
                 Console.WriteLine("after");
                 return nextResult;
             });

            var handler = Substitute.For<AlwaysTrueRequestHandler>();
            handler.Handle(Arg.Any<SkillRequest>()).Returns(c => ResponseBuilder.Empty());

            var req = new Request(new[] { handler }, null, new[] { log }, null);
            await req.Process(new SkillRequest());
        }
    }
}
