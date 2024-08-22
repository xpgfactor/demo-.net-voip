using Microsoft.AspNetCore.SignalR;

namespace SignalR
{
    public class ListenerHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Send", message);
        }
    }
}
