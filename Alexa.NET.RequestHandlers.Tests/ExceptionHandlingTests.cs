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
            var request = new AlexaRequestPipeline();
            await Assert.ThrowsAsync<ArgumentNullException>(() => request.Process(null));
        }

        [Fact]
        public async Task EmptyRequestHandlersHandledByErrors()
        {
            var request = new AlexaRequestPipeline();
			await Assert.ThrowsAsync<AlexaRequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task EmptyRequestHandlerThrowsWhenSet()
        {
			var errorHandler = Substitute.For<AlwaysTrueErrorHandler>();
			errorHandler.Handle(Arg.Any<AlexaRequestInformation>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var request = new AlexaRequestPipeline(null,new[]{errorHandler}) { RequestHandlerTriggersErrorHandlers = false };
            await Assert.ThrowsAsync<AlexaRequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task EmptyErrorHandlerThrowsOnException()
        {
            var requestHandler = Substitute.For<AlwaysTrueRequestHandler>();
            requestHandler.Handle(Arg.Any<AlexaRequestInformation<SkillRequest>>()).Throws<InvalidOperationException>();
            var request = new AlexaRequestPipeline(new[] { requestHandler });
			await Assert.ThrowsAsync<InvalidOperationException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task NoValidRequestHandlerThrowsException()
        {
            var requestHandler = Substitute.For<IAlexaRequestHandler>();
            requestHandler.CanHandle(Arg.Any<AlexaRequestInformation>()).Returns(false);
            var request = new AlexaRequestPipeline(new[] { requestHandler });
			await Assert.ThrowsAsync<AlexaRequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

        [Fact]
        public async Task NoValidErrorHandlerThrowsException()
        {
            var requestHandler = Substitute.For<IAlexaRequestHandler>();
            requestHandler.CanHandle(Arg.Any<AlexaRequestInformation>()).Returns(false);

            var errorHandler = Substitute.For<IAlexaErrorHandler>();
            errorHandler.CanHandle(Arg.Any<AlexaRequestInformation>(), Arg.Any<Exception>()).Returns(false);

            var request = new AlexaRequestPipeline(new[] { requestHandler }, new[] { errorHandler });
			await Assert.ThrowsAsync<AlexaRequestHandlerNotFoundException>(() => request.Process(new SkillRequest()));
        }

		[Fact]
        public async Task ValidErrorHandlerReturnsResponse()
		{
			var errorHandler = Substitute.For<AlwaysTrueErrorHandler<SkillRequest>>();
            errorHandler.Handle(Arg.Any<AlexaRequestInformation<SkillRequest>>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var request = new AlexaRequestPipeline(null, new[] { errorHandler });
			var response = await request.Process(new SkillRequest());
			Assert.NotNull(response);
		}

		[Fact]
        public async Task InvalidResponseValidErrorHandlerReturnsResponse()
        {
			var requestHandler = Substitute.For<IAlexaRequestHandler<SkillRequest>>();
            requestHandler.CanHandle(Arg.Any<AlexaRequestInformation<SkillRequest>>()).Returns(false);

            var errorHandler = Substitute.For<AlwaysTrueErrorHandler<SkillRequest>>();
            errorHandler.Handle(Arg.Any<AlexaRequestInformation<SkillRequest>>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var request = new AlexaRequestPipeline(new[]{requestHandler}, new[] { errorHandler });
            var response = await request.Process(new SkillRequest());
            Assert.NotNull(response);
        }

		[Fact]
        public async Task SpecificExceptionHandlerPicks()
        {
			var requestHandler = Substitute.For<AlwaysTrueRequestHandler<SkillRequest>>();
			requestHandler.Handle(Arg.Any<AlexaRequestInformation<SkillRequest>>()).Throws<InvalidOperationException>();


			var specificError = Substitute.For<IAlexaErrorHandler>();
			specificError.CanHandle(Arg.Any<AlexaRequestInformation<SkillRequest>>(), Arg.Any<InvalidOperationException>()).Returns(true);
			specificError.Handle(Arg.Any<AlexaRequestInformation<SkillRequest>>(), Arg.Any<Exception>()).Returns(new Response.SkillResponse());
			var errorHandler = Substitute.For<AlwaysTrueErrorHandler>();


            var request = new AlexaRequestPipeline(new[] { requestHandler }, new IAlexaErrorHandler<SkillRequest>[] { specificError,errorHandler });
            var response = await request.Process(new SkillRequest());
            Assert.NotNull(response);
        }
    }
}
