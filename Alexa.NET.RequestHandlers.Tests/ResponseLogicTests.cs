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

            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<SkillRequest>()).Returns(true);
            requestHandler.Handle(Arg.Any<SkillRequest>()).Returns(expected);

            var errorHandler = Substitute.For<IErrorHandler>();
            errorHandler.CanHandle(Arg.Any<SkillRequest>(), Arg.Any<Exception>()).Returns(false);

            var request = new Request(new[] { requestHandler }, new[] { errorHandler });
            var actual = await request.Process(new SkillRequest());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ValidSecondRequestReturnsSecondResponse()
        {
            var expected = ResponseBuilder.Empty();

            var alwaysFalse = Substitute.For<IRequestHandler>();
            alwaysFalse.CanHandle(Arg.Any<SkillRequest>()).Returns(false);

            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.CanHandle(Arg.Any<SkillRequest>()).Returns(true);
            requestHandler.Handle(Arg.Any<SkillRequest>()).Returns(expected);

            var errorHandler = Substitute.For<IErrorHandler>();
            errorHandler.CanHandle(Arg.Any<SkillRequest>(), Arg.Any<Exception>()).Returns(false);

            var request = new Request(new[] { alwaysFalse, requestHandler }, new[] { errorHandler });
            var actual = await request.Process(new SkillRequest());
            Assert.Equal(expected, actual);
        }
    }
}

