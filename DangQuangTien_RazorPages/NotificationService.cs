using DangQuangTien_RazorPages.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages
{
    public class NotificationService
    {
        private readonly IHubContext<NewsHub> _hub;

        public NotificationService(IHubContext<NewsHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAsync()
        {
            await _hub.Clients.All.SendAsync("ReceiveNotification");
        }
    }
}