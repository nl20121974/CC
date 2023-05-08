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
        [Inject] protected NavigationManager? NavigationManager { get; set; }

        public void OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["memberName"]))
                {
                    RedirectToPage("/Account/SetUserProfile");
                }
            }
        }
    }
}
