﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CC.Data;

namespace CC.Models
{
    public class PickUserModel : ComponentBase, IDisposable
    {
        //private readonly CC.Data.CCContext _context;
        protected Boolean Busy { get; set; }
        protected List<Member>? Users { get; set; } = new List<Member>();

        //public PickUserModel(CC.Data.CCContext context)
        //{
        //    _context = context;
        //}
        protected CCContext? Context { get; set; }
        // name of the user who will be chatting
        protected string _username = string.Empty;
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] IDbContextFactory<CCContext>? DbFactory { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        //[Inject]
        //protected UserManager<IdentityUser>? IdentityUserManager { get; set; }

        override protected async Task OnInitializedAsync()
        {

            Busy = true;
            if (AuthenticationStateTask == null)
            {
                throw new Exception("AuthenticationStateTask is null");
            }
            //if (IdentityUserManager == null)
            //{
            //    throw new Exception("IdentityUserManager is null");
            //}
            //var user = (await AuthenticationStateTask).User;
            //if (user.Identity != null && user.Identity.IsAuthenticated)
            //{
            //    var currentUser = await IdentityUserManager.GetUserAsync(user) ?? throw new Exception("User is null");
            //    var currentUserId = currentUser.Id;
            //    var currentUserEmail = currentUser.Email;
            //    var currentUserPhone = currentUser.PhoneNumber;
            //    var currentUserEmailConfirmed = currentUser.EmailConfirmed;

            //    _username = currentUser.Email ?? throw new Exception("currentUser.Email is null");

            //    try
            //    {
            //        if (DbFactory != null && Context == null)
            //        {
            //            Context = DbFactory.CreateDbContext();

            //            if (Context is not null && Context.Members is not null)
            //            {
            //                Users = await Context.Members.ToListAsync();
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        Busy = false;
            //    }
            //}
            //else
            //{
            //    throw new Exception("User is not logged in");
            //}

            await base.OnInitializedAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}