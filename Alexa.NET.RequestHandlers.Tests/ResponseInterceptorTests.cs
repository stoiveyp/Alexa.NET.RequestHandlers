using System;
using System.Threading.Tasks;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Request;
using Alexa.NET.Response;
using NSubstitute;
using Xunit;

namespace Alexa.NET.RequestHandlers.Tests
{
    public class ResponseInterceptorTests
    {

		IAlexaRequestHandler Handler { get; }
		public ResponseInterceptorTests()
		{
			Handler = Substitute.For<AlwaysTrueRequestHandler>();
		}

		[Fact]
        public async Task EmptyInterceptorReturnsHandler()
		{
			var expected = new SkillResponse();
			Handler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(expected);
			var request = new AlexaRequestPipeline(new[] { Handler }, null);
			var actual = await request.Process(new SkillRequest());
			Assert.Equal(expected, actual);
		}

        [Fact]
        public async Task OneInterceptorCallsCorrectly()
		{
			var expected = new SkillResponse();
			var before = string.Empty;
			Handler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(c =>
			{
				before = before + "2";
				return expected;
			});

			var interceptor = Substitute.For<IAlexaRequestInterceptor>();
			interceptor.Intercept(Arg.Any<AlexaRequestInformation>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
				before = before + "1";
				var actual = c.Arg<RequestInterceptorCall>()(c.Arg<AlexaRequestInformation>());
				before = before + "3";
				return actual;
			});

			var request = new AlexaRequestPipeline(new[] { Handler }, null,new[]{interceptor},null);
            await request.Process(new SkillRequest());
            Assert.Equal("123", before);
		}

		[Fact]
        public async Task TwoInterceptorCallsCorrectly()
        {
            var expected = new SkillResponse();
            var before = string.Empty;
			Handler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(c =>
            {
                before = before + "3";
                return expected;
            });

            var interceptor = Substitute.For<IAlexaRequestInterceptor>();
			interceptor.Intercept(Arg.Any<AlexaRequestInformation>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "1";
				var actual = c.Arg<RequestInterceptorCall>()(c.Arg<AlexaRequestInformation>());
                before = before + "5";
                return actual;
            });

			var secondInterceptor = Substitute.For<IAlexaRequestInterceptor>();
			secondInterceptor.Intercept(Arg.Any<AlexaRequestInformation>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "2";
				var actual = c.Arg<RequestInterceptorCall>()(c.Arg<AlexaRequestInformation>());
                before = before + "4";
                return actual;
            });

			var request = new AlexaRequestPipeline(new[] { Handler }, null, new[] { interceptor,secondInterceptor }, null);
            await request.Process(new SkillRequest());
            Assert.Equal("12345", before);
        }

		[Fact]
        public async Task TwoInterceptorCallsInOrder()
        {
            var expected = new SkillResponse();
            var before = string.Empty;
			Handler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(c =>
            {
                before = before + "3";
                return expected;
            });

            var interceptor = Substitute.For<IAlexaRequestInterceptor>();
			interceptor.Intercept(Arg.Any<AlexaRequestInformation>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "1";
				var actual = c.Arg<RequestInterceptorCall>()(c.Arg<AlexaRequestInformation>());
                before = before + "5";
                return actual;
            });

            var secondInterceptor = Substitute.For<IAlexaRequestInterceptor>();
			secondInterceptor.Intercept(Arg.Any<AlexaRequestInformation>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "2";
				var actual = c.Arg<RequestInterceptorCall>()(c.Arg<AlexaRequestInformation>());
                before = before + "4";
                return actual;
            });

			var request = new AlexaRequestPipeline(new[] { Handler }, null, new[] { secondInterceptor,interceptor }, null);
            await request.Process(new SkillRequest());
            Assert.Equal("21354", before);
        }
    }
}
