using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CC.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static MudBlazor.Colors;
using System.Net;

namespace CC.Areas.Identity.Pages.Account
{
    public class SetUserProfileModel : PageModel
    {
        private readonly DataContext _DataContext;
        private readonly ILogger<LoginModel> _logger;

        public SetUserProfileModel(DataContext DataContext, ILogger<LoginModel> logger)
        {
            _DataContext = DataContext;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = new();

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; } = string.Empty;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            public string MemberName { get; set; } = string.Empty;
        }

        public void OnGet(string returnUrl)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("UserId is null");
            }
            UserProfiles = _DataContext.UserProfiles.Where(m => m.UserId == userId).ToList();
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["memberName"]))
            {
                UserProfiles = _DataContext.UserProfiles.Where(m => m.UserId == userId).ToList();
            }
            else if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["memberName"]))
            {
                if (UserProfiles.Any(m => m.Name == HttpContext.Request.Cookies["memberName"]))
                {
                    LocalRedirect(returnUrl);
                }
            }

            ReturnUrl = returnUrl;
        }

        public IActionResult OnPost(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Set the expiration date and path of the cookie:
                HttpContext.Response.Cookies.Append("memberName", Input.MemberName, new CookieOptions {Expires = DateTime.Now.AddDays(1), Path = "/"});
                return LocalRedirect(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
