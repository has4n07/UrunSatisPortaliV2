using Microsoft.AspNetCore.SignalR;

namespace UrunSatisPortali.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderNotification()
        {
            await Clients.All.SendAsync("ReceiveOrderNotification");
        }
    }
}
