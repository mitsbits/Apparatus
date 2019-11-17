using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polemic.Hubs
{
    public class BallotsHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} joined the conversation");
            return base.OnConnectedAsync();
        }

        public async Task JoinRoom(string topic)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, topic);
        }

        public async Task LeaveRoom(string topic)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topic);
        }
        public void Send(string name, string message)
        {
            Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
