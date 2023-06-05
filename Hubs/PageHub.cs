using Microsoft.AspNetCore.SignalR;

namespace BienenstockCorpAPI.Hubs
{
    public class PageHub : Hub
    {
        #region Constructor
        public PageHub() { }
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
