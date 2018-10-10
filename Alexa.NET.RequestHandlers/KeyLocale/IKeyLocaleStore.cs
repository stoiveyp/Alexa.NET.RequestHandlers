using System;
using System.Threading.Tasks;
using Alexa.NET.Response;

namespace Alexa.NET.RequestHandlers.KeyLocale
{
    public interface IKeyLocaleStore
    {
        bool Supports(string locale);

        Task<IOutputSpeech> Translate(string key, string locale);
    }
}