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
			await Assert.ThrowsAsync<RequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task EmptyRequestHandlerThrowsWhenSet()
        {
			var errorHandler = Substitute.For<AlwaysTrueErrorHandler>();
			errorHandler.Handle(Arg.Any<RequestInformation>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var request = new Request(null,new[]{errorHandler}) { RequestHandlerTriggersErrorHandlers = false };
            await Assert.ThrowsAsync<RequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task EmptyErrorHandlerThrowsOnException()
        {
            var requestHandler = Substitute.For<AlwaysTrueRequestHandler>();
            requestHandler.Handle(Arg.Any<RequestInformation>()).Throws<InvalidOperationException>();
            var request = new Request(new[] { requestHandler });
			await Assert.ThrowsAsync<InvalidOperationException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task NoValidRequestHandlerThrowsException()
        {
            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<RequestInformation>()).Returns(false);
            var request = new Request(new[] { requestHandler });
			await Assert.ThrowsAsync<RequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task NoValidErrorHandlerThrowsException()
        {
            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<RequestInformation>()).Returns(false);

            var errorHandler = Substitute.For<IErrorHandler>();
            errorHandler.CanHandle(Arg.Any<RequestInformation>(), Arg.Any<Exception>()).Returns(false);

            var request = new Request(new[] { requestHandler }, new[] { errorHandler });
			await Assert.ThrowsAsync<RequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

		[Fact]
        public async Task ValidErrorHandlerReturnsResponse()
		{
			var errorHandler = Substitute.For<AlwaysTrueErrorHandler>();
            errorHandler.Handle(Arg.Any<RequestInformation>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var request = new Request(null, new[] { errorHandler });
			var response = await request.Process(new SkillRequest());
			Assert.NotNull(response);
		}

		[Fact]
        public async Task InvalidResponseValidErrorHandlerReturnsResponse()
        {
			var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<RequestInformation>()).Returns(false);

            var errorHandler = Substitute.For<AlwaysTrueErrorHandler>();
            errorHandler.Handle(Arg.Any<RequestInformation>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var request = new Request(new[]{requestHandler}, new[] { errorHandler });
            var response = await request.Process(new SkillRequest());
            Assert.NotNull(response);
        }

		[Fact]
        public async Task SpecificExceptionHandlerPicks()
        {
			var requestHandler = Substitute.For<AlwaysTrueRequestHandler>();
			requestHandler.Handle(Arg.Any<RequestInformation>()).Throws<InvalidOperationException>();


			var specificError = Substitute.For<IErrorHandler>();
			specificError.CanHandle(Arg.Any<RequestInformation>(), Arg.Any<InvalidOperationException>()).Returns(true);
			specificError.Handle(Arg.Any<RequestInformation>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var errorHandler = Substitute.For<AlwaysTrueErrorHandler>();


            var request = new Request(new[] { requestHandler }, new[] { specificError,errorHandler });
            var response = await request.Process(new SkillRequest());
            Assert.NotNull(response);
        }
    }
}
