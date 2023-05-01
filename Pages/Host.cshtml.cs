using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using CC.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace CC.Pages
{
    public partial class Host : PageModel
    {
        [Inject] protected IHttpContextAccessor? HttpContextAccessor { get; set; }
        [Inject] protected IJSRuntime? JsRuntime { get; set; }
        [Inject] protected NavigationManager? NavigationManager { get; set; }

        [Inject]
        SignInManager<IdentityUser>? SignInManager { get; set; }

        public async void OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["memberName"]))
                {
                    //NavigationManager.NavigateTo("pickuser");
                }
            }
        }
        public void click()
        {
            RedirectToAction("pickuser");
        }
        public string myCookieValue { get; set; } = "";
    }
}
