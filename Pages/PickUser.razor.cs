using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CC.Data;
using CC.Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using MudBlazor;
using Microsoft.JSInterop;

namespace CC.Pages
{
    public class PickUserModel : ComponentBase, IDisposable
    {
        protected bool Busy { get; set; }
        protected List<UserProfile>? UserProfiles { get; set; } = new List<UserProfile>();
        protected bool mandatory = true;
        protected MudChip? selected;
        protected DataContext? Context { get; set; }

        protected string _username = string.Empty;

        [Inject] protected NavigationManager? NavigationManager { get; set; }
        [Inject] protected IDbContextFactory<DataContext>? DbFactory { get; set; }
        [Inject] protected ConnectedUser? ConnectedUser { get; set; }
        [Inject] protected IJSRuntime? JSRuntime { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        //protected async void PickMember2(UserProfile userProfile)
        //{
        //    if (ConnectedUser != null)
        //    {
        //        //CookiesManager cookiesManager = new CookiesManager(JSRuntime);
        //        //await cookiesManager.WriteCookie("memberName", userProfile.Name);
        //        ConnectedUser.UserProfile = userProfile;
        //        NavigationManager?.NavigateTo("toto");
        //    }
        //}

        protected void PickUserProfile(UserProfile userProfile)
        {
            if (ConnectedUser != null)
            {
                ConnectedUser.UserProfile = userProfile;
                NavigationManager?.NavigateTo("toto");
            }
        }

        [Inject]
        protected UserManager<IdentityUser>? IdentityUserManager { get; set; }

        override protected async Task OnInitializedAsync()
        {

            Busy = true;
            if (AuthenticationStateTask == null)
            {
                throw new Exception("AuthenticationStateTask is null");
            }
            if (IdentityUserManager == null)
            {
                throw new Exception("IdentityUserManager is null");
            }
            var user = (await AuthenticationStateTask).User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var currentUser = await IdentityUserManager.GetUserAsync(user) ?? throw new Exception("User is null");

                _username = currentUser.Email ?? throw new Exception("currentUser.Email is null");

                try
                {
                    if (DbFactory != null && Context == null)
                    {
                        Context = DbFactory.CreateDbContext();

                        if (Context is not null && Context.UserProfiles is not null)
                        {
                            UserProfiles = await Context.UserProfiles.ToListAsync();
                        }
                    }
                }
                finally
                {
                    Busy = false;
                }
            }
            else
            {
                throw new Exception("User is not logged in");
            }

            await base.OnInitializedAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
