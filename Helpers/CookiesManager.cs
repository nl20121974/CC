using Microsoft.Build.Execution;
using Microsoft.JSInterop;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace CC.Helpers
{
    public class CookiesManager
    {
        private readonly IJSRuntime JsRuntime;

        public CookiesManager(IJSRuntime JsRuntime)
        {
            this.JsRuntime = JsRuntime;
        }

        public async Task WriteCookie(string cookieName, string cookieValue)
        {
            await JsRuntime.InvokeAsync<object>("WriteCookie.WriteCookie", cookieName, cookieValue, DateTime.Now.AddMinutes(1));
        }

        public async Task<string> ReadCookie(string cookieName)
        {
            string cookieValue = await JsRuntime.InvokeAsync<string>("ReadCookie.ReadCookie", cookieName);
            return cookieValue;
        }
    }
}
