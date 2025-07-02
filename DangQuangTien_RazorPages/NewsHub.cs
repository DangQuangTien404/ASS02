using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages.Hubs
{
    public class NewsHub : Hub
    {
        public async Task BroadcastNewArticle(string title, string url)
        {
            await Clients.All.SendAsync("ReceiveNewArticle", title, url);
        }
    }
}
