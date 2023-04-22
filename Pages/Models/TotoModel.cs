using CC.Helpers;
using CC.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;

namespace CC.Pages.Models
{
    public class TotoModel : ComponentBase
    {
        [Inject] NavigationManager? NavigationManager { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        [Inject]
        protected UserManager<IdentityUser>? IdentityUserManager { get; set; }
        [Inject] protected ConnectedUser? ConnectedUser { get; set; }

        // flag to indicate chat status
        protected bool _isChatting = false;

        // name of the user who will be chatting
        protected string _username = string.Empty;

        // on-screen message
        protected string _message = string.Empty;

        // new message input
        protected string _newMessage = string.Empty;

        // list of messages in chat
        protected List<Message> _messages = new();

        protected string _hubUrl = string.Empty;
        protected HubConnection? _hubConnection;

        override protected async Task OnInitializedAsync()
        {
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
                //var currentUserId = currentUser.Id;
                //var currentUserEmail = currentUser.Email;
                //var currentUserPhone = currentUser.PhoneNumber;
                //var currentUserEmailConfirmed = currentUser.EmailConfirmed;

                _username = currentUser.Email ?? throw new Exception("currentUser.Email is null");
                if (ConnectedUser != null && ConnectedUser.Member != null)
                {
                    _username = ConnectedUser.Member.Name;
                    await Chat();
                }
            }
            else
            {
                throw new Exception("User is not logged in");
            }
        }

        public async Task Chat()
        {
            try
            {
                if (NavigationManager == null)
                {
                    return;
                }
                // Start chatting and force refresh UI.
                _isChatting = true;
                await Task.Delay(1);

                // remove old messages if any
                _messages.Clear();

                // Create the chat client
                string baseUrl = NavigationManager.BaseUri;

                _hubUrl = baseUrl.TrimEnd('/') + ChatHub.HubUrl;
                _hubConnection = new HubConnectionBuilder().WithUrl(_hubUrl).Build();
                _hubConnection.On<string, string>("Broadcast", BroadcastMessage);

                await _hubConnection.StartAsync();

                await SendAsync($"[Notice] {_username} joined chat room.");
            }
            catch (Exception e)
            {
                _message = $"ERROR: Failed to start chat client: {e.Message}";
                _isChatting = false;
            }
        }

        protected void BroadcastMessage(string name, string message)
        {
            bool isMine = name.Equals(_username, StringComparison.OrdinalIgnoreCase);

            _messages.Add(new Message(name, message, isMine));

            // Inform blazor the UI needs updating
            InvokeAsync(StateHasChanged);
        }

        protected async Task DisconnectAsync()
        {
            if (_hubConnection == null)
            {
                throw new NullReferenceException("HubConnection is null");
            }
            if (_isChatting)
            {
                await SendAsync($"[Notice] {_username} left chat room.");
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();

                _hubConnection = null;
                _isChatting = false;
            }
        }

        protected async Task SendAsync(string message)
        {
            if (_isChatting && !string.IsNullOrWhiteSpace(message))
            {
                if (_hubConnection == null)
                {
                    throw new NullReferenceException("_hubConnection is null");
                }
                await _hubConnection.SendAsync("Broadcast", _username, message);

                _newMessage = string.Empty;
            }
        }

        protected class Message
        {
            public Message(string username, string body, bool mine)
            {
                Username = username;
                Body = body;
                Mine = mine;
            }

            public string Username { get; set; }
            public string Body { get; set; }
            public bool Mine { get; set; }

            public bool IsNotice => Body.StartsWith("[Notice]");

            public string CSS => Mine ? "sent" : "received";
        }
    }
}
