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

		IRequestHandler Handler { get; }
		public ResponseInterceptorTests()
		{
			Handler = Substitute.For<AlwaysTrueRequestHandler>();
		}

		[Fact]
        public async Task EmptyInterceptorReturnsHandler()
		{
			var expected = new SkillResponse();
			Handler.Handle(Arg.Any<SkillRequest>()).Returns(expected);
			var request = new Request(new[] { Handler }, null);
			var actual = await request.Process(new SkillRequest());
			Assert.Equal(expected, actual);
		}

        [Fact]
        public async Task OneInterceptorCallsCorrectly()
		{
			var expected = new SkillResponse();
			var before = string.Empty;
			Handler.Handle(Arg.Any<SkillRequest>()).Returns(c =>
			{
				before = before + "2";
				return expected;
			});

			var interceptor = Substitute.For<IRequestHandlerInterceptor>();
			interceptor.Intercept(Arg.Any<SkillRequest>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
				before = before + "1";
				var actual = c.Arg<RequestInterceptorCall>()(c.Arg<SkillRequest>());
				before = before + "3";
				return actual;
			});

			var request = new Request(new[] { Handler }, null,new[]{interceptor},null);
            await request.Process(new SkillRequest());
            Assert.Equal("123", before);
		}

		[Fact]
        public async Task TwoInterceptorCallsCorrectly()
        {
            var expected = new SkillResponse();
            var before = string.Empty;
            Handler.Handle(Arg.Any<SkillRequest>()).Returns(c =>
            {
                before = before + "3";
                return expected;
            });

            var interceptor = Substitute.For<IRequestHandlerInterceptor>();
            interceptor.Intercept(Arg.Any<SkillRequest>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "1";
                var actual = c.Arg<RequestInterceptorCall>()(c.Arg<SkillRequest>());
                before = before + "5";
                return actual;
            });

			var secondInterceptor = Substitute.For<IRequestHandlerInterceptor>();
			secondInterceptor.Intercept(Arg.Any<SkillRequest>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "2";
                var actual = c.Arg<RequestInterceptorCall>()(c.Arg<SkillRequest>());
                before = before + "4";
                return actual;
            });

			var request = new Request(new[] { Handler }, null, new[] { interceptor,secondInterceptor }, null);
            await request.Process(new SkillRequest());
            Assert.Equal("12345", before);
        }

		[Fact]
        public async Task TwoInterceptorCallsInOrder()
        {
            var expected = new SkillResponse();
            var before = string.Empty;
            Handler.Handle(Arg.Any<SkillRequest>()).Returns(c =>
            {
                before = before + "3";
                return expected;
            });

            var interceptor = Substitute.For<IRequestHandlerInterceptor>();
            interceptor.Intercept(Arg.Any<SkillRequest>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "1";
                var actual = c.Arg<RequestInterceptorCall>()(c.Arg<SkillRequest>());
                before = before + "5";
                return actual;
            });

            var secondInterceptor = Substitute.For<IRequestHandlerInterceptor>();
            secondInterceptor.Intercept(Arg.Any<SkillRequest>(), Arg.Any<RequestInterceptorCall>()).Returns(c => {
                before = before + "2";
                var actual = c.Arg<RequestInterceptorCall>()(c.Arg<SkillRequest>());
                before = before + "4";
                return actual;
            });

			var request = new Request(new[] { Handler }, null, new[] { secondInterceptor,interceptor }, null);
            await request.Process(new SkillRequest());
            Assert.Equal("21354", before);
        }
    }
}
