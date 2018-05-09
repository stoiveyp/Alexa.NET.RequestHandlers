using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.RequestHandlers.Handlers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Alexa.NET.RequestHandlers.Tests
{
    public class ExceptionHandlingTests
    {
        [Fact]
        public async Task ArgumentNullExceptionIfNoRequest()
        {
            var request = new Request();
            await Assert.ThrowsAsync<ArgumentNullException>(() => request.Process(null));
        }

        [Fact]
        public async Task EmptyRequestHandlersHandledByErrors()
        {
            var request = new Request();
            await Assert.ThrowsAsync<ErrorHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task EmptyRequestHandlerThrowsWhenSet()
        {
            var request = new Request { RequestHandlerTriggersErrorHandlers = false };
            await Assert.ThrowsAsync<RequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task EmptyErrorHandlerThrowsOnException()
        {
            var requestHandler = Substitute.For<AlwaysTrueRequestHandler>();
            requestHandler.Handle(Arg.Any<SkillRequest>()).Throws<InvalidOperationException>();
            var request = new Request(new[] { requestHandler }, null);
            await Assert.ThrowsAsync<ErrorHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task NoValidRequestHandlerThrowsException()
        {
            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<SkillRequest>()).Returns(false);
            var request = new Request(new[] { requestHandler }, null);
            await Assert.ThrowsAsync<ErrorHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task NoValidErrorHandlerThrowsException()
        {
            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<SkillRequest>()).Returns(false);

            var errorHandler = Substitute.For<IErrorHandler>();
            errorHandler.CanHandle(Arg.Any<SkillRequest>(), Arg.Any<Exception>()).Returns(false);

            var request = new Request(new[] { requestHandler }, new[] { errorHandler });
            await Assert.ThrowsAsync<ErrorHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }
    }
}
