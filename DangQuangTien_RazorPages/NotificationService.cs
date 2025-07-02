using DangQuangTien_RazorPages.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages
{
    public class NotificationService
    {
        private readonly IHubContext<NewsHub> _hubContext;

        public NotificationService(IHubContext<NewsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAsync()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification");
        }
    }
}