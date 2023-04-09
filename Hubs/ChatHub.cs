using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Security.Principal;

namespace CC.Hubs
{
    public class ChatHub : Hub
    {
        public const string HubUrl = "/chat";
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        public override async Task OnConnectedAsync()
        {
            //Groups.Add(Context.ConnectionId, name);
            Console.WriteLine($"{Context.ConnectionId} connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            if (e != null)
                await base.OnDisconnectedAsync(e);
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task Join(string username)
        {
            if (!_connections.GetConnections(username).Contains(Context.ConnectionId))
            {
                _connections.Add(username, Context.ConnectionId);
                await Clients.AllExcept(Context.ConnectionId).SendAsync("UserJoined", username, $"{username} joined the chat");
            }
        }

        public async Task Leave(string username)
        {
            if (!_connections.GetConnections(username).Contains(Context.ConnectionId))
            {
                _connections.Remove(username, Context.ConnectionId);
                await Clients.AllExcept(Context.ConnectionId).SendAsync("UserLeaved", username, $"{username} leaved the chat");
            }
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        private IIdentity getIdentity()
        {
            if (Context.User == null)
            {
                throw new Exception("Context.User is null");
            }
            if (Context.User.Identity == null)
            {
                throw new Exception("Context.User.Identity is null");
            }
            return Context.User.Identity;
        }
    }
}
