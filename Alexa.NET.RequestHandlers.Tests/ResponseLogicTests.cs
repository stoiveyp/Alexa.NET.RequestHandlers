using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using NSubstitute;
using Xunit;

namespace Alexa.NET.RequestHandlers.Tests
{
    public class ResponseLogicTests
    {
        [Fact]
        public async Task ValidFirstRequestReturnsResponse()
        {
            var expected = ResponseBuilder.Empty();

            var requestHandler = Substitute.For<IAlexaRequestHandler>();
			requestHandler.CanHandle(Arg.Any<AlexaRequestInformation>()).Returns(true);
			requestHandler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(expected);

            var errorHandler = Substitute.For<IAlexaErrorHandler>();
			errorHandler.CanHandle(Arg.Any<AlexaRequestInformation>(), Arg.Any<Exception>()).Returns(false);

            var request = new AlexaRequestPipeline(new[] { requestHandler }, new[] { errorHandler });
            var actual = await request.Process(new SkillRequest());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ValidSecondRequestReturnsSecondResponse()
        {
            var expected = ResponseBuilder.Empty();

            var alwaysFalse = Substitute.For<IAlexaRequestHandler>();
			alwaysFalse.CanHandle(Arg.Any<AlexaRequestInformation>()).Returns(false);

            var requestHandler = Substitute.For<IAlexaRequestHandler>();
			requestHandler.CanHandle(Arg.Any<AlexaRequestInformation>()).Returns(true);
			requestHandler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(expected);

            var errorHandler = Substitute.For<IAlexaErrorHandler>();
			errorHandler.CanHandle(Arg.Any<AlexaRequestInformation>(), Arg.Any<Exception>()).Returns(false);

            var request = new AlexaRequestPipeline(new[] { alwaysFalse, requestHandler }, new[] { errorHandler });
            var actual = await request.Process(new SkillRequest());
            Assert.Equal(expected, actual);
        }
    }
}

