using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.RequestHandlers.Interceptors;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.KeyLocale
{
    public class KeyLocaleInterceptor:IAlexaRequestInterceptor,IKeyLocaleStore
    {
        public IKeyLocaleStore[] Stores { get; set; }

        public KeyLocaleInterceptor(IKeyLocaleStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            Stores = new[] {store};
        }

        public KeyLocaleInterceptor(IEnumerable<IKeyLocaleStore> stores)
        {
            Stores = stores.ToArray();
            if (!Stores.Any())
            {
                throw new InvalidOperationException("No stores found");
            }
        }

        public async Task<SkillResponse> Intercept(AlexaRequestInformation information, RequestInterceptorCall next)
        {
            var response = await next(information);
            return response;
        }

        public bool Supports(string locale)
        {
            throw new NotImplementedException();
        }

        public Task<IOutputSpeech> Translate(string key, string locale)
        {
            throw new NotImplementedException();
        }
    }
}
