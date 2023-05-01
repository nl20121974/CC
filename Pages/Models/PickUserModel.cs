﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CC.Data;
using CC.Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using MudBlazor;
using Microsoft.JSInterop;

namespace CC.Pages.Models
{
    public class PickUserModel : ComponentBase, IDisposable
    {
        protected bool Busy { get; set; }
        protected List<Member>? Members { get; set; } = new List<Member>();
        protected bool mandatory = true;
        protected MudChip? selected;
        protected CCContext? Context { get; set; }

        protected string _username = string.Empty;

        [Inject] protected NavigationManager? NavigationManager { get; set; }
        [Inject] protected IDbContextFactory<CCContext>? DbFactory { get; set; }
        [Inject] protected ConnectedUser? ConnectedUser { get; set; }
        [Inject] protected IJSRuntime? JSRuntime { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        protected async void PickMember2(Member member)
        {
            if (ConnectedUser != null)
            {
                CookiesManager cookiesManager = new CookiesManager(JSRuntime);
                await cookiesManager.WriteCookie("memberName", member.Name);
                ConnectedUser.Member = member;
                NavigationManager?.NavigateTo("toto");
            }
        }

        protected void PickMember(Member member)
        {
            if (ConnectedUser != null)
            {
                ConnectedUser.Member = member;
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

                        if (Context is not null && Context.Members is not null)
                        {
                            Members = await Context.Members.ToListAsync();
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
