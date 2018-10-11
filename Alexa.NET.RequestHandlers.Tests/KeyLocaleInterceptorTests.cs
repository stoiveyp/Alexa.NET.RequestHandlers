using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.RequestHandlers.KeyLocale;
using Alexa.NET.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace Alexa.NET.RequestHandlers.Tests
{
    public class KeyLocaleInterceptorTests
    {
        [Fact]
        public void NullKeyLocaleStoreThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new KeyLocaleInterceptor((IKeyLocaleStore)null));
        }

        [Fact]
        public void EmptyLocaleStoreThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => new KeyLocaleInterceptor(new IKeyLocaleStore[]{}));
        }

        [Fact]
        public async Task IgnoresResponseIfNoKeyOutputFound()
        {
            var response = ResponseBuilder.Empty();
            var fakeStore = Substitute.For<IKeyLocaleStore>();
            var result = await RunPipeline(fakeStore, response);

            Assert.True(JToken.DeepEquals(JObject.FromObject(response), JObject.FromObject(result)));
            fakeStore.DidNotReceive().Supports(Arg.Any<string>());
        }

        [Fact]
        public async Task KeySpeechTriggersSupportCheck()
        {

        }

        private Task<SkillResponse> RunPipeline(IKeyLocaleStore store, SkillResponse response)
        {

            var handler = Substitute.For<AlwaysTrueRequestHandler>();
            handler.Handle(Arg.Any<AlexaRequestInformation>()).Returns(response);

            var pipeline = new AlexaRequestPipeline(new[] { handler },
                null,
                new[] { new KeyLocaleInterceptor(store) },
                null);

            return pipeline.Process(new SkillRequest());
        }
    }
}
