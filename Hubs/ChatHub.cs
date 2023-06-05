using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BienenstockCorpAPI.Hubs
{
    public class ChatHub : Hub
    {
        #region Constructor
        public ChatHub() { }
        #endregion

        #region Methods
        public override Task OnConnectedAsync()
        {
            var query = Context.GetHttpContext()?.Request.Query;

            Groups.AddToGroupAsync(Context.ConnectionId, query["HubCode"].ToString());

            return base.OnConnectedAsync();
        }
        #endregion
    }
}
