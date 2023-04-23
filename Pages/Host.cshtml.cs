using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CC.Pages
{
    public class Host : PageModel
    {
        public Host()
        {
            var message = HttpContext.Request.PathBase;
        }
        public void OnGet()
        {
            var message = HttpContext.Request.PathBase;

            // ...
        }
    }
}
